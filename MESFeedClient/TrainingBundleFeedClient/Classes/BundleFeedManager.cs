using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace TrainingBundleFeedClient.Classes
{
    public class BundleFeedManager : FeedManager<TrainingBundleDownload>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<TrainingBundleDownload> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IFeedReader<TrainingBundleDownload> FeedReader;
        private readonly IUpdater<TrainingBundleDownload> xmlUpdater;
        public BundleFeedManager(IIntervalTrigger intervalTrigger,
            IXmlProcessor processor,
            IActivityLogger<TrainingBundleDownload> alogger,
            IErrorHandler ehandler,
            IFeedReader<TrainingBundleDownload> reader,
            IUpdater<TrainingBundleDownload> updater
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
        public override async Task UpdateAsync(IFeedReader<TrainingBundleDownload> reader, IUpdater<TrainingBundleDownload> updater)
        {
            DateTime runtime = DateTime.Now;
            var item = await reader.GetRecordsAsync();
            await updater.UpdateAsync(item, runtime);
        }
    }
}
