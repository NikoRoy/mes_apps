
using OracleInventoryFeedClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OracleInventoryFeedClient
{
    class Program
    {

        static void Main(string[] args)
        {
            RunAsConsole();
            //RunAsService();
        }
        private static void RunAsService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OracleInventoryMESService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        private static void RunAsConsole()
        {
            OracleInventoryMESService i = new OracleInventoryMESService();
            i.StartService();

            Console.WriteLine("Running service as console. Press any key to stop.");
            //Console.ReadKey();
            i.Stop();
        }
    }
}
