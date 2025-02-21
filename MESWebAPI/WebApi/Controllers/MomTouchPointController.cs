using MESFeedClientLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Controllers
{
    public class MomTouchPointController
    {
        private HttpClient Client { get; set; }

        public MomTouchPointController(string baseaddress)
        {
            this.Client = new HttpClient();
            Client.BaseAddress = new Uri( baseaddress);
        }

        public async Task ExecuteInventoryPost(string xml)
        {
            using (Client)
            {
                var content = new StringContent(xml, Encoding.UTF8, "application/xml");

                await Client.PostAsync(ApiRoutingAdjustment.mominventory.ToString(), content); 
            }
        }
        public async Task ExecuteItemPost(string xml)
        {
            using (Client)
            {
                var content = new StringContent(xml, Encoding.UTF8, "application/xml");

                await Client.PostAsync(ApiRoutingAdjustment.momitem.ToString(), content);
            }
        }
        public async Task ExecuteWorkOrderPost(string xml)
        {
            using (Client)
            {
                var content = new StringContent(xml, Encoding.UTF8, "application/xml");

                await Client.PostAsync(ApiRoutingAdjustment.momworkorder.ToString(), content);
            }
        }
        public void LogActivity()
        {

        }
    }
}