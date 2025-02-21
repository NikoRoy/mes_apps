using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;

using MESFeedClientEFModel;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace MESFeedClient.Classes
{
    public class OracleItemActivityLogger : ActivityLogger<Item>  
    {
        private readonly string ConnectionString;
        private readonly IErrorHandler ErrorHandler;
        public OracleItemActivityLogger(string connectionString, IErrorHandler errorHandler) : base(connectionString, errorHandler)
        {
            this.ConnectionString = connectionString;
            this.ErrorHandler = errorHandler;
        }

        public override async Task LogActivity(Item t, string action, string response)
        {
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                try
                {
                    var i = new tblDownloadFeedLog()
                    {
                        Action = action,
                        TransactionDate = t.TransactionDateTime,
                        TransactionID = Convert.ToString(t.TransactionId),
                        TransactionType = Item.TransactionType,
                        RequestXml = t.ToXml(),
                        Response = response
                    };
                    context.tblDownloadFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Item Activity");
                }
            }
        }
        #region //old interface
            //public async Task LogInventory(InventoryDownload inventoryDownload, string action, string response)
            //{
            //    try
            //    {
            //        //log acitivty to database
            //    }
            //    catch (Exception ex)
            //    {
            //        await this.ErrorHandler.LogError(ex, "Error adding Activity");
            //    }
            //}

            //public async Task LogItem(Item item, string action, string response)
            //{
            //    try
            //    {
            //        //log acitivty to database
            //    }
            //    catch (Exception ex)
            //    {
            //        await this.ErrorHandler.LogError(ex, "Error adding Activity");
            //    }
            //}

            //public async Task LogWorkOrder(WorkOrder workOrder, string action, string response)
            //{
            //    try
            //    {
            //        //log acitivty to database
            //    }
            //    catch (Exception ex)
            //    {
            //        await this.ErrorHandler.LogError(ex, "Error adding Activity");
            //    }
            //}
            #endregion
        }
}
