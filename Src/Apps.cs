using ApkCenterAdminApi.Src;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{
    
    public class Apps
    {
        private const string _AppsFolder = Program.AppsDirectory + "Data/";

        public Dictionary<string, List<AppModel>> AllApps { get; private set; } = new Dictionary<string, List<AppModel>>();

        private readonly FileHelper _fileHelper;
        private readonly MatchHelper _matchHelper;

        
        public Apps()
        {
            _fileHelper = new FileHelper();
            _matchHelper = new MatchHelper();

            // create apps
            {
                foreach (string dirPaths in Directory.GetDirectories(_AppsFolder))
                {
                    string dir = dirPaths[^1].ToString();
                    AllApps.Add(dir, new List<AppModel>());
                    foreach (string filePaths in Directory.GetDirectories(dirPaths))
                    {
                        string filename = filePaths + "\\" + filePaths.Split("/")[^1].Split("\\")[^1] + ".json";
                        AppModel model = _fileHelper.LoadJson<AppModel>(filename);
                        AllApps[dir].Add(model);
                    }
                }
            }
        }

        public (int, string) DeleteAdd(AppModel model)
        {
            string title = model.Title;
            string dirFolder = title.ToUpper()[0].ToString();

            if(!AllApps.ContainsKey(dirFolder))
            {
                return (404, "Failed Title not found");
            }

            for (int i = 0; i < AllApps[dirFolder].Count; i++)
            {
                string storedTitle = AllApps[dirFolder][i].Title;
                if (storedTitle == title)
                {
                    // remove app from files
                    string path = _AppsFolder + dirFolder + "/" + title;
                    Directory.Delete(path,true);
                    AllApps[dirFolder].RemoveAt(i);
                    bool succeedded = Program.MyCategories.DeleteApp(title);

                    return (200, $"Request Deletion succeedded:{ succeedded }");
                }
            }

            return (404, "Failed Title not found");
        }

        public async Task<(int, string)> AddApp(List<IFormFile> files, string appTitle)
        {
            string appDir = appTitle.ToUpper()[0].ToString();
            string folder = _AppsFolder + appDir + "/" + appTitle + "/";
            string jsonFile = "";

            var (code, message) = CheckCorrectFiles(files);
            if (code != 200) return (code, message);

            // create files
            {
                Directory.CreateDirectory(folder);
                if(!AllApps.ContainsKey(appDir))
                {
                    AllApps.Add(appDir, new List<AppModel>());
                }

                int fileIndex = 0;
                foreach (IFormFile file in files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    string name = file.FileName.Split("\\")[^1].Split("/")[^1];
                    string filename = folder + name;
                    if (filename.EndsWith(".json"))
                    {
                        jsonFile = filename;
                    }

                    fileIndex++;
                    using FileStream stream = new FileStream(filename, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                // check
                if(fileIndex == 0 || fileIndex != files.Count || jsonFile == "")
                {
                    Directory.Delete(folder);
                    return (404, "Files that are uploaded is not equal the files that you are given !");
                }
            }
            
            // add app to allApps
            AppModel newModel = _fileHelper.LoadJson<AppModel>(jsonFile);
            {
                int matched = _matchHelper.Matched(AllApps[appDir], newModel);

                if (matched == -1)
                {
                    AllApps[appDir].Add(newModel);
                } else {
                    AllApps[appDir][matched] = newModel;
                }
            }

            // add app to categories
            foreach (string version in newModel.GetTypenApis())
            {
                (code, message) = Program.MyCategories.AddApp(version, newModel, appTitle);
                if(code != 200) return (code, message);
            }

            return (200, "Adding app succeeded");
        }

        public AppModel GetApp(string apiVersion, string country, string appTitle)
        {
            AppModel returnApp = new AppModel();
            string dir = appTitle.ToUpper()[0].ToString();

            if (!AllApps.ContainsKey(dir))
                return null;

            foreach (AppModel app in AllApps[dir])
            {
                if(app.Title == appTitle)
                {
                    foreach (string apiType in app.GetTypenApis())
                    {
                        if (apiType == apiVersion)
                        {
                            app.SetPopular(country);
                            app.Apk.SetAbout(country);
                            returnApp = app;
                            break;
                        }
                    }
                    break;
                }
            }

            return returnApp;
        }

        private (int, string) CheckCorrectFiles(List<IFormFile> files)
        {
            if (files.Count == 1 && files[0].FileName.EndsWith(".json"))
            {
                return ( 200, "Found 1 json file" );
            }
            else if (files.Count == 2)
            {
                bool containsJson = false;
                bool containsApk = false;
                foreach (IFormFile file in files)
                {
                    if (file.FileName.EndsWith(".json") && !containsJson)
                    {
                        containsJson = true;
                    }
                    else if (file.FileName.EndsWith(".apk") && !containsApk)
                    {
                        containsApk = true;
                    }
                    else
                    {
                        return (404, "Failed files extentions not contains: 1 json & 1 apk");
                    }
                }
            }
            else
            {
                return (404, "Failed maximum 2 Files can been requested containing [json, apk]");
            }
            
            return ( 200, "Found 2 files [json, apk]");
        }

    }
}
