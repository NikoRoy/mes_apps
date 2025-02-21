using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using BMRAMMesFeeds.DataAccess;
using BMRAMMesFeeds.Messages;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Factory;

namespace UnitTests
{
    [TestClass]
    public class BlueMountainTest
    {
        [TestMethod]
        public async Task TestBlueMountain()
        {
            // act
            //var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (60000 * 5));
            var t = Task.Run(() => AssetQuery.GetTrainingRecords());
            await t;
            //assert
            Assert.IsInstanceOfType(t.Result, typeof(List<AssetDownload>));
        }
        [TestMethod]
        public void MyTestMethod()
        {
            IFeedManager bm = new BlueMountainFeedManagerFactory().MakeManager(); 
        }
    }
}
