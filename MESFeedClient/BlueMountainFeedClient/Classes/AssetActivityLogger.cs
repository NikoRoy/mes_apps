using BMRAMMesFeeds.Messages;
using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueMountainFeedClient.Classes
{
    public class AssetActivityLogger : MESFeedClientLibrary.Classes.ActivityLogger<AssetDownload>
    {
        private readonly string Connection;
        private readonly IErrorHandler ErrorHandler;
        public AssetActivityLogger(string connection, IErrorHandler handler): base(connection, handler)
        {
            this.Connection = connection;
            this.ErrorHandler = handler;
        }
        public override async Task LogActivity(AssetDownload o, string action, string response)
        {
            using (var context = new MESFeedClientEntities())
            {
                try
                {
                    var i = new tblBlueMountainFeedLog()
                    {
                        Action = action,
                        EquipmentDescription = o.EquipmentDescription,
                        EquipmentID = o.EquipmentId,
                        EquipmentStatus = o.EquipmentStatus,
                        NextCalibrationDueDate = o.NextCalibrationDueDate,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionId),
                        TransactionType = AssetDownload.TransactionType,
                        XmlRequest = o.ToXml(),
                        XmlResponse = response
                    };
                    context.tblBlueMountainFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Blue Mountain Activity");
                }
            }
        }
    }
}
