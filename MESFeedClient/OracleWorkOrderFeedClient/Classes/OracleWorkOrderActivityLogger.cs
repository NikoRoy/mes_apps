
using MESFeedClientEFModel;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleWorkOrderFeedClient.Classes
{
    public class OracleWorkOrderActivityLogger : ActivityLogger<WorkOrder>
    {
        private readonly string ConnectionString;
        private readonly IErrorHandler ErrorHandler;
        public OracleWorkOrderActivityLogger(string connectionString, IErrorHandler errorHandler) : base(connectionString, errorHandler)
        {
            this.ConnectionString = connectionString;
            this.ErrorHandler = errorHandler;
        }

        public override async Task LogActivity(WorkOrder o, string action, string response)
        {
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                try
                {
                    var i = new tblDownloadFeedLog()
                    {
                        Action = action,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionId),
                        TransactionType = WorkOrder.TransactionType,
                        RequestXml = o.ToXml(),
                        Response = response
                    };
                    context.tblDownloadFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Work Order Activity");
                }
            }
        }
    }
}
