using MESFeedClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OracleWorkOrderFeedClient
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
                new OracleWorkOrderMESService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        private static void RunAsConsole()
        {
            OracleWorkOrderMESService i = new OracleWorkOrderMESService();
            i.StartService();

            Console.WriteLine("Running service as console. Press any key to stop.");
            //Console.ReadKey();
            i.Stop();
        }
    }
}
