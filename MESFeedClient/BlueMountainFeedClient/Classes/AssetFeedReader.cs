using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRAMMesFeeds.Messages;
using MESFeedClientLibrary.Interfaces;

namespace BlueMountainFeedClient.Classes
{
    public class AssetFeedReader : IFeedReader<AssetDownload>
    {
        private readonly IErrorHandler ErrorHandler;
        private readonly string Connection;

        public AssetFeedReader(IErrorHandler handler, string cn)
        {
            this.ErrorHandler = handler;
            this.Connection = cn;
        }
        public async Task<IEnumerable<AssetDownload>> GetRecordsAsync()
        {
            try
            {
                var task = Task.Run(() => BMRAMMesFeeds.DataAccess.AssetQuery.GetTrainingRecords());
                return await task;
            }
            catch (Exception ex)
            {
                await this.ErrorHandler.LogError(ex, "Error in GetAssetRecordsAsync()");
            }
            return null;
        }
    }
}
