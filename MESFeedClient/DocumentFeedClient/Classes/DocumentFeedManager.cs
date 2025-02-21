using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.Messages;

namespace DocumentFeedClient.Classes
{
    class DocumentFeedManager : FeedManager<DocumentDownload>
    {
        

        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<DocumentDownload> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IFeedReader<DocumentDownload> FeedReader;
        private readonly IUpdater<DocumentDownload> xmlUpdater;

        public DocumentFeedManager
        (
            IIntervalTrigger intervalTrigger,
            IXmlProcessor processor,
            IActivityLogger<DocumentDownload> alogger,
            IErrorHandler ehandler,
            IFeedReader<DocumentDownload> reader,
            IUpdater<DocumentDownload> updater
        )
            :base(intervalTrigger, processor, alogger, ehandler, reader, updater)
        {
            this.intervalTrigger = intervalTrigger; 

            activityLogger = alogger;
            errorHandler = ehandler;

            xmlProcessor = processor;

            FeedReader = reader;
            xmlUpdater = updater;
        }
        public override void Start()
        {
            ProcessFeed();
        }
        private void ProcessFeed()
        {
            try
            {
                //perform http requests and process response
                Task.WaitAll(
                        UpdateAsync(FeedReader, xmlUpdater)
                    );
            }
            catch (Exception ex)
            {
                this.errorHandler.LogError(ex, "Error Processing Feed");
            }            
        }
        public override  async Task UpdateAsync(IFeedReader<DocumentDownload> reader, IUpdater<DocumentDownload> updater)
        {
            DateTime runtime = DateTime.Now;
            var item = await reader.GetRecordsAsync();
            await updater.UpdateAsync(item, runtime);
        }
    }
}
