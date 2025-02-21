using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IXmlProcessor : IDisposable
    {
        string URI { get; set; }      
        Task<string> Execute(string xmlRequest);
    }
}
