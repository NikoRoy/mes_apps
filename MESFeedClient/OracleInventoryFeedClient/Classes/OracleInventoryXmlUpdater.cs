using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;


namespace OracleInventoryFeedClient.Classes
{
    public class OracleInventoryXmlUpdater : XmlUpdater<InventoryDownload>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<InventoryDownload> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public OracleInventoryXmlUpdater(IXmlProcessor xmlProcessor, IActivityLogger<InventoryDownload> activityLogger, IErrorHandler errorHandler) : base(xmlProcessor, activityLogger, errorHandler)
        {
            this.Processor = xmlProcessor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }

        public override async Task UpdateAsync(IEnumerable<InventoryDownload> invRecords, DateTime date = default(DateTime))
        {
            if (invRecords.Count() <= 0) { return; }

            foreach (InventoryDownload rec in invRecords)
            {
                try
                {
                    var response = await Processor.Execute(rec.ToXml());
                    Console.WriteLine(response);
                    //insert into oracle message notification table
                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        //write to log file.
                        await ErrorHandler.LogError(new Exception(response), "Inventory Exception");
                    }
                }
                catch (Exception ex)
                {
                    //send alert to IT
                    await this.ErrorHandler.LogError(ex, "Error Processing Oracle Item");
                }
            }
        }
        
    }
}
