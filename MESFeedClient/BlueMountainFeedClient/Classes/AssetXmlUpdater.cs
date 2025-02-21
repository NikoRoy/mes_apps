using BMRAMMesFeeds.Messages;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueMountainFeedClient.Classes
{
    public class AssetXmlUpdater : MESFeedClientLibrary.Classes.XmlUpdater<AssetDownload>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<AssetDownload> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public AssetXmlUpdater(IXmlProcessor processor, IActivityLogger<AssetDownload> activityLogger, IErrorHandler errorHandler):base(processor, activityLogger, errorHandler)
        {
            this.Processor = processor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }
        public override async Task UpdateAsync(IEnumerable<AssetDownload> objlist, DateTime date = default(DateTime))
        {
            if (objlist.Count() <= 0) { return; }
            bool atleastOneError = false;

            foreach (AssetDownload rec in objlist)
            {
                try
                {
                    var response = await Processor.Execute(rec.ToXml());
                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        atleastOneError = true;
                        await this.ErrorHandler.LogError(new Exception(response), "Blue Mountain Feed Was Unsuccessful");
                    }
                }
                catch (Exception ex)
                {
                    atleastOneError = true;
                    await this.ErrorHandler.LogError(ex, "Error Processing Blue Mountain Asset");
                }
            }

            if (!atleastOneError)
            {
                try
                {
                    await BusinessExtensions.UpdateControlTime(date, InterfaceTypes.BlueMountain);
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error Updating Blue Mountain Feed Control Date");
                }
                
            }
        }
    }
}
