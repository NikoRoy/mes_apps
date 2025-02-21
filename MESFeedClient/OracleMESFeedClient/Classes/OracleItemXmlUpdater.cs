using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;
using Newtonsoft.Json;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace MESFeedClient.Classes
{
    
    public class OracleItemXmlUpdater : XmlUpdater<Item>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<Item> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public OracleItemXmlUpdater(IXmlProcessor xmlProcessor, IActivityLogger<Item> activityLogger, IErrorHandler errorHandler) : base(xmlProcessor, activityLogger, errorHandler)
        {
            this.Processor = xmlProcessor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }

        public override async Task UpdateAsync(IEnumerable<Item> itemRecords, DateTime date = default(DateTime))
        {
            if (itemRecords.Count() <= 0) { return; }

            foreach (Item rec in itemRecords)
            {
                try
                {
                    var response = await Processor.Execute(rec.ToXml());
                    Console.WriteLine(response);

                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        //Send Error to oracle
                        await ErrorHandler.LogError(new Exception(response), "Oracle Item Exception");
                    }
                }
                catch (Exception ex)
                {
                    //Alert IT
                    await ErrorHandler.LogError(ex, "Error Processing Oracle Item");
                }
            }
        }

        #region old logic
        //public async Task UpdateInventory(IEnumerable<InventoryDownload> inventoryRecords)
        //{
        //    if(inventoryRecords.Count() <= 0) { return; }

        //    Processor.URI = ((XmlProcessor)Processor).Root + ApiRoutingAdjustment.oracleinventory.ToString();

        //    foreach (var rec in inventoryRecords)
        //    {
        //        try
        //        {

        //            var response = await Processor.Execute(rec.ToXml());
        //            await ActivityLogger.LogInventory(rec, "Post Request", response);

        //            if (!WasResponseSuccessful(response))
        //            {
        //                await this.ErrorHandler.LogError(new Exception(response), "Oracle Inventory API Was Unsuccessful");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await this.ErrorHandler.LogError(ex, "Error Processing Oracle Inventory");
        //        }
        //    }
        //}

        //public async Task UpdateItems(IEnumerable<Item> itemRecords)
        //{
        //    if (itemRecords.Count() <= 0) { return; }

        //    Processor.URI = ((XmlProcessor)Processor).Root + ApiRoutingAdjustment.oracleitem.ToString();

        //    foreach (var rec in itemRecords)
        //    {
        //        try
        //        {
        //            var response = await Processor.Execute(rec.ToXml());
        //            await ActivityLogger.LogItem(rec, "Post Request", response);
        //            if (!WasResponseSuccessful(response))
        //            {
        //                await this.ErrorHandler.LogError(new Exception(response), "Oracle Items API Was Unsuccessful");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await this.ErrorHandler.LogError(ex, "Error Processing Oracle Item");
        //        }
        //    }
        //}

        //public async Task UpdateWorkOrders(IEnumerable<WorkOrder> workOrderRecords)
        //{
        //    if (workOrderRecords.Count() <= 0) { return; }

        //    Processor.URI = ((XmlProcessor)Processor).Root + ApiRoutingAdjustment.oracleworkorder.ToString();

        //    foreach (var rec in workOrderRecords)
        //    {
        //        try
        //        {

        //            var response = await Processor.Execute(rec.ToXml());
        //            await ActivityLogger.LogWorkOrder(rec, "Post Request", response);
        //            if (!WasResponseSuccessful(response))
        //            {
        //                await this.ErrorHandler.LogError(new Exception(response), "Oracle Work Order API Was Unsuccessful");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await this.ErrorHandler.LogError(ex, "Error Processing Oracle Work Order");
        //        }
        //    }
        //}
        #endregion
    }
}
