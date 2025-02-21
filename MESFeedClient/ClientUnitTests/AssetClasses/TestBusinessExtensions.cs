using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MESFeedClientLibrary.BusinessLayer;
using Moq;
using System.Threading.Tasks;
using System.Data.Entity;
using MESFeedClientEFModel.Extensions;

namespace ClientUnitTests.AssetClasses
{
    public static class TestBusinessExtensions
    {
        public static async Task UpdateBlueMountainControlTime(DateTime date)
        {
            //var mockSet = new Mock<DbSet<tblMESControl>>();
            //var mockContext = new Mock<MESFeedClientEntities>();
            //mockContext.Setup(m => m.tblMESControls).Returns(mockSet.Object);

            await tblMESControl.UpdateLastRunDate(EntityFactory.GenerateContext(), InterfaceTypes.BlueMountain.ToString(), date);
        }
    }
}
