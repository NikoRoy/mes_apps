using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;


namespace OracleInventoryFeedClient.Classes
{
    public class OracleInventoryFeedManager : FeedManager<InventoryDownload>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<InventoryDownload> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;

        private readonly IFeedReader<InventoryDownload> FeedReader;
        private readonly IUpdater<InventoryDownload> xmlUpdater;

        public OracleInventoryFeedManager
        (
            IIntervalTrigger it,
            IXmlProcessor processor,
            IActivityLogger<InventoryDownload> alogger,
            IErrorHandler ehandler,
            IFeedReader<InventoryDownload> reader,
            IUpdater<InventoryDownload> updater
        ): base(it, processor,alogger,ehandler,reader,updater)
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
                var updater = new OracleInventoryXmlUpdater(xmlProcessor, activityLogger, errorHandler);
                Task.WaitAll(
                        UpdateAsync(FeedReader, updater)
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

        

        public override async Task UpdateAsync(IFeedReader<InventoryDownload> reader, IUpdater<InventoryDownload> updater)
        {
            var item = await reader.GetRecordsAsync();
            await updater.UpdateAsync(item);
        }

        //From Console
        public override void Start()
        {
            this.ProcessFeed();
        }
        private void ProcessFeed()
        {
            try
            {
                //perform http requests and process response
                var updater = new OracleInventoryXmlUpdater(xmlProcessor, activityLogger, errorHandler);
                Task.WaitAll(
                        UpdateAsync(FeedReader, updater)
                    );

            }
            catch (Exception ex)
            {
                this.errorHandler.LogError(ex, "Error Processing Feed");
            }
            finally
            {
            }
        }

    }
}
