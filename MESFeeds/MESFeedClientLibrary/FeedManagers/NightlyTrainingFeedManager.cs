using System;
using System.Threading.Tasks;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;

namespace MESFeedClientLibrary.FeedManagers
{
    internal class NightlyTrainingFeedManager : IFeedManager
    {
        private IMessageReader _reader;
        private IMessageUpdater _updater;
        private IXmlProcessor _xmlProcessor;
        private IIntervalTrigger _trigger;
        private ILogger _logger;

        public NightlyTrainingFeedManager(IMessageReader reader, IMessageUpdater updater, IXmlProcessor xmlProcessor, IIntervalTrigger trigger, ILogger logger)
        {
            this._reader = reader;
            this._updater = updater;
            this._xmlProcessor = xmlProcessor;
            this._trigger = trigger;
            this._logger = logger;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            try
            {
                Task.WaitAll(UpdateAsync());
            }
            catch (Exception ex)
            {
                Task.WaitAll(_logger.LogError(ex, "Error Processing Feed"));
            }
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync()
        {
            DateTime runtime = DateTime.Now;
            var records = await _reader.GetRecordsAsync();
            await _updater.UpdateAsync(records, runtime);
        }
    }
}