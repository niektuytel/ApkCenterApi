using ApkCenterAdminApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApkCenterAdminApi.Src
{
    public class Categories
    {
        private const string _Filename = Program.DataDirectory + "Categories.json";

        public CategoryModel AllCategories { get; private set; } = new CategoryModel();

        private readonly FileHelper _fileHelper;
        private readonly MatchHelper _matchedHelper;

        public Categories()
        {
            _fileHelper = new FileHelper();
            _matchedHelper = new MatchHelper();

            var countries = new Dictionary<string, Dictionary<string, string>>();
            AllCategories.Countries = _fileHelper.UpdateStorage(_Filename, countries);
            AllCategories.Apps = new Dictionary<string, Dictionary<string, List<(string, long)>>>();

            if (!Directory.Exists(Program.AppsDirectory))
            {
                Directory.CreateDirectory(Program.AppsDirectory);
            }

            // all category apps
            foreach (string versionPaths in Directory.GetDirectories(Program.AppsDirectory))
            {
                string path = versionPaths + "/Categories/";
                if(!Directory.Exists(path)) continue;

                string version = versionPaths.Split("/")[^1].Split("\\")[^1];
                AllCategories.Apps.Add(version, new Dictionary<string, List<(string, long)>>());

                foreach (string categoryFile in Directory.GetFiles(path))
                {
                    List<(string, long)> apps = _fileHelper.UpdateStorage(categoryFile, new List<(string,long)>());
                    string category = categoryFile.Split("\\")[^1].Split("/")[^1].Replace(".json", "");
                    AllCategories.Apps[version].Add(category, apps);
                }
            }
        }

        public (int, string) AddCategory(Dictionary<string, string> categories, string oldCategory = "")
        {
            Dictionary<string, string> countryCategories = categories;
            var (code, message) = CheckCountries(countryCategories);
            if (code != 200) return (code, message);

            string newCategory = countryCategories["Globally"];
            bool isUnique = !AllCategories.Countries["Globally"].ContainsKey(newCategory);
            bool hasOldCategory = AllCategories.Countries["Globally"].ContainsKey(oldCategory);

            // default language/value
            if (isUnique)
            {
                AllCategories.Countries["Globally"].Add( newCategory, newCategory );
            } else {
                AllCategories.Countries["Globally"][newCategory] = newCategory;
            }


            // add category to versions
            string newName = newCategory.ToLower();
            string oldName = oldCategory.ToLower();
            newName = newName.Replace(" & ", "_and_");
            oldName = oldName.Replace(" & ", "_and_");
            AddApps(newName, oldName);

            // other languages/values
            foreach (KeyValuePair<string, string> category in countryCategories)
            {
                string country = category.Key;
                string value = category.Value;

                if (hasOldCategory && oldCategory != newCategory)
                {
                    AllCategories.Countries[country].Remove(oldCategory);
                }

                if (country != "Globally")
                {
                    if (!AllCategories.Countries[country].ContainsKey(newCategory))
                    {
                        AllCategories.Countries[country].Add(newCategory, value);
                    } else {
                        AllCategories.Countries[country][newCategory] = value;
                    }
                }
            }
            AllCategories.Countries = _fileHelper.UpdateStorage(_Filename, AllCategories.Countries);

            return (200, "Succeedded adding category");
        }

        private void AddApps(string newCategory, string oldCategory = "")
        {
            foreach (string versionPath in Directory.GetDirectories(Program.AppsDirectory))
            {
                newCategory = newCategory.ToLower().Replace(" & ", "_and_");
                oldCategory = oldCategory.ToLower().Replace(" & ", "_and_");
                string newFile = versionPath + "/Categories/" + newCategory + ".json";
                string oldFile = versionPath + "/Categories/" + oldCategory + ".json";
                string version = versionPath.Split("/")[^1].Split("\\")[^1];
                List<(string,long)> apps = new List<(string,long)>();

                if (File.Exists(oldFile))
                {
                    apps = AllCategories.Apps[version][oldCategory];
                    AllCategories.Apps[version].Remove(oldCategory);
                    File.Delete(oldFile);
                }

                if (Directory.Exists(versionPath + "/Categories/"))
                {
                    apps = _fileHelper.UpdateStorage(newFile, apps);
                    if (!AllCategories.Apps[version].ContainsKey(newCategory))
                    {
                        AllCategories.Apps[version].Add(newCategory, apps);
                    } else {
                        AllCategories.Apps[version][newCategory] = apps;
                    }
                }
            }
        }

        public (int, string) AddApp(string apiVersion, AppModel appModel, string title)
        {
            string category = appModel.GetCategory().ToLower().Replace(" & ", "_and_");
            string filename = Program.AppsDirectory + apiVersion + "/Categories/" + category + ".json";
            var apps = AllCategories.Apps;

            if(!File.Exists(filename))
            {
                return (404, $"<filename>:{ filename } do not exists");
            }
            else if(!apps.ContainsKey(apiVersion))
            {
                return (404, "Given Api version is not valid");
            }
            else if(!apps[apiVersion].ContainsKey(category))
            {
                return (404, $"Given <category>:{ category } is not founded");
            }

            // add app
            (string, long) currentApp = (title, appModel.Apk.Downloads);
            int insertIndex = GetInsertPosition(apps[apiVersion][category], currentApp, out bool foundMatch);

            if (!foundMatch)
            {
                apps[apiVersion][category].Insert(insertIndex, currentApp);
                _fileHelper.StoreJson(filename, apps[apiVersion][category]);
            }
            
            return (200, "Succeeded added app to categories");
        }

        public (int, string) DeleteCategory(string category)
        {
            var countries = AllCategories.Countries;
            foreach (KeyValuePair<string, Dictionary<string, string>> countrySection in countries)
            {
                string country = countrySection.Key;
                if (countries[country].ContainsKey(category))
                {
                    countries[country].Remove(category);
                }
            }

            _fileHelper.StoreJson(_Filename, countries);
            var (code, message) = DeleteApps(category);
            return (code, message);
        }

        private (int, string) DeleteApps(string category)
        {
            category = category.ToLower().Replace(" & ", "_and_");
            bool called = false;
            foreach (string versionPath in Directory.GetDirectories(Program.AppsDirectory))
            {
                string version = versionPath.Split("/")[^1].Split("\\")[^1];
                string jsonFile = versionPath + "/Categories/" + category + ".json";
                if (!File.Exists(jsonFile)) continue;

                if (!AllCategories.Apps[version].ContainsKey(category))
                {
                    return (404, $"<category>:{ category } is not found in the categories");
                }
                called = true;
                AllCategories.Apps[version].Remove(category);
                File.Delete( jsonFile );
            }

            if(!called)
            {
                return (404, "given category name can't been found");
            }

            return (200, "Successfully deleted category");
        }

        public bool DeleteApp(string title)
        {
            bool didDeletion = false;
            CategoryModel categoryModel = Program.MyCategories.AllCategories;
            foreach (string version in categoryModel.Apps.Keys)
            {
                var versionModel = categoryModel.Apps[version];

                for (int i = 0; i < versionModel.Count; i++)
                {
                    string category = versionModel.ElementAt(i).Key;
                    List<(string, long)> apps = versionModel[category];

                    int matched = -1;// _matchedHelper.Matched(apps, appTitle);
                    for (int j = 0; j < apps.Count; j++)
                    {
                        if(apps[j].Item1 == title)
                        {
                            matched = j;
                            break;
                        }
                    }
                    if (matched == -1) continue;

                    didDeletion = true;
                    string filepath = Program.AppsDirectory + version + "/Categories/";
                    string filename = filepath + category + ".json";

                    // delete app
                    apps.RemoveAt(matched);
                    _fileHelper.StoreJson(filename, apps);
                    Program.MyCategories.AllCategories.Apps[version][category] = apps;
                }
            }

            return didDeletion;
        }

        public List<SimpleAppModel> GetApps(string apiVersion, string category, int beginIndex = 0, int endIndex = 20)
        {
            category = category.ToLower().Replace(" & ", "_and_");
            Dictionary<string, Dictionary<string, List<(string, long)>>> apps = Program.MyCategories.AllCategories.Apps;
            if (!apps.ContainsKey(apiVersion))
            {
                return null;
            }

            if (!apps[apiVersion].ContainsKey(category))
            {
                return null;
            }

            if (endIndex > apps[apiVersion][category].Count)
                endIndex = apps[apiVersion][category].Count;

            if (beginIndex < 0 || beginIndex > endIndex)
                beginIndex = 0;

            List<(string, long)> appTitles = apps[apiVersion][category].GetRange(beginIndex, endIndex);
            List<SimpleAppModel> smallApps = new List<SimpleAppModel>();

            foreach ((string, long) appTitle in appTitles)
            {
                Dictionary<string, List<AppModel>> allApps = Program.MyApps.AllApps;
                string dirFolder = appTitle.Item1[0].ToString();
                if (!allApps.ContainsKey(dirFolder)) continue;

                for (int i = 0; i < allApps[dirFolder].Count; i++)
                {
                    AppModel appModel = allApps[dirFolder][i];
                    if (appModel.Title == appTitle.Item1)
                    {
                        smallApps.Add(appModel.SimpleApp());
                        break;
                    }
                }
            }

            return smallApps;
        }

        private (int, string) CheckCountries(Dictionary<string, string> categories)
        {
            foreach (string country in AllCategories.Countries.Keys)
            {
                if (!categories.ContainsKey(country))
                {
                    return (404, $"Can't find <country>:{ country } in categories");
                }
            }

            if (!categories.ContainsKey("Globally"))
            {
                return (404, $"<country>:Globally value not given");
            }

            return (200, $"valid input has been sended");
        }


        private int GetInsertPosition(List<(string, long)> list, (string, long) value, out bool foundMatch)
        {
            foundMatch = false;
            int insertIndex = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item2 > value.Item2)
                {
                    insertIndex = i + 1;
                }

                if (list[i].Item1 == value.Item1)
                {
                    foundMatch = true;
                    break;
                }
            }
            if (insertIndex == -1 || insertIndex > list.Count)
            {
                insertIndex = list.Count;
            }

            return insertIndex;
        }
      
    }
}
