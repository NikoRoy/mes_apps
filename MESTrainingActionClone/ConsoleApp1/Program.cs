
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TrainingActionCloneClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceStart();

            //ConsoleStart();
            //Console.WriteLine("Enter any key to exit");
            //Console.ReadKey();

        }
        private static void ServiceStart()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TrainingActionCloneService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        private static void ConsoleStart()
        {
            var client = new TrainingActionCloneService();
            client.StartService();
        }
    }
}
