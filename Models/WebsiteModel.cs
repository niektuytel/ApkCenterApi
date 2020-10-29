using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{
    public interface IWebsiteModel
    {
        string Url { get; set; }
    }

    public class WebsiteModel
    {
        public string Url { get; set; }
    }
}
