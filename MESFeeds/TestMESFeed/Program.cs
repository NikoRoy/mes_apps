using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MESFeedClientLibrary.Model.Oracle.DataAccess;
using MESFeedClientLibrary.Model.Oracle.Messages;
using OracleMESFeeds;

namespace TestMESFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            //testing tdb
            Console.WriteLine("Press Enter for tdb");
            Console.ReadLine();



            var dt = DateTime.Parse("8/27/2021 1:52:24 PM");

            var d = dt.ToString("dd-MM-yyyy hh:mm:ss");

            var t = new TDBTest();
            //t.TDBBundleTest();
            //t.TDBDocumentTest();
            t.TDBTrainingRecordTest();

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();


            //testing blue mountain
            Console.WriteLine("Press Enter for BM");
            Console.ReadLine();

            var b = new BMTest();
            b.BMRAMTestMethod();

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();

            

            // This code is just for testing whether the library works
            Console.WriteLine("Press Enter for workorders");
            Console.ReadLine();

            IList<OracleMESFeeds.Messages.WorkOrder> result3 =
                OracleMESFeeds.DataAccess.WorkOrderDownloadQuery.GetWorkOrders();

            using (StreamWriter sw = new StreamWriter(@"C:\Work\WorkOrderXML.xml"))
            {

                foreach (OracleMESFeeds.Messages.WorkOrder invDl in result3)
                {
                    string xml = invDl.ToXml();
                    Console.WriteLine(xml);
                    sw.WriteLine(xml);
                }
            }


            // This code is just for testing whether the library works
            Console.WriteLine("Press Enter For Inventory");
            Console.ReadLine();
            IList<OracleMESFeeds.Messages.InventoryDownload> result =
                OracleMESFeeds.DataAccess.InventoryDownloadQuery.GetOpenInventoryTransactions();

            using (StreamWriter sw2 = new StreamWriter(@"C:\Work\InventoryXML.xml"))
            {
                foreach (OracleMESFeeds.Messages.InventoryDownload invDl in result)
                {
                    string xml = invDl.ToXml();
                    Console.WriteLine(xml);
                    sw2.WriteLine(xml);
                }
            }

            // This code is just for testing whether the library works
            Console.WriteLine("Press Enter for items");
            Console.ReadLine();

            IList<Item > result2 =
                ItemDownloadQuery.GetItemTransactions();

            using (StreamWriter sw3 = new StreamWriter(@"C:\Work\ItemsXML.xml"))
            {
                foreach (Item invDl in result2)
                {
                    string xml = invDl.ToXml();
                    Console.WriteLine(xml);
                    sw3.WriteLine(xml);
                }
            }

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();




        }
    }
}
