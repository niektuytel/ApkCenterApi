using ApkCenterAdminApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Src
{
    //NOT USING YET
    public class Populars
    {

        public Dictionary<string, Dictionary<string, List<SimpleAppModel>>> AllPopulars;

        private readonly FileHelper _fileHelper;

        public Populars()
        {
            _fileHelper = new FileHelper();
            AllPopulars = new Dictionary<string, Dictionary<string, List<SimpleAppModel>>>();

            LoadFromFiles();
           
        }

        private void LoadFromFiles()
        {
            foreach (string path in Directory.GetDirectories(Program.AppsDirectory))
            {
                string filename = path + "Populars.json";
                string version = path.Split("\\")[^1].Split("/")[^1];
                if (!File.Exists(filename)) continue;

                AllPopulars.Add(version, _fileHelper.LoadJson<Dictionary<string, List<SimpleAppModel>>>(filename));
            }
        }
    }
}
