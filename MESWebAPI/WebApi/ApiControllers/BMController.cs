using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class BMController : ApiController
    {

        public IHttpActionResult Get()
        {
            return Ok("Hello world");
        }
        public IHttpActionResult Get(string id)
        {
            return Ok("Hello world");
        }
        public IHttpActionResult Post([FromBody] string value)
        {
            return Ok("Hello world");
        }
        public IHttpActionResult Put(string id,[FromBody] string value)
        {
            return Ok("Hello world");
        }
        public IHttpActionResult Delete(string id)
        {
            return Ok("Hello world");
        }
    }
}
