using System.Collections.Generic;

namespace ApkCenterAdminApi.Src
{
    public class Country
    {
        public List<string> AllCountries { get; private set; }

        public Country()
        {
            AllCountries = new List<string>();
            var countries = Program.MyCategories.AllCategories.Countries;
            foreach (string country in countries.Keys)
            {
                AllCountries.Add(country);
            }
        }
    }
}
