using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Model.BlueMountaion;

namespace MESFeedClientLibrary.Factory
{
    public class BlueMountainFeedManagerFactory : FeedManagerFactory
    {
        protected override IFeedManager Create()
        {
            var interval = ConfigurationManager.AppSettings["BundleIntervalMinutes"];
            var endPoint = ConfigurationManager.AppSettings["MESWebApiUrl"];
            var connection = ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
            var fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            var recipients = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            var smtp = ConfigurationManager.AppSettings["SmtpServerName"];
            string logFolder = ConfigurationManager.AppSettings["LogFolder"];
            if (ConfigurationManager.AppSettings["instance"] == "local")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            }

            IQuery query = new AssetQuery();
            IXmlProcessor xmlProcessor = new XmlProcessor(endPoint);
            ILogger logger = new LoggerBuilder()
                                        .AttachMessageWriter(new EmailAlertHandler(fromEmail, recipients, smtp))
                                        .AttachErrorWriter(new ErrorHandlerNoAlert(logFolder))
                                        .AttachActivityWriter(new BlueMountainActivity())
                                        .Build();

            IFeedFactory bfm = new BlueMountainFeedFactory(query, xmlProcessor, logger);
            return new BlueMountainFeedManager(bfm);
        }
    }
}
