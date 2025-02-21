using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDBModelLibrary;

namespace CloneClientTests
{
    [TestClass]
    public class TrainingHistoryTests
    {
        [TestMethod]
        public void UpdateMaxIdentityTest()
        {
            //Arrange
            //make the static constructor fire
            var max = tblTrainingHistory.MaxIdentity;
            //Act
            tblTrainingHistory.UpdateIdentityState(max + 1);
            //Assert
            Assert.AreEqual(max + 1, tblTrainingHistory.MaxIdentity);
        }
    }
}
