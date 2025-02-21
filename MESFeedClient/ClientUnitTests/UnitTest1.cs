using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ClientUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        HttpClient client = new HttpClient();
        [TestMethod]
        public void TestPost()
        {
            //Debugger.Launch();
            //TestKronos();

            //Debugger.Launch();
            var result = Task.Run(TaskAsync).GetAwaiter().GetResult();
            Console.WriteLine(result);

        }
        private async Task<string> TaskAsync()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => { return true; };

                using (var client = new HttpClient())
                {


                    string xmlRequest = @"<Message>
<TransactionType><![CDATA[ProductDownload]]></TransactionType>
<TransactionId><![CDATA[7c7875fd-5861-4ce9-a8d4-656a27339558]]></TransactionId>
<TransactionDateTime><![CDATA[2021-06-22T16:34:51.1787392-04:00]]></TransactionDateTime>
<Product>
<Name><![CDATA[Z01]]></Name>
<Revision><![CDATA[-]]></Revision>
<UseROR><![CDATA[false]]></UseROR>
</Product>
<Description><![CDATA[ASSY,ADJUSTABLE REGULATOR]]></Description>
<Status><![CDATA[1]]></Status>
<ProductType><![CDATA[SA]]></ProductType>
<ProductFamily><![CDATA[Oasis]]></ProductFamily>
<StartQuantity><![CDATA[10]]></StartQuantity>
<StartQuantityUOM><![CDATA[EA]]></StartQuantityUOM>
</Message>
";


                    var byteArray = new UTF8Encoding().GetBytes("CamstarAdmin:SDesw65$53@.?");

                    //client.BaseAddress = 
                    var uri = new Uri("https://usmer-dmesapp01.getingegroup.local:4401/opexcradapter/messages?wait=true");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var content = new StringContent(xmlRequest, Encoding.UTF8, "application/xml");
                    Console.WriteLine(content.ReadAsStringAsync());


                    var result = await client.PostAsync(uri, content);
                    Debug.WriteLine(result);
                    return await result.Content.ReadAsStringAsync();
                    //Console.WriteLine(resultContent);

                }
            }
        }

        //private async void TestKronos()
        //{
        //    using (client)
        //    {
        //        //var response = await client.GetStringAsync("https://jsonplaceholder.typicode.com/todos");
        //        //Console.WriteLine(response);

        //        var uri = new Uri("https://secure.saashr.com/ta/rest/v1/company/profiles");

        //        var creds = new Credentials();
        //        var content = new StringContent(JsonConvert.SerializeObject(creds), Encoding.UTF8, "application/json");
        //        content.Headers.Add("Api-Key", creds.apikey);

        //        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        //        request.Headers.Add("Api-Key", creds.apikey);


        //        //content.Headers.Add("Accept", "application/json");
        //        //content.Headers.Add("Content-Type", "application/json");
        //        //client.DefaultRequestHeaders.Add("Api-Key", creds.apikey);

        //        var response = await client.PostAsync(uri, content);

        //        Console.WriteLine(response);
        //        var source = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine(source);
        //    };
        //}
    }
}
