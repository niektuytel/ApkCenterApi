using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Controllers
{
    public class AppController : _ControllerBase
    {
        private readonly Apps _apps;

        public AppController()
        {
            _apps = Program.MyApps;
        }

        // GET : {api version}/apkcenter/app/<country>/<app title>
        [HttpGet("{apiVersion}/apkcenter/[controller]/{country}/{appTitle}")]
        public AppModel GetAppByTitle(string apiVersion, string country, string appTitle)
        {
            string title = Uri.EscapeDataString(appTitle);
            AppModel model = _apps.GetApp(apiVersion, country, title);

            return model;
        }

        // POST admin/apkcenter/app/<app title>
        [HttpPost("admin/apkcenter/[controller]/{appTitle}")]
        [Consumes("multipart/form-data")]
        public async Task<ObjectResult> CreateApp(List<IFormFile> files, string appTitle)
        {
            string title = Uri.EscapeDataString(appTitle);
            var (code, message) = await _apps.AddApp(files, title);
            return StatusCode(code, message);
        }

        // DELETE admin/apkcenter/app
        [HttpDelete("admin/apkcenter/[controller]")]
        [Consumes("application/json")]
        public ObjectResult Delete(AppModel appModel)
        {
            var (code, message) = _apps.DeleteAdd(appModel);
            return StatusCode(code, message);
        }
    }
}
