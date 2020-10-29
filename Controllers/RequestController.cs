using ApkCenterAdminApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApkCenterAdminApi.Controllers
{
    public class RequestController: _ControllerBase
    {
        // POST : {api version}/apkcenter/request/{app title}
        [HttpPost("{apiVersion}/apkcenter/[controller]/{appTitle}")]
        public ObjectResult AddRequest(string apiVersion, string appTitle)
        {
            var (code, message) = Program.MyRequests.AddRequest(apiVersion, appTitle);
            return StatusCode(code, message);
        }

        // GET: admin/apkcenter/request
        [HttpGet("admin/apkcenter/[controller]")]
        public List<RequestModel> GetAll()
        {
            return Program.MyRequests.AllRequests;
        }

        // DELETE: admin/apkcenter/request
        [HttpDelete("admin/apkcenter/[controller]")]
        [Consumes("application/json")]
        public ObjectResult Delete([FromBody] RequestModel data)
        {
            var (code, message) = Program.MyRequests.DeleteRequest(data.Title);
            return StatusCode(code, message);
        }
    }
}
