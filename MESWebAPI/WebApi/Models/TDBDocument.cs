using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Models
{
    public class TDBDocument 
    {
        public string DocumentID { get; set; }
        public string Revision { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public TDBDocument() { }
    }
}