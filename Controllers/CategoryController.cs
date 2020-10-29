using System.Collections.Generic;
using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    public class CategoryController : _ControllerBase
    {
        // POST : /admin/apkcenter/category
        [HttpPost("admin/apkcenter/[controller]")]
        [Consumes("application/json")]
        public ObjectResult Create([FromBody] Dictionary<string, string> categories)
        {
            var (code, message) = Program.MyCategories.AddCategory(categories);
            return StatusCode(code, message);
        }

        // POST : /admin/apkcenter/category/{oldCategory}
        [HttpPost("admin/apkcenter/[controller]/{oldCategory}")]
        [Consumes("application/json")]
        public ObjectResult Edit(string oldCategory, [FromBody] Dictionary<string, string> categories)
        {
            var (code, message) = Program.MyCategories.AddCategory(categories, oldCategory);
            return StatusCode(code, message);
        }

        // GET : /admin/apkcenter/category/countries 
        [HttpGet("admin/apkcenter/[controller]/countries")]
        public Dictionary<string, Dictionary<string, string>> GetAllCountries()
        {
            return Program.MyCategories.AllCategories.Countries;
        }

        // GET : /{apiVersion}/apkcenter/category/{country}/categories 
        [HttpGet("{apiVersion}/apkcenter/[controller]/{country}/categories")]
        public Dictionary<string, string> GetCategories(string _, string country)
        {
            Dictionary<string, Dictionary<string, string>> countries = Program.MyCategories.AllCategories.Countries;
            if (!countries.ContainsKey(country))
            {
                return null;
            }

            return countries[country];
        }

        // GET : /{apiVersion}/apkcenter/category/{ category }/apps 
        [HttpGet("{apiVersion}/apkcenter/[controller]/{category}/apps")]
        public List<SimpleAppModel> GetApps(string apiVersion, string category, int beginIndex = 0, int endIndex = 20)
        {
            return Program.MyCategories.GetApps(apiVersion, category, beginIndex, endIndex);
        }

        // DELETE : /admin/apkcenter/category/{globallyKey}
        [HttpDelete("admin/apkcenter/[controller]/{category}")]
        public ObjectResult Delete(string category)
        {
            var (code, message) = Program.MyCategories.DeleteCategory(category);
            return StatusCode(code, message);
        }
    }
}
