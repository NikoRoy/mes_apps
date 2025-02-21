using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueMountainFeedClient.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;
using System.Configuration;
using ClientUnitTests.AssetClasses;
using MESFeedClientLibrary.Model.BlueMountaion;

namespace BlueMountainFeedClient.AssetClasses.Tests
{
    [TestClass()]
    public class AssetXmlUpdaterTests
    {

        [TestMethod()]
        public async Task ThrownErrorOnUpdateAsyncTest()
        {
            //arrange
            IEnumerable<AssetDownload> assetDownloads = new List<AssetDownload>() { new AssetDownload("10000", "test desc", "FAILURE_STATUS", null) };
            var updater = new TestAssetXmlUpdater();
            
            //act
            var task = updater.UpdateAsync(assetDownloads);

            //assert
            Assert.AreEqual<bool>(await task, true);
        }

    }
}