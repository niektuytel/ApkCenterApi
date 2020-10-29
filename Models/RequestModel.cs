using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{
    public class RequestModel
    {
        public string Title { get; set; }
        public int RequestedTimes { get; set; } = 0;
    }
}
