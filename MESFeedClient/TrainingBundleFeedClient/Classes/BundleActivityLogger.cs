using MESFeedClientEFModel;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace TrainingBundleFeedClient.Classes
{
    public class BundleActivityLogger : ActivityLogger<TrainingBundleDownload>
    {
        private readonly string Connection;
        private readonly IErrorHandler ErrorHandler;
        public BundleActivityLogger(string connection, IErrorHandler handler) : base(connection, handler)
        {
            this.Connection = connection;
            this.ErrorHandler = handler;
        }
        public override async Task LogActivity(TrainingBundleDownload o, string action, string response)
        {
            using (var context = new MESFeedClientEntities())
            {
                try
                {
                    var i = new tblBundleFeedLog()
                    {
                        Action = action,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionID),
                        TransactionType = TrainingBundleDownload.TransactionType,
                        XmlRequest = o.ToXml(),
                        XmlResponse = response
                    };
                    context.tblBundleFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Training Bundle Activity");
                }
            }
        }
    }
}
