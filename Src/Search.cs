using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace ApkCenterAdminApi.Src
{
    public class Search : ControllerBase
    {
        public Search()
        { }

        public IActionResult SearchTitle(string apiVersion, string input, string category = "")
        {
            bool containsCategory = category != "";
            var apps = Program.MyCategories.AllCategories.Apps;
            List<string> appValues = new List<string>();

            if(!apps.ContainsKey(apiVersion))
            {
                return NotFound("api version is invalid");
            }
            else if(containsCategory && !apps[apiVersion].ContainsKey(category))
            {
                return NotFound("category is invalid");
            }

            // search value in category
            if(containsCategory)
            {
                return Ok(CaptureValues(apps[apiVersion][category], input, out _));
            }
            // search value is category
            else if(!containsCategory && apps[apiVersion].ContainsKey(input))
            {
                foreach (var (title, _) in apps[apiVersion][input])
                {
                    appValues.Add(title);
                }

                //limit
                if (appValues.Count > 40)
                    return Ok(appValues.GetRange(0, 40));

                return Ok(appValues);
            }

            // search value in categories
            (int, int, int) size = (0, 0, 0);
            foreach (string key in apps[apiVersion].Keys)
            {
                List<string> values = CaptureValues(apps[apiVersion][key], input, out (int,int,int) newSize);

                if(newSize.Item1 != 0)//start
                {
                    int index = size.Item1;
                    appValues.InsertRange(index, values.GetRange(0, newSize.Item1));
                }

                if (newSize.Item2 != 0)//contains
                {
                    int index = size.Item1 + size.Item2;
                    appValues.InsertRange(index, values.GetRange(newSize.Item1, newSize.Item2));
                }

                if (newSize.Item3 != 0)//ends
                {
                    int index = newSize.Item1 + newSize.Item2;
                    int indexSize = values.Count - index;
                    appValues.AddRange(values.GetRange(index, indexSize));
                }
            }

            //limit
            if (appValues.Count > 40)
                return Ok(appValues.GetRange(0, 40));

            return Ok(appValues);
        }

        private List<string> CaptureValues(List<(string, long)> allValues, string input, out (int,int,int) size)
        {
            input = input.ToUpper();
            size = (0, 0, 0);
            List<string> results = new List<string>();

            foreach (var (appTitle, _) in allValues)
            {
                string title = appTitle.ToUpper();

                if (title.StartsWith(input))
                {
                    int index = size.Item1;
                    results.Insert(index, appTitle);
                    size.Item1++;
                }
                else if(title.EndsWith(input))
                {
                    results.Add(appTitle);
                    size.Item3++;
                }
                else if(title.Contains(input))
                {
                    int index = size.Item1 + size.Item2;
                    results.Insert(index, appTitle);
                    size.Item2++;
                }

                //limit
                if(results.Count >= 20) break;
            }

            return results;
        }

    }
}
