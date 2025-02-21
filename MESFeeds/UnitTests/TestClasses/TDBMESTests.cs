using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using TDBMESFeeds.DataAccess;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using TDBMESFeeds.Messages;
using System.Configuration;
using MESFeedClientLibrary.FeedManagers;
using System.Data.Entity;
using Moq;
using MESFeedClientEFModel;
using System.Data.Entity.Core.Objects;
using System.Data;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Error;
using System.IO;
using MESFeedClientLibrary.Alert;
using MESFeedClientLibrary.Factory;
using UnitTests.Mock;
using MESFeedClientLibrary;

namespace UnitTests
{
    [TestClass]
    public class TDBMESTests
    {
        [TestMethod]
        public async Task TestTrainingRecordActionAsync()
        {
            //arrange
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@attempts", 3));

            // act
            var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (60000 * 5));
            var t = Task.Run(() => new TrainingRecordAction().GetRecords(parameters));
            await t.TimeoutAfter(timeout);
            //assert
            Assert.IsInstanceOfType(t.Result, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public async Task TestTrainingRecordCurrency()
        {
            //arrange
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //parameters.Add(new SqlParameter("@attempts", 3));

            // act
            var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (60000 * 5));
            var t = Task.Run(() => new TrainingRecordCurrency().GetRecords());
            await t.TimeoutAfter(timeout);
            //assert
            Assert.IsInstanceOfType(t.Result, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public async Task TestTrainingRecordExpiration()
        {
            //arrange
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //parameters.Add(new SqlParameter("@attempts", 3));

            // act
            var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (60000 * 5));
            var t = Task.Run(() => new TrainingRecordExpiration().GetRecords());
            await t.TimeoutAfter(timeout);
            //assert
            Assert.IsInstanceOfType(t.Result, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public void TestEntities()
        {
            using (var entity = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                entity.Database.CommandTimeout = 120;
                var o = entity.spCheckLastActions(3);
            }
        }
        [TestMethod]
        public void TestFeedManager()
        {
            var a = new MESFeedClientLibrary.Factory.TrainingFeedManagerFactory().Create();
            Assert.IsInstanceOfType(a, typeof(IFeedManager));
        }
        [TestMethod]
        public async Task TestBundles()
        {
            var t = await Task.Run(() => new TrainingBundleQuery().GetRecords());
            Assert.IsInstanceOfType(t, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public async Task TestDocuments()
        {
            var t = await Task.Run(() => new DocumentQuery().GetRecords());
            Assert.IsInstanceOfType(t, typeof(IEnumerable<IMessage>));
        }
        [TestMethod]
        public async Task TestBundleXML()
        {

            //var moqDB = new Mock<MESFeedClientEntities>();
            //var moqOr = new Mock<ObjectResult<spBundleDailyChangesGet_Result>>();
            //var data = new List<spBundleDailyChangesGet_Result>() {
            //    new spBundleDailyChangesGet_Result() { TrainingRequirementGroupName = "T1 - Oasis QC Final", Description = "T1 - Oasis QC Final", Document= "MP000005", DocCurrentRev= "AB"}
            //    ,new spBundleDailyChangesGet_Result() { TrainingRequirementGroupName = "T1 - Oasis QC Final", Description = "T1 - Oasis QC Final", Document= "MP000006", DocCurrentRev= "AC"}

            //};
            //moqOr.Setup(i => i.GetEnumerator()).Returns(data.GetEnumerator());
            //moqDB.Setup(b => b.spBundleDailyChangesGet()).Returns( moqOr.Object);


            //var readerMock = new Mock<IDataReader>();

            //readerMock.SetupSequence(_ => _.Read())
            //    .Returns(true)
            //    .Returns(false);

            //readerMock.Setup(reader => reader.GetOrdinal("Id")).Returns(0);
            //readerMock.Setup(reader => reader.GetOrdinal("Name")).Returns(1);

            //readerMock.Setup(reader => reader.GetInt32(It.IsAny<int>())).Returns(1);
            //readerMock.Setup(reader => reader.GetString(It.IsAny<int>())).Returns("Hello World");

            //var commandMock = new Mock<IDbCommand>();
            //commandMock.Setup(m => m.ExecuteReader()).Returns(readerMock.Object).Verifiable();

            //var connectionMock = new Mock<IDbConnection>();
            //connectionMock.Setup(m => m.CreateCommand()).Returns(commandMock.Object);

            ////var data = new Data(() => connectionMock.Object);

            ////Act
            //var result = data.FindAll();

            ////Assert - FluentAssertions
            //result.Should().HaveCount(1);
            //commandMock.Verify(); //since it was marked verifiable.

            //string cn = ConfigurationManager.ConnectionStrings["bundles"].ToString();
            //var t = await Task.Run(() => new UnitTests.Mock.TestTrainingBundleQuery().GetDownloadRecords(moqDB));

            //foreach (var i in t)
            //{
            //    if (i.ToXml().Contains("T1 - Oasis QC Final"))
            //    {
            //        Console.WriteLine(i.ToXml());
            //    }
            //}
           // Assert.IsInstanceOfType(t, typeof(IEnumerable<IMessage>));
            
        }

        [TestMethod]
        public void TestInterfaceTypeErrorHandler()
        {
            //arrange
            string res = Path.Combine("C:\\Workspace\\Logs", string.Format("MESFeedLog-{0}-{1}", MESFeedClientLibrary.BusinessLayer.InterfaceTypes.TrainingGroup.ToString(), DateTime.Today.ToString("yyyy-MM-dd")));
            IErrorHandler itf = new InterfaceTypeErrorHandler("C:\\Workspace\\Logs", MESFeedClientLibrary.BusinessLayer.InterfaceTypes.TrainingGroup);
            //act
            itf.LogError(new Exception("Test Exception"), "Unit Test");
            //assert file
            Assert.IsTrue(File.Exists(res));
            //assert content
            Assert.AreEqual("Unit Test", File.ReadAllText(res).Substring(0,9));
        }
        [TestMethod]
        public async Task TestInterfaceTypeAlertHandler()
        {
            IAlertHandler ah = 
                new InterfaceTypeAlertHandler("bi@getinge.com", "nicholas.roy@getinge.com", "mailrelay.getingegroup.local", MESFeedClientLibrary.BusinessLayer.InterfaceTypes.TrainingGroup);
            await ah.SendAlert("Test Message", "Test Subject");

        }

        [TestMethod]
        public void TestProcessFeedFailure()
        {
            int ct = 0;
            IFeedManager fm = new TestBFMBLFactory().Create();
            try
            {
                fm.Start();
            }
            catch(StoredProcedureException ex)
            {
                ct++;
            }
            finally
            {
                Assert.AreEqual(1, ct);
            }
        }
        [TestMethod]
        public void TestUpdateAsyncFailure()
        {
            int ct = 0;
            IFeedManager fm = new TestBFMUpdateFactory().Create();
            try
            {
                fm.Start();
            }
            catch (AggregateException ex)
            {
                foreach (var item in ex.Flatten().InnerExceptions)
                {
                    ct++;
                }
            }
            finally
            {
                Assert.AreEqual(1, ct);
            }
        }
    }
}
