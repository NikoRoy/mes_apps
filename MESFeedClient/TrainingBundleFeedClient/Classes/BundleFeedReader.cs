using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.DataAccess;
using TDBMESFeeds.Messages;

namespace TrainingBundleFeedClient.Classes
{
    public class BundleFeedReader : IFeedReader<TrainingBundleDownload>
    {
        private readonly IErrorHandler ErrorHandler;
        private readonly string Connection;

        public BundleFeedReader(IErrorHandler handler, string cn)
        {
            this.ErrorHandler = handler;
            this.Connection = cn;
        }
        public async Task<IEnumerable<TrainingBundleDownload>> GetRecordsAsync()
        {
            try
            {
                //var task = Task.Run(() => DataAccess.ItemDownloadQuery.GetWorkOrders(Connection));
                var task = await Task.Run(() => TrainingBundleQuery.GetBundleDownloads(Connection));
                return task;
            }
            catch (Exception ex)
            {
                await this.ErrorHandler.LogError(ex, "Error in GetBundleRecordsAsync()");
            }
            return null;
        }
    }
}
