using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebApplication1.Controllers
{
    public class MomWorkOrderController : ApiController
    {

        [HttpGet]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IEnumerable<string> Get()
        {
            return new string[] { "hello world"};
        }

        [HttpPost]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Post([FromBody]string workorder)
        {
            //check for valid  data from mes
            if(workorder == null || workorder == "") { return BadRequest(); }
            //send the  data to oracle and log activity
            
            //return response
            return Ok(workorder);
        }

        [HttpPut]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Put([FromUri] string id, [FromBody] string workorder)
        {
            return Ok(workorder);
        }
        [HttpDelete]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult delete([FromUri] string id)
        {
            return NotFound();
        }
    }
}
