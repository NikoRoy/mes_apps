using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace OracleInventoryFeedClient.Classes
{
    public class OracleInventoryFeedReader : IFeedReader<InventoryDownload>
    {
        private readonly string Connection;
        private readonly IErrorHandler Handler;
        public OracleInventoryFeedReader(IErrorHandler handler, string connectiion)
        {
            this.Handler = handler;
            this.Connection = connectiion;
        }
        public async Task<IEnumerable<InventoryDownload>> GetRecordsAsync()
        {
            try
            {
                //var task = Task.Run(() => DataAccess.ItemDownloadQuery.GetWorkOrders(Connection));
                var task = Task.Run(() => MESFeedClientLibrary.Model.Oracle.DataAccess.InventoryDownloadQuery.GetOpenInventoryTransactions());
                return await task;
            }
            catch (Exception ex)
            {
                await Handler.LogError(ex, "Error in GetInventoryRecordsAsync()");
            }
            return null;
        }
    }
}
