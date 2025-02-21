using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;


namespace OracleWorkOrderFeedClient.Classes
{
    public class OracleWorkOrderFeedReader : IFeedReader<WorkOrder>
    {
        private readonly string Connection;
        private readonly IErrorHandler Handler;
        public OracleWorkOrderFeedReader(IErrorHandler handler, string connectiion)
        {
            this.Handler = handler;
            this.Connection = connectiion;
        }

        public async Task<IEnumerable<WorkOrder>> GetRecordsAsync()
        {
            try
            {
                var task = Task.Run(() => MESFeedClientLibrary.Model.Oracle.DataAccess.WorkOrderDownloadQuery.GetWorkOrders());
                return await task;
            }
            catch (Exception ex)
            {
                await Handler.LogError(ex, "Error in GetWorkOrderRecordsAsync()");
            }
            return null;
        }
    }
}
