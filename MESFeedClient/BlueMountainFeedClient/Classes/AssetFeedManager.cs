using BMRAMMesFeeds.Messages;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueMountainFeedClient.Classes
{
    public class AssetFeedManager : FeedManager<AssetDownload>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<AssetDownload> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IFeedReader<AssetDownload> FeedReader;
        private readonly IUpdater<AssetDownload> xmlUpdater;
        public AssetFeedManager(IIntervalTrigger intervalTrigger,
            IXmlProcessor processor,
            IActivityLogger<AssetDownload> alogger,
            IErrorHandler ehandler,
            IFeedReader<AssetDownload> reader,
            IUpdater<AssetDownload> updater
        )
            : base(intervalTrigger, processor, alogger, ehandler, reader, updater)
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
                Task.WaitAll(
                        UpdateAsync(FeedReader, xmlUpdater)
                    );
            }
            catch (Exception ex)
            {
                this.errorHandler.LogError(ex, "Error Processing Feed");
            }
        }
        public override async Task UpdateAsync(IFeedReader<AssetDownload> reader, IUpdater<AssetDownload> updater)
        {
            DateTime runtime = DateTime.Now;
            var items = await reader.GetRecordsAsync();
            await updater.UpdateAsync(items, runtime);
        }
    }
}
