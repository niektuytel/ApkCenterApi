
using System.Linq;

namespace ApkCenterAdminApi.Models
{
    public class SimpleAppModel
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public double Star { get; set; }

    }

    public class AppModel
    {

        public string Title { get; set; }
        public string Category { private get; set; }
        public WebsiteModel Website { private get; set; } = new WebsiteModel();
        public string WebsiteUrl {
            get { return Website.Url; } 
        }
        public ApkModel Apk { get; set; }
        public string[] Types { private get; set; }
        public string[] Populars { private get; set; }
        public bool Popular { get; private set; }
        public string[] TypenApis { private get; set; }
        public string Limit { get; set; }

        public SimpleAppModel SimpleApp()
        {
            return new SimpleAppModel{ 
                Title = Title, 
                Icon = Apk.Icon, 
                Star = Apk.Reviews.Star 
            };
        }

        public string GetCategory()
        {
            return Category;
        }
        public WebsiteModel GetWebsite()
        {
            return Website;
        }
        public string[] GetTypes()
        {
            return Types;
        }
        public string[] GetTypenApis()
        {
            return TypenApis;
        }

        public bool SetPopular(string country)
        {
            Popular = Populars.Contains(country);
            return Popular;
        }

    }
}
