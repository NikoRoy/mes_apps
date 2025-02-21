using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDBModelLibrary;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace CloneClientTests
{
    [TestClass]
    public class CloneManagerTests 
    {
        //dev db to mock db
        [TestMethod]
        public void ProcessHistoryCloneTest()
        {
            //Arrange
            var mset = new Mock<DbSet<tblTrainingHistory>>();
            var mcontext = new Mock<EmployeeTrainingEntities>();
            mcontext.Setup(m => m.tblTrainingHistories).Returns(mset.Object);
            var cm = new CloneManagerTest(mcontext.Object);

            //Act
            cm.TestHistoryClone();

            //Assert
            mset.Verify(s => s.Add(It.IsAny<tblTrainingHistory>()), Times.AtMostOnce());
            mcontext.Verify(s => s.SaveChanges(), Times.AtMostOnce());
        }

        [TestMethod]
        public void ProcessHistoryGetTest()
        {
            //Arrange
            var data = new List<tblTrainingHistory>()
            {
                new tblTrainingHistory(){TrainingHistoryID = 1},
                new tblTrainingHistory(){TrainingHistoryID = 2},
                new tblTrainingHistory(){TrainingHistoryID = 3}
            }.AsQueryable();

            var mset = new Mock<DbSet<tblTrainingHistory>>();

            mset.As<IQueryable<tblTrainingHistory>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mset.As<IQueryable<tblTrainingHistory>>().Setup(m => m.Provider).Returns(data.Provider);
            mset.As<IQueryable<tblTrainingHistory>>().Setup(m => m.Expression).Returns(data.Expression);
            mset.As<IQueryable<tblTrainingHistory>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mcontext = new Mock<EmployeeTrainingEntities>();

            mcontext.Setup(m => m.tblTrainingHistories).Returns(mset.Object);

            var cm = new CloneManagerTest(mcontext.Object);
            //max identity
            int i = 0;
            //Act
            var records = cm.TestHistoryGet(i).ToList();

            //Assert
            Assert.AreEqual(3, records.Count());
            Assert.AreEqual(1, records[0].TrainingHistoryID);
            Assert.AreEqual(2, records[1].TrainingHistoryID);
            Assert.AreEqual(3, records[2].TrainingHistoryID); 
        }
    }
}
