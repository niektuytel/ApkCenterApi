using System.Collections.Generic;
using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApkCenterAdminApi.Controllers
{
    public class ErrorController : _ControllerBase
    {
        // POST : /{api version}/apkcenter/error
        [HttpPost("{apiVersion}/apkcenter/[controller]")]
        [Consumes("application/json")]
        public ObjectResult AddError(string apiVersion, [FromBody]ErrorModel body)
        {
            var (code, message) = Program.MyErrors.AddError(apiVersion, body);
            return StatusCode(code, message);
        }

        // GET: /admin/apkcenter/error
        [HttpGet("admin/apkcenter/[controller]")]
        public Dictionary<string, List<ErrorModel>> GetAll()
        {
            return Program.MyErrors.AllErrors;
        }

        // DELETE: /admin/apkcenter/error
        [HttpDelete("admin/apkcenter/[controller]")]
        public ObjectResult Delete(ErrorModel errorModel)
        {
            var (code, message) = Program.MyErrors.DeleteError(errorModel);
            return StatusCode(code, message);
        }

    }
}
