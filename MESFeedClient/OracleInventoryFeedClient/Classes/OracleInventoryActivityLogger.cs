using System;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;

using MESFeedClientEFModel;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace OracleInventoryFeedClient.Classes
{
    public class OracleInventoryActivityLogger : ActivityLogger<InventoryDownload>
    {
        private readonly string ConnectionString;
        private readonly IErrorHandler ErrorHandler;
        public OracleInventoryActivityLogger(string connectionString, IErrorHandler errorHandler) : base(connectionString, errorHandler)
        {
            this.ConnectionString = connectionString;
            this.ErrorHandler = errorHandler;
        }
        public override async Task LogActivity(InventoryDownload o, string action, string response)
        {

            #region log to sql server
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                try
                {
                    var i = new tblDownloadFeedLog()
                    {
                        Action = action,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionId),
                        TransactionType = InventoryDownload.TransactionType,
                        RequestXml = o.ToXml(),
                        Response = response
                    };
                    context.tblDownloadFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Inventory Activity");
                }
            }
            #endregion
        }
    }
}
