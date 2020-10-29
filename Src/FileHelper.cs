using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ApkCenterAdminApi.Src
{
    public class FileHelper
    {
        public FileHelper()
        { }

        public List<T> UpdateStorage<T>(string filename, List<T> values)
        {

            if(!File.Exists(filename))
            {
                StoreJson(filename, values);
                return values;
            }

            if (values.Count == 0)
            {
                return LoadJson<List<T>>(filename);
            }
            else
            {
                StoreJson(filename, values);
            }

            return values;
        }

        public Dictionary<string, Dictionary<string, string>> UpdateStorage(string filename, Dictionary<string, Dictionary<string, string>> dictionary)
        {
            if (!File.Exists(filename))
            {
                StoreJson(filename, dictionary);
                return dictionary;
            }

            if (dictionary.Keys.Count == 0)
            {
                return LoadJson<Dictionary<string, Dictionary<string, string>>>(filename);
            }
            else
            {
                StoreJson(filename, dictionary);
            }

            return dictionary;
        }

        public Dictionary<string, string> UpdateStorage(string filename, Dictionary<string, string> dictionary)
        {
            if (!File.Exists(filename))
            {
                StoreJson(filename, dictionary);
                return dictionary;
            }

            if (dictionary.Keys.Count == 0)
            {
                return LoadJson<Dictionary<string, string>>(filename);
            }
            else
            {
                StoreJson(filename, dictionary);
            }

            return dictionary;
        }

        public Dictionary<string, List<T>> UpdateStorage<T>(string filename, Dictionary<string,List<T>> dictionary)
        {
            if (!File.Exists(filename))
            {
                StoreJson(filename, dictionary);
                return dictionary;
            }


            bool hasInformation = false;
            foreach (KeyValuePair<string, List<T>> entry in dictionary)
            {
                if (entry.Value.Count != 0)
                {
                    hasInformation = true;
                    break;
                }
            }

            if (!hasInformation)
            {
                return LoadJson<Dictionary<string, List<T>>>(filename);
            } else {
                StoreJson(filename, dictionary);
            }

            return dictionary;
        }

        public T LoadJson<T>(string filename)
        {
            using StreamReader r = new StreamReader(filename);
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void StoreJson<T>(string filename, T data)
        {
            using StreamWriter file = File.CreateText(filename);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, data);
        }

    }
}
