using BusinessLayer.Controller;
using OracleMESFeeds.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;

namespace WebApplication1.Controllers
{
    public class OracleMessageController : ApiController
    {
        protected static HttpClient Client = new HttpClient();

        [HttpGet]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Get()
        {
            //return new string[] { "hello world" };
            return Ok(WorkOrderDownloadQuery.GetWorkOrders());
        }
   
        [HttpPost]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public async Task<IHttpActionResult> Post([FromBody]XElement message)
        {
            try
            {
                //check for valid data from mes
                if(message == null ) { return BadRequest("null request"); }
                //send mes data to oracle and log activity
                Enum.TryParse<OracleRecordType>(message.Element("TransactionType").Value, out var ort);
                var ora = OracleController.GetTypeFromFactory(ort);

                bool res = await ora.Create(message);
                if (!res)
                {
                    return InternalServerError(); //BadRequest();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                
                return await Task.Run(() => InternalServerError(ex));
            }
        }
        [HttpPut]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public async Task<IHttpActionResult> Put([FromUri] string id, [FromBody] XElement message)
        {
            try
            {
                //check for valid data from mes
                if (message == null ) { return BadRequest("null request"); }
                //send mes data to oracle and log activity
                Enum.TryParse<OracleRecordType>(message.Element("TransactionType").Value, out var ort);
                var ora = OracleController.GetTypeFromFactory(ort);

                bool res = await ora.UpdateAsync(id, message);
                if (!res)
                {
                    return await Task.Run(() => BadRequest());
                }
                return await Task.Run(() => Ok(res));
            }
            catch (Exception ex)
            {
                return await Task.Run(() => InternalServerError(ex));
            }
        }
        [HttpDelete]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult delete([FromUri] string id)
        {
            return Unauthorized();
        }
    }
}
