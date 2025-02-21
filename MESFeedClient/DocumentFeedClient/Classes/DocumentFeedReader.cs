using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.DataAccess;
using TDBMESFeeds.Messages;

namespace DocumentFeedClient.Classes
{
    public class DocumentFeedReader : IFeedReader<DocumentDownload>
    {
        private readonly string Connection;
        private readonly IErrorHandler Handler;
        public DocumentFeedReader(IErrorHandler handler, string connection)
        {
            this.Handler = handler;
            this.Connection = connection;
        }
        public async Task<IEnumerable<DocumentDownload>> GetRecordsAsync()
        {
            try
            {
                
                var task = Task.Run(() => DocumentQuery.GetDocumentDownloads(Connection));
                return await task;
            }
            catch (Exception ex)
            {
                await Handler.LogError(ex, "Error in GetDocumentRecordsAsync()");
            }
            return null;
        }
    }
}
