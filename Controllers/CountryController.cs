using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    [Route("admin/apkcenter/[controller]")]
    public class CountryController : _ControllerBase
    {

        // GET : /admin/apkcenter/country
        [HttpGet]
        public List<string> GetAll()
        {
            return Program.MyCountry.AllCountries;
        }
    }
}
