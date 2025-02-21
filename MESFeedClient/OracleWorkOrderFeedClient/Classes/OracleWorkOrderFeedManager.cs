
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace OracleWorkOrderFeedClient.Classes
{
    public class OracleWorkOrderFeedManager : FeedManager<WorkOrder>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<WorkOrder> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        private readonly IFeedReader<WorkOrder> FeedReader;
        private readonly IUpdater<WorkOrder> xmlUpdater;

        public OracleWorkOrderFeedManager
        (
            IIntervalTrigger it,
            IXmlProcessor processor,
            IActivityLogger<WorkOrder> alogger,
            IErrorHandler ehandler,
            IFeedReader<WorkOrder> reader,
            IUpdater<WorkOrder> updater
        ) : base (it,processor, alogger, ehandler, reader, updater)
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
                var updater = new OracleWorkOrderXmlUpdater(xmlProcessor, activityLogger, errorHandler);                
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
        public override async Task UpdateAsync(IFeedReader<WorkOrder> reader, IUpdater<WorkOrder> updater)
        {
            var wo = await reader.GetRecordsAsync();
            await updater.UpdateAsync(wo);
        }
        public override void Start()
        {
            this.ProcessFeed();
        }
        private void ProcessFeed()
        {
            try
            {
                //perform http requests and process response
                var updater = new OracleWorkOrderXmlUpdater(xmlProcessor, activityLogger, errorHandler);
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
