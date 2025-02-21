using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MESFeedClientLibrary.Factory;
using MESFeedClientLibrary.FeedManagers;

using MESFeedClientLibrary.Updater;
using MESFeedClientLibrary.Reader;
using System.Threading.Tasks;
using System.Collections.Generic;
using MESFeedClientLibrary.Interfaces;

namespace ClientUnitTests.FactoryTests
{
    [TestClass]
    public class GenericInterfaceTests
    {
        [TestMethod]
        public async Task TestUpdaterUpdate()
        {
            //arrange
            TestUpdater updater = new TestUpdater();
            IEnumerable<IMessage> set = new List<IMessage>();
            //act 
            await updater.UpdateAsync(set, DateTime.Now);
            //assert
            Assert.AreEqual(updater.testingstring, "Updated");
        }
        [TestMethod]
        public async Task TestManagerUpdate()
        {
            //arrange
            IMessageUpdater updater = new TestUpdater();
            IMessageReader reader = new TestReader("not an empty or whitespace string", 0, 0);
            TestFeedManager fm = new TestFeedManager(reader, updater);
            //act
            await fm.UpdateAsync();
            //assert
            Assert.AreEqual(fm.testupdatestring, "UpdatedAsync");
        }
        [TestMethod]
        public async Task TestReaderGet()
        {
            //arrange
            TestReader reader = new TestReader("not an empty or whitespace string", 0, 0);
            //act
            var a = await reader.GetRecordsAsync();
            //assert
            Assert.IsInstanceOfType(a, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public void TestFactoryCreate()
        {
            //Arrange
            IFeedManagerFactory factory = new TestFeedManagerFactory();
            //act
            var fm = factory.Create();
            //assert
            Assert.IsInstanceOfType(fm, typeof(IFeedManager));
        }
    }
}
