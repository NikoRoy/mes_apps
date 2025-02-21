using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KronosFeedClient
{
    class Program
    {
        public static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            TestKronosLogin().Wait();


            Console.ReadLine();
        }
        private async static Task TestKronosLogin()
        {
            using (client)
            {
                //var response = await client.GetStringAsync("https://jsonplaceholder.typicode.com/todos");
                //Console.WriteLine(response);

                var uri = new Uri("https://secure6.saashr.com:443/ta/rest/v1/login");

                var creds = new Credentials();
                var content = new StringContent(JsonConvert.SerializeObject(new { credentials = creds }), Encoding.UTF8, "application/json");
                content.Headers.Add("Api-Key", creds.apikey);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                

                Console.WriteLine(uri.ToString());
                Console.WriteLine(await content.ReadAsStringAsync());

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                //request.Headers.Add("Api-Key", creds.apikey);
                request.Headers.Add("Accept", "application/json");

                request.Content = content;

                Console.WriteLine(request.Headers);
                Console.WriteLine(content.Headers);


                var response = await client.SendAsync(request);
                //Console.WriteLine(response.Headers);
                var source = await response.Content.ReadAsStringAsync();
                Console.WriteLine(source);


                
            };
        }
    }

}
