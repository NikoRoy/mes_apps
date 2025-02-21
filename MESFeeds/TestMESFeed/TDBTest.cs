using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using TDBMESFeeds.Messages;
using TDBMESFeeds.Helper;
using TDBMESFeeds.DataAccess;
using MESFeedClientLibrary.Interfaces;

namespace TestMESFeed
{
    [TestClass]
    class TDBTest
    {
        [TestMethod]
        public void TDBTrainingRecordTest()
        {
            Console.WriteLine("Press 0 for p_currencyget - tdb, 1 for TV Currency - TDB, 2 for OLTP currency");
            int.TryParse(Console.ReadLine(), out int a);
            TrainingRecordType type;
            string connection;
            
            switch (a)
            {
                case 0:
                    type = TrainingRecordType.TdbCur;
                    connection = (ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
                    break;
                case 1:
                    type = TrainingRecordType.TdbAct;
                    connection = (ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
                    break;
                case 2:
                    type = TrainingRecordType.TdbExp;
                    connection = (ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
                    break;
                default:
                    type = TrainingRecordType.TdbCur;
                    connection = (ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
                    break;
            }

            

            Console.WriteLine("enter EE");
            string ee = Console.ReadLine();
            Console.WriteLine("enter document");
            string doc = Console.ReadLine();

            

            
            SqlParameter pEE = new SqlParameter("@attempts", 3);
            //SqlParameter pDoc = new SqlParameter("@DocumentNumber", doc);

            List<SqlParameter> lp = new List<SqlParameter>() { pEE};
            
            IEnumerable<IMessage> list = TrainingRecordBuilder.GetTrainingRecordDownloads(connection, type, lp);
            foreach (TrainingRecordDownload invDl in list)
            {
                Console.WriteLine(invDl.ToXml());
            }
            //Console.WriteLine("Press Enter to Exit");
            //Console.ReadLine();
        }
        [TestMethod]
        public  void TDBDocumentTest()
        {
            // This code is just for testing whether the library works
            //Console.WriteLine("Press Enter");
            //Console.ReadLine();

            IEnumerable<IMessage> result =
                new DocumentQuery().GetDownloadRecords(ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);

            foreach (DocumentDownload aDl in result)
            {
                Console.WriteLine(aDl.ToXml());
            }

            //Console.WriteLine("Press Enter to Exit");
            //Console.ReadLine();
        }
        [TestMethod]
        public void TDBBundleTest()
        {
            IEnumerable<IMessage> result = new TrainingBundleQuery().GetDownloadRecords(ConfigurationManager.ConnectionStrings["TDB"].ConnectionString);
            foreach (TrainingBundleDownload bdl  in result)
            {
                Console.WriteLine(bdl.ToXml());
            }
        }
    }
}
