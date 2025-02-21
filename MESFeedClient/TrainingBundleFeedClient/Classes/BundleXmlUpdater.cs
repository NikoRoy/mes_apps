using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TDBMESFeeds.Messages;

namespace TrainingBundleFeedClient.Classes
{
    public class BundleXmlUpdater : XmlUpdater<TrainingBundleDownload>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<TrainingBundleDownload> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public BundleXmlUpdater(IXmlProcessor processor, IActivityLogger<TrainingBundleDownload> activityLogger, IErrorHandler errorHandler) : base(processor, activityLogger, errorHandler)
        {
            this.Processor = processor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }

        public override async Task UpdateAsync(IEnumerable<TrainingBundleDownload> objlist, DateTime date = default(DateTime))
        {
            if (objlist.Count() <= 0) { return; }
            bool atleastOneError = false;
            foreach (TrainingBundleDownload rec in objlist)
            {
                try
                {
                    Console.WriteLine(rec.ToXml());
                    var response = await Processor.Execute(rec.ToXml());
                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        atleastOneError = true;
                        await this.ErrorHandler.LogError(new Exception(response), "MES Training Bundle Download Was Unsuccessful");
                    }
                }
                catch (Exception ex)
                {
                    atleastOneError = true;
                    await this.ErrorHandler.LogError(ex, "Error Processing MES Training Bundles");
                }
            }
            if (!atleastOneError)
            {
                try
                {
                    await BusinessExtensions.UpdateControlTime(date, InterfaceTypes.TrainingGroup);
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error Updating Training Group Feed Control Date");
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
