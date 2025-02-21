using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDBModelLibrary;

namespace CloneClientTests
{
    [TestClass]
    public class JPTTests
    {
        [TestMethod]
        public void UpdateMaxIdentityTest()
        {
            //Arrange
            //make the static constructor fire
            var max = xxatr_jobprocedure_tracking.MaxIdentity;
            //Act
            xxatr_jobprocedure_tracking.UpdateIdentityState(max + 1);
            //Assert
            Assert.AreEqual(max + 1, xxatr_jobprocedure_tracking.MaxIdentity);
        }
    }
}
