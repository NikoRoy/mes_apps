using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using MESFeedClient.Services;

namespace MESFeedClient
{   
    class Program
    {
       
        static void Main()
        {
            RunAsConsole();
            //RunAsService();
        }
        private static void RunAsService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OracleItemMESService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        private static void RunAsConsole()
        {
            OracleItemMESService i = new OracleItemMESService();
            i.StartService();

            //OracleInventoryMESService i = new OracleInventoryMESService();
            //i.StartService();

            Console.WriteLine("Running service as console. Press any key to stop.");
            //Console.ReadKey();
            i.Stop();
        }
    }
}
