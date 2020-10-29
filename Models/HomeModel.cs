using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// NOT USING YET
namespace ApkCenterAdminApi.Models
{
    public class HomeModel
    {
        // country
        public Dictionary<string, List<string>> Names { get; set; }

        // apiversion // country // globally key
        public Dictionary<string, Dictionary<string, Dictionary<string, List<SimpleAppModel>>>> Apps { get; set; }

    }
}
