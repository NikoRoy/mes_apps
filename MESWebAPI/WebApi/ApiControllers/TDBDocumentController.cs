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
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Route("controller")]
    public class TDBDocumentController : ApiController
    {
        List<TDBDocument> docs = new List<TDBDocument>()
        {
            new TDBDocument{DocumentID="MP000001", Revision="AA", Description="TEST Description", URL="TEST URL"},
            new  TDBDocument{DocumentID="MP000002", Revision="AA", Description="TEST Description", URL="TEST URL"}
        };

        [HttpGet]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IEnumerable<TDBDocument> Get()
        {
            //var docAccess = new TDBMESFeeds.DataAccess.DocumentQuery(ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
            //return docAccess.GetDocumentDownloads();
            return docs;
        }
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var doc = docs.FirstOrDefault((p) => p.DocumentID == id);
            if (doc == null)
            {
                return NotFound();
            }
            return Ok(doc);
        }
        [HttpPost]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok(value);
        }
   

        [HttpPut]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Put([FromUri]string id, [FromBody]string value)
        {
            return Ok(value);
        }

        [HttpDelete]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public IHttpActionResult Delete([FromBody]string value)
        {
            return Ok(value);
        }
    }
}
