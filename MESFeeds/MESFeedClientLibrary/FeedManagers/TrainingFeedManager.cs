using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.FeedManagers
{
    public class TrainingFeedManager : MESFeedClientLibrary.FeedManagers.IFeedManager
    {
        private readonly IMessageReader _reader;
        private readonly IMessageUpdater _updater;
        private readonly IIntervalTrigger _intervalTrigger;
        private readonly IXmlProcessor _xmlProcessor;
        private readonly ILogger _logger;

        public TrainingFeedManager( IMessageReader reader, IMessageUpdater updater, IXmlProcessor processor, IIntervalTrigger interval, Logger.ILogger logger)
        {
            this._reader = reader;
            this._updater = updater;
            this._intervalTrigger = interval;
            this._xmlProcessor = processor;
            this._logger = logger;
        }


        public void Dispose() {}

        public void Start()
        {
            ProcessFeed();
        }

        public void Stop() { }

        public async Task UpdateAsync()
        {
            DateTime runtime = DateTime.Now;
            var records = await _reader.GetRecordsAsync();
            await _updater.UpdateAsync(records, runtime);
        }
        public void UpdateQueue()
        {
            //queue table updates
            BusinessExtensions.UpdateQueueTable();
            BusinessExtensions.UpdateQueueTableNewUser();
        }
        private void ProcessFeed()
        {
            try
            {
                UpdateQueue();
                Task.WaitAll(UpdateAsync());
            }
            catch (Exception ex)
            {
                Task.WaitAll(_logger.LogError(ex, "Error Processing Feed"));
            }
        }
    }

}
