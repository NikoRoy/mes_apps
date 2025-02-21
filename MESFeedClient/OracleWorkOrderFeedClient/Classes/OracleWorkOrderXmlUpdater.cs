using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Oracle.Messages;


namespace OracleWorkOrderFeedClient.Classes
{
    public class OracleWorkOrderXmlUpdater : XmlUpdater<WorkOrder>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<WorkOrder> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public OracleWorkOrderXmlUpdater(IXmlProcessor xmlProcessor, IActivityLogger<WorkOrder> activityLogger, IErrorHandler errorHandler) :base(xmlProcessor, activityLogger, errorHandler)
        {
            this.Processor = xmlProcessor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }
        public override async Task UpdateAsync(IEnumerable<WorkOrder> woRecords, DateTime date = default(DateTime))
        {
            if (woRecords.Count() <= 0) { return; }

            foreach (WorkOrder rec in woRecords)
            {
                try
                {
                    Console.WriteLine(rec.ToXml());
                    var response = await Processor.Execute(rec.ToXml());
                    Console.WriteLine(response);
                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        //send error to oracle
                        await ErrorHandler.LogError(new Exception(response), "Work Order Exception");
                    }
                }
                catch (Exception ex)
                {
                    //send alert to IT
                    await this.ErrorHandler.LogError(ex, "Error Processing Oracle Item");
                }
            }
        }
        
        public override bool WasResponseSuccessful(string response)
        {
            XDocument xmlDoc = XDocument.Parse(response);

            var isresponse = from x in xmlDoc.Root.Elements()
                             where x.Name.LocalName == "IsResponse"
                             select x.Value;

            if (isresponse.SingleOrDefault() == "false")
            {
                return false;
            }


            var content = from x in xmlDoc.Root.Elements()
                          where x.Name.LocalName == "Contents"
                          select x.Value;

            var h = XElement.Parse(content.SingleOrDefault());


            var k = h.DescendantsAndSelf().Elements().Where(p => p.Name.LocalName == "__errorDescription").SingleOrDefault();
            if (k != null)
            {
                return false;
            }

            var g = h.DescendantsAndSelf().Elements().Where(p => p.Name.LocalName == "CompletionMsg").SingleOrDefault();
            if (g != null)
            {
                return true;
            }

            return false;
        }
    }
}
