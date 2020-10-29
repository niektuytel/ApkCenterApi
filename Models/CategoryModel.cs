using System.Collections.Generic;
using System;

namespace ApkCenterAdminApi.Models
{
    public class CategoryModel
    {
        public Dictionary<string, Dictionary<string, string>> Countries { get; set; }

        //version //category //list<(title,downloads)>
        public Dictionary<string, Dictionary<string, List<(string, long)>>> Apps { get; set; }
    }
}
