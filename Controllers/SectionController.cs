
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    public class SectionController : _ControllerBase
    {

        //[HttpGet("{apiVersion}/apkcenter/home/{country}/names")]
        //public IActionResult HomeKeyNames(string _, string country)
        //{
        //    var (code, keys) = Program.MyHomeKeys.GetHomeKeys(country);

        //    if (code == 404)
        //    {
        //        return NotFound("country not found");
        //    }
                
        //    return Ok(keys);
        //}


        //[HttpGet("{apiVersion}/apkcenter/home/{country}/apps")]
        //public IActionResult HomeApps(string apiVersion, string country)
        //{
        //    var (code, apps) = Program.MyHomeKeys.GetHomeApps(apiVersion, country);

        //    if(code == 404)
        //    {
        //        return NotFound("`country / api version` not found");
        //    }

        //    return Ok(apps);
        //}
    }
}
