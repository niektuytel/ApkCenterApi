using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    [Route("{apiVersion}/apkcenter/[controller]")]
    public class SearchController : _ControllerBase
    {
        // GET : /{api version}/apkcenter/search/{category}?title=
        [HttpGet("{category}")]
        public IActionResult GetSearchTitle(string apiVersion, string category, [FromQuery]string title)
        {
            if (title is null)
            {
                return Ok(new List<SimpleAppModel>());
            }

            title = Uri.EscapeDataString(title);
            return Program.MySearch.SearchTitle(apiVersion, title, category);
        }

        // GET : /{api version}/apkcenter/search?title=
        [HttpGet]
        public IActionResult GetSearchTitle(string apiVersion, [FromQuery] string title)
        {
            if(title is null)
            {
                return Ok(new List<SimpleAppModel>());
            }

            title = Uri.EscapeDataString(title);
            return Program.MySearch.SearchTitle(apiVersion, title);
        }
    }
}
