using MESFeedClientEFModel;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace TrainingRecordFeedClient.Classes
{
    public class RecordActivityLogger : ActivityLogger<TrainingRecordDownload>
    {
        private readonly string Connection;
        private readonly IErrorHandler ErrorHandler;
        public RecordActivityLogger(string connection, IErrorHandler handler) : base(connection, handler)
        {
            this.Connection = connection;
            this.ErrorHandler = handler;
        }
        public override async Task LogActivity(TrainingRecordDownload o, string action, string response)
        {
            using (var context = new MESFeedClientEntities())
            {
                try
                {
                    
                    var j = new tblRecordFeedLog()
                    {
                        Action = action,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionID),
                        TransactionType = TrainingRecordDownload.TransactionType,
                        XmlRequest = o.ToXml(),
                        XmlResponse = response
                    };

                    context.tblRecordFeedLogs.Add(j);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await ErrorHandler.LogError(ex, "Error adding Training Record Activity");
                }
            }
        }
    }
}
