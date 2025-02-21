using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.FeedManagers
{
    public class FeedManager : MESFeedClientLibrary.FeedManagers.IFeedManager
    {
        private readonly IQuery _query;
        private readonly IMessageReader _reader;
        private readonly IMessageUpdater _updater;
        private readonly IIntervalTrigger _intervalTrigger;
        private readonly IXmlProcessor _xmlProcessor;
        private readonly ILogger _logger;

        public FeedManager(IQuery query, IMessageReader reader, IMessageUpdater updater, IXmlProcessor processor, IIntervalTrigger interval, Logger.ILogger logger)
        {
            this._query = query;
            this._reader = reader;
            this._updater = updater;
            this._intervalTrigger = interval;
            this._xmlProcessor = processor;
            this._logger = logger;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public async Task UpdateAsync()
        {
            DateTime runtime = DateTime.Now;
            var records = await _reader.GetRecordsAsync();
            await _updater.UpdateAsync(records, runtime);
        }

    }
}
