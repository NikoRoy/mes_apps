using System.Configuration;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Alert;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Error;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Training.DataAccess;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;

namespace MESFeedClientLibrary.Factory
{
    public class BundleFeedManagerFactory : IFeedManagerFactory
    {
        private const int MILLISECONDS_PER_MINUTE = 60000;
        public IFeedManager Create()
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



            IQuery query = new TrainingBundleQuery();
            IIntervalTrigger trigger = new IntervalTimer(int.Parse(interval) * MILLISECONDS_PER_MINUTE); //3 minutes
            IXmlProcessor xmlProcessor = new XmlProcessor(endPoint);

            Logger.ILogger logger = new Logger.LoggerBuilder()
                                        .AttachMessageWriter(new InterfaceTypeAlertHandler(fromEmail, recipients, smtp, BusinessLayer.InterfaceTypes.TrainingGroup))
                                        .AttachErrorWriter(new InterfaceTypeErrorHandler(logFolder, BusinessLayer.InterfaceTypes.TrainingGroup))
                                        .AttachActivityWriter(new BundleActivity())
                                        .Build();

            IMessageReader reader = new TrainingMessageReader(query, logger, connection, MILLISECONDS_PER_MINUTE);
            IMessageUpdater updater = new MessageUpdater(xmlProcessor, logger, BusinessLayer.InterfaceTypes.TrainingGroup);

            IFeedManager bfm = new BundleFeedManager(query, reader, updater, xmlProcessor, trigger, logger);

            return bfm;
        }
    }
}
