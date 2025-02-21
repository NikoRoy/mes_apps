using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Classes
{
    public abstract class FeedManager<T> : Interfaces.IFeedManager<T>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<T> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IFeedReader<T> FeedReader;
        private readonly IUpdater<T> xmlUpdater;
        public FeedManager
        (
            IIntervalTrigger it,
            IXmlProcessor processor,
            IActivityLogger<T> alogger,
            IErrorHandler ehandler,
            IFeedReader<T> reader,
            IUpdater<T> updater
        )
        {
            intervalTrigger = it;
            intervalTrigger.IntervalReached += this.ProcessFeed;

            activityLogger = alogger;
            errorHandler = ehandler;

            xmlProcessor = processor;

            FeedReader = reader;
            xmlUpdater = updater;

        }

        private void ProcessFeed(object sender, EventArgs e)
        {
            this.intervalTrigger.Stop();
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
            finally
            {
                this.intervalTrigger.Start();
            }
        }

        public virtual void Dispose()
        {
            this.intervalTrigger.IntervalReached -= this.ProcessFeed;
            this.intervalTrigger.Stop();
            this.intervalTrigger.Dispose();
        }

        public virtual void Start()
        {
            this.intervalTrigger.Start();

            this.ProcessFeed(this, new EventArgs());
        }

        public virtual void Stop()
        {
            this.intervalTrigger.Stop();
        }

        public abstract Task UpdateAsync(IFeedReader<T> reader, IUpdater<T> updater);
    }
}
