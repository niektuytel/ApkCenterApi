using System.Collections.Generic;

namespace ApkCenterAdminApi.Models
{
    public class ApkModel
    {
        public string Url { get; set; }
        public string Version { private get; set; }
        public string Icon { get; set; }
        public ReviewModel Reviews { get; set; } = new ReviewModel();
        public long Downloads { get; set; } = 0;
        public int Pegi { get; set; } = 0;
        public string[] Images { get; set; }
        public Dictionary<string, AboutCountryModel> Abouts { private get; set; } = new Dictionary<string, AboutCountryModel>();
        public string About { get; private set; }

        public string GetVersion()
        {
            return Version;
        }
        public string SetAbout(string country)
        {
            if(!Abouts.ContainsKey(country))
            {
                country = "Globally";
            }

            About = Abouts[country].Text;
            return Abouts[country].Text;
        }

    }
}
