using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace MESFeedClient.Classes
{
    public class OracleItemFeedManager : FeedManager<Item>
    {
        private readonly IIntervalTrigger intervalTrigger;
        private readonly IActivityLogger<Item> activityLogger;
        private readonly IErrorHandler errorHandler;
        private readonly IXmlProcessor xmlProcessor;
        
        private readonly IFeedReader<Item> FeedReader;
        private readonly IUpdater<Item> xmlUpdater;


        public OracleItemFeedManager
        (
            IIntervalTrigger it,
            IXmlProcessor processor,
            IActivityLogger<Item> alogger,
            IErrorHandler ehandler,
            IFeedReader<Item> reader,
            IUpdater<Item> updater
        ) : base(it, processor, alogger, ehandler, reader, updater)
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
                var updater = new OracleItemXmlUpdater(xmlProcessor, activityLogger, errorHandler);
                Task.WaitAll(
                        UpdateAsync(FeedReader, updater)
                    );

            }
            catch(Exception ex)
            {
                this.errorHandler.LogError(ex, "Error Processing Feed");
            }
            finally
            {
                this.intervalTrigger.Start();
            }
        }
        public override async Task UpdateAsync(IFeedReader<Item> reader, IUpdater<Item> updater)
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
                var updater = new OracleItemXmlUpdater(xmlProcessor, activityLogger, errorHandler);
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

        #region old logic
        //private async Task UpdateWorkOrderAsync(IFeedReader reader, IUpdater updater)
        //{
        //    var wo = await reader.GetWorkOrderRecordsAsync();
        //    await updater.UpdateWorkOrders(wo);
        //}
        //private async Task UpdateItemAsync(IFeedReader reader, IUpdater updater)
        //{
        //    var item = await reader.GetItemRecordsAsync();
        //    await updater.UpdateItems(item);
        //}
        //private async Task UpdateInventoryAsync(IFeedReader reader, IUpdater updater)
        //{
        //    var inv = await reader.GetInventoryRecordsAsync();
        //    await updater.UpdateInventory(inv);
        //}
        #endregion
    }
}
