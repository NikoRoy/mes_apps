using MESFeedClientLibrary.Factory;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.FeedManagers
{
    public class BlueMountainFeedManager : IFeedManager
    {
        private IMessageUpdater _updater;
        private IMessageReader _reader;
        //private ILogger _logger;

        public BlueMountainFeedManager(IFeedFactory feedFactory)
        {
            this._updater = feedFactory.CreateUpdater();
            this._reader = feedFactory.CreateReader();
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public void Start()
        {
            Task.Run(()=> UpdateAsync()).Wait();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync()
        {
            DateTime dt = DateTime.Now;
            var items = await this._reader.GetRecordsAsync();
            await this._updater.UpdateAsync(items, dt);
        }

        //public IFeedManager AttachUpdater(IMessageUpdater updater)
        //{
        //    this._updater = updater;
        //    return this;
        //}
        //public IFeedManager AttachReader(IMessageReader reader)
        //{
        //    this._reader = reader;
        //    return this;
        //}
        ////public void AttachLogger(ILogger logger)
        ////{
        ////    this._logger = logger;
        ////}
        //public IFeedManager Build()
        //{
        //    return this;
        //}
    }
}
