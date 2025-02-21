using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDBModelLibrary;

namespace CloneClientTests
{
    [TestClass]
    public class CurrentRequirementTests
    {
        [TestMethod]
        public void UpdateMaxIdentityTest()
        {
            //Arrange
            //make the static constructor fire
            var max = tblCurrentTrainingRequirement.MaxIdentity;
            //Act
            tblCurrentTrainingRequirement.UpdateIdentityState(max + 1);
            //Assert
            Assert.AreEqual(max + 1, tblCurrentTrainingRequirement.MaxIdentity);
        }
    }
}
