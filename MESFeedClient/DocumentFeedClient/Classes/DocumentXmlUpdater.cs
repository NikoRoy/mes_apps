using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.Messages;

namespace DocumentFeedClient.Classes
{
    public class DocumentXmlUpdater : XmlUpdater<DocumentDownload>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<DocumentDownload> activityLogger;
        private readonly IErrorHandler errorHandler;

        public DocumentXmlUpdater(IXmlProcessor xmlProcessor, IActivityLogger<DocumentDownload> activityLogger,IErrorHandler errorHandler): base(xmlProcessor, activityLogger, errorHandler)
        {
            this.Processor = xmlProcessor;
            this.activityLogger = activityLogger;
            this.errorHandler = errorHandler;
        }


        public override async Task UpdateAsync(IEnumerable<DocumentDownload> objlist, DateTime date = default(DateTime))
        {
            if (objlist.Count() <= 0) { return; }
            bool atleastOneError = false;
            foreach (DocumentDownload rec in objlist)
            {
                try
                {
                    var response = await Processor.Execute(rec.ToXml());
                    await activityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        atleastOneError = true;
                        await this.errorHandler.LogError(new Exception(response), "MES Training Document API Was Unsuccessful");
                    }
                }
                catch (Exception ex)
                {
                    atleastOneError = true;
                    await this.errorHandler.LogError(ex, "Error Processing MES Training Document");
                }
            }
            if (!atleastOneError)
            {
                try
                {
                    await BusinessExtensions.UpdateControlTime(date, InterfaceTypes.TrainingDocument);
                }
                catch (Exception ex)
                {
                    await this.errorHandler.LogError(ex, "Error Updating Training Document Feed Control Date");
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


            var k = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "__errorDescription").SingleOrDefault();
            if (k != null)
            {
                return false;
            }

            var g = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "CompletionMsg").SingleOrDefault();
            if (g != null)
            {
                return true;
            }

            return false;

        }

    }
}
