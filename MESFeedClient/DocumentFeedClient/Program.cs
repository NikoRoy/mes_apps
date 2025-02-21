using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.Messages;
using DocumentFeedClient.Classes;
using MESFeedClientLibrary.FeedManagers;

namespace DocumentFeedClient
{
    class Program
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;
        static void Main(string[] args)
        {
            //var manager = ConstructManager();
            IFeedManager manager = new MESFeedClientLibrary.Factory.DocumentFeedManagerFactory().Create();
            manager.Start();
        }

        private static IFeedManager<DocumentDownload> ConstructManager()
        {
            string url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            string connectionString = ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
            string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            if (ConfigurationManager.AppSettings["instance"] == "prd")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];
            int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["TrainingDocIntervalMinutes"]);

            IIntervalTrigger intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);
            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);
            IErrorHandler errorHandler = new ErrorHandler(logFolder, alertHandler);
            IFeedReader<DocumentDownload> reader = new DocumentFeedReader(errorHandler, connectionString);
            IXmlProcessor processor = new XmlProcessor(url);
            IActivityLogger<DocumentDownload> activityLogger = new DocumentActivityLogger(connectionString, errorHandler);
            IUpdater<DocumentDownload> updater = new DocumentXmlUpdater(processor, activityLogger, errorHandler);

            return new DocumentFeedManager(
                    intervalTrigger,
                    processor,
                    activityLogger,
                    errorHandler,
                    reader,
                    updater
                );
        }
    }
}
