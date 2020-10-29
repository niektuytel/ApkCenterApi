using System;
using System.IO;
using System.Threading.Tasks;
using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    public class ApkController : _ControllerBase
    {
        // GET  : {api version}/apkcenter/apk/apk/{application title}
        [HttpGet("{apiVersion}/apkcenter/[controller]/{title}")]
        public async Task<IActionResult> Download(string apiVersion, string title)
        {
            title = Uri.EscapeDataString(title);
            AppModel appModel = Program.MyApps.GetApp(apiVersion, "Globally", title);
            if (appModel is null || appModel.Title is null)
            {
                return NotFound("Title not found");
            }


            string dirFolder = appModel.Title.ToUpper()[0].ToString();
            string filename = Program.AppsDirectory + "Data/" + dirFolder + "/" + appModel.Title + "/" + appModel.Apk.Url;

            var memory = new MemoryStream();
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "multipart/form-data", appModel.Apk.Url);
        }
    }


}
