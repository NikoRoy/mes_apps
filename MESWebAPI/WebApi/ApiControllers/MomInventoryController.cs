using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class MomInventoryController : ApiController
    {
        // GET: api/MomInventory
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MomInventory/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MomInventory
        public void Post([FromBody]string value)
        {
            //update oracle
        }

        // PUT: api/MomInventory/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/MomInventory/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
