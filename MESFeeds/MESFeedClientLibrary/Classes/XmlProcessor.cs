using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Classes
{
    public class XmlProcessor : IXmlProcessor
    {
        public string Root { get; private set; }
        public string URI { get;  set; }

        public HttpClient Client { get; private set; }

        public XmlProcessor(string url)
        {
            Root = url;
            URI = "";
            Client = new HttpClient();
        }
        public void Dispose()
        {
            this.Client.Dispose();
        }

        public async Task<string> Execute(string xmlRequest)
        {
            var httpContent = new StringContent(xmlRequest, Encoding.UTF8, "application/xml");
            var request = new HttpRequestMessage(HttpMethod.Post, Root);
            request.Content = httpContent;

            try
            {
                var response = await this.Client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
            finally
            {
                httpContent.Dispose();
                request.Dispose();
            }
        }
    }
}
