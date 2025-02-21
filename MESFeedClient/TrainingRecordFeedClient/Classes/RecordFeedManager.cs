using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace TrainingRecordFeedClient.Classes
{
    public class RecordFeedManager : IFeedManager<TrainingRecordDownload>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IActivityLogger<TrainingRecordDownload> alogger;
        private readonly IErrorHandler ehandler;
        private readonly IFeedReader<TrainingRecordDownload> FeedReader;
        private readonly IUpdater<TrainingRecordDownload> xmlUpdater;
        public RecordFeedManager(IIntervalTrigger intervalTrigger,
            IXmlProcessor processor,
            IActivityLogger<TrainingRecordDownload> alogger,
            IErrorHandler ehandler,
            IFeedReader<TrainingRecordDownload> reader,
            IUpdater<TrainingRecordDownload> updater
        )
        {
            this.intervalTrigger = intervalTrigger;

            xmlProcessor = processor;
            this.alogger = alogger;
            this.ehandler = ehandler;
            FeedReader = reader;
            xmlUpdater = updater;
        }
        public void Start()
        {
            //base.Start();
            //override to skip timer as a scheduled task
            ProcessFeed();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
        private void ProcessFeed()
        {
            try
            {
                UpdateQueue();
                Task.WaitAll( UpdateAsync(FeedReader, xmlUpdater));
            }
            catch (Exception ex)
            {
                Task.WaitAll(ehandler.LogError(ex, "Error Processing Feed"));
            }
        }
        public async Task UpdateAsync(IFeedReader<TrainingRecordDownload> reader, IUpdater<TrainingRecordDownload> updater)
        {
            var item = await reader.GetRecordsAsync();
            await updater.UpdateAsync(item);
        }
        public void UpdateQueue()
        {
            try
            {
                BusinessExtensions.UpdateQueueTable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
