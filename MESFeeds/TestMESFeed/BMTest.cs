using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRAMMesFeeds.DataAccess;
using BMRAMMesFeeds.Messages;
using System.Configuration;

namespace TestMESFeed
{
    [TestClass]
    class BMTest
    {
        [TestMethod]
        public void BMRAMTestMethod()
        {
            // This code is just for testing whether the library works

            IList<AssetDownload> result =
                AssetQuery.GetAssetDownloads(ConfigurationManager.ConnectionStrings["BMRAM"].ConnectionString);

            foreach (AssetDownload aDl in result)
            {
                Console.WriteLine(aDl.ToXml());
            }

        }
    }
}
