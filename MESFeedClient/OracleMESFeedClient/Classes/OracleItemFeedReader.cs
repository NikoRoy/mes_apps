using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace MESFeedClient.Classes
{
    /*NOTES ON THIS CLASS:
     * if connections strings are being passed from services configuration, maybe the xml library getters should be overloaded so we dont need to maintain config in both places.
     * possible change commented out below
    */
    public class OracleItemFeedReader : IFeedReader<Item>
    {
        private readonly string Connection;
        private readonly IErrorHandler Handler;
        public OracleItemFeedReader(IErrorHandler handler, string connectiion)
        {
            this.Handler = handler;
            this.Connection = connectiion;
        }

        public async Task<IEnumerable<Item>> GetRecordsAsync()
        {
            try
            {
                //var task = Task.Run(() => DataAccess.ItemDownloadQuery.GetWorkOrders(Connection));
                var task = Task.Run(() => MESFeedClientLibrary.Model.Oracle.DataAccess.ItemDownloadQuery.GetItemTransactions());
                return await task;
            }
            catch (Exception ex)
            {
                await Handler.LogError(ex, "Error in GetItemRecordsAsync()");
            }
            return null;
        }

        //Task<IEnumerable<Item>> IFeedReader<Item>.GetRecordsAsync()
        //{
        //    throw new NotImplementedException();
        //}
        #region old logic
        //public async Task<IEnumerable<InventoryDownload>> GetInventoryRecordsAsync()
        //{
        //    try
        //    {
        //        //var task = Task.Run(() => DataAccess.InventoryDownloadQuery.GetWorkOrders(Connection));
        //        var task = Task.Run(() => DataAccess.InventoryDownloadQuery.GetOpenInventoryTransactions());
        //        return await task;
        //    }
        //    catch (Exception ex)
        //    {
        //        await Handler.LogError(ex, "Error in GetInventoryRecordsAsync()");
        //    }
        //    return null;
        //}

        //public async Task<IEnumerable<Item>> GetItemRecordsAsync()
        //{
        //    try
        //    {
        //        //var task = Task.Run(() => DataAccess.ItemDownloadQuery.GetWorkOrders(Connection));
        //        var task = Task.Run(() => DataAccess.ItemDownloadQuery.GetItemTransactions());
        //        return await task;
        //    }
        //    catch (Exception ex)
        //    {
        //        await Handler.LogError(ex, "Error in GetItemRecordsAsync()");
        //    }
        //    return null;
        //}

        //public async Task<IEnumerable<WorkOrder>> GetWorkOrderRecordsAsync()
        //{
        //    try
        //    {
        //        //var task = Task.Run(() => DataAccess.WorkOrderDownloadQuery.GetWorkOrders(Connection));
        //        var task = Task.Run(() => DataAccess.WorkOrderDownloadQuery.GetWorkOrders());
        //        return await task;
        //    }
        //    catch (Exception ex)
        //    {
        //        await Handler.LogError(ex, "Error in GetWorkOrderRecordsAsync()");
        //    }
        //    return null;
        //}
        #endregion
    }
}
