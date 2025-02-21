using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using MESFeedClientLibrary.BusinessLayer;
using System;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.FeedManagers
{
    public class BundleFeedManager : MESFeedClientLibrary.FeedManagers.IFeedManager
    {
        private IQuery _query;
        private IMessageReader _reader;
        private IMessageUpdater _updater;
        private IIntervalTrigger _intervalTrigger;
        private IXmlProcessor _xmlProcessor;
        private ILogger _logger;

        public BundleFeedManager(IQuery query, IMessageReader reader, IMessageUpdater updater, IXmlProcessor processor, IIntervalTrigger interval, Logger.ILogger logger)
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
            this._xmlProcessor.Dispose();
            this._intervalTrigger.Dispose();
        }

        public void Start()
        {
            ProcessFeed();
        }

        public void Stop()
        {
            this._intervalTrigger.Stop();
        }

        public async Task UpdateAsync()
        {
            var dt = DateTime.Now;
            var res = await _reader.GetRecordsAsync();
            await _updater.UpdateAsync(res);
        }
        private void ProcessFeed()
        {            
            try
            {
                BusinessExtensions.BackFeedTrainingActivity();
                Task.WaitAll(UpdateAsync());
            }
            catch(AggregateException ex)
            {
                foreach (var iEx in ex.Flatten().InnerExceptions)
                {
                    Task.Run(() => this._logger.LogError(iEx, "Aggregate Error Processing Feed")).Wait();
                }
            }
            catch(StoredProcedureException ex)
            {
                Task.Run(() => this._logger.LogError(ex, "BusinessExtension Error Processing Feed")).Wait();
            }
            catch (Exception ex)
            {
                Task.WaitAll(_logger.LogError(ex, "Error Processing Feed"));
            }
        }
    }
}
