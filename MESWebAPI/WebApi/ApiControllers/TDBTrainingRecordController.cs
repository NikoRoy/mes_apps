using BusinessLayer.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;

namespace WebApplication1.ApiControllers
{
    public class TDBTrainingRecordController : ApiController
    {
        [HttpGet]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Get()
        {
            return NotFound();
        }

        [HttpPost]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public async Task<IHttpActionResult> Post([FromBody]XElement message)
        {
            try
            {
                //check for valid data from mes
                if (message == null) { return BadRequest("null request"); }
                //send tdb data to mes endpoint then loopback to update tdb sync field on success
                var tdb = new TDBRecordController();

                var res = await tdb.Create(message);
                if (res.Contains("Failure"))
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {

                return await Task.Run(() => InternalServerError(ex));
            }
        }
    }
}
