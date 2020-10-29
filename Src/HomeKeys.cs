using ApkCenterAdminApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Src
{
    // NOT USING YET
    public class HomeKeys
    {
        private const string _Filename = Program.DataDirectory + "HomeKeys.json";
        public HomeModel AllHomeKeys;

        private readonly FileHelper _fileHelper;
        public HomeKeys()
        {
            _fileHelper = new FileHelper();
            AllHomeKeys = new HomeModel
            {
                Names = _fileHelper.LoadJson<Dictionary<string, List<string>>>(_Filename),
            };
        }

        public (int, List<string>) GetHomeKeys(string country)
        {
            var keys = AllHomeKeys.Names;
            if (!keys.ContainsKey(country))
            {
                return (404, null);
            }

            return (200, keys[country]);
        }

        //public (int, Dictionary<string, List<SimpleAppModel>>) GetHomeApps(string apiVersion, string country)
        //{
        //    var keys = AllHomeKeys.Names;
        //    if (!keys.ContainsKey(country) || !keys.ContainsKey("Globally"))
        //    {
        //        return (404, null);
        //    }

        //    // add apps to it
        //    Dictionary<string, List<SimpleAppModel>> apps = new Dictionary<string, List<SimpleAppModel>>();
        //    for (int i = 0; i < keys[country].Count; i++)
        //    {
        //        string defaultKey = keys["Globally"][i];
        //        string currentKey = keys[country][i];
        //        List<SimpleAppModel> keyApps = AllHomeKeys.Apps[apiVersion][country][defaultKey];

        //        apps.Add(currentKey, keyApps);
        //    }

        //    return (200, apps);
        //}

    }
}
