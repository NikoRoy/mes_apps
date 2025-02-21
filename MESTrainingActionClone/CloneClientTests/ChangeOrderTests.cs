using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDBModelLibrary;

namespace CloneClientTests
{
    [TestClass]
    public class ChangeOrderTests
    {
        [TestMethod]
        public void UpdateMaxIdentityTest()
        {
            //Arrange
                //make the static constructor fire
            var max = ChangeOrderRequiredTraining.MaxIdentity;
            //Act
            ChangeOrderRequiredTraining.UpdateIdentityState(max + 1);
            //Assert
            Assert.AreEqual(max + 1, ChangeOrderRequiredTraining.MaxIdentity);
        }
    }
}
