using System.Configuration;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Training.DataAccess;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;


namespace MESFeedClientLibrary.Factory
{
    public class DocumentFeedManagerFactory : IFeedManagerFactory
    {
        private const int MILLISECONDS_PER_MINUTE = 60000;
        public FeedManagers.IFeedManager Create()
        {
            var interval = ConfigurationManager.AppSettings["TrainingDocIntervalMinutes"];
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

            IQuery query = new DocumentQuery();
            IIntervalTrigger trigger = new IntervalTimer(int.Parse(interval) * MILLISECONDS_PER_MINUTE); 
            IXmlProcessor xmlProcessor = new XmlProcessor(endPoint);
            Logger.ILogger logger = new Logger.LoggerBuilder()
                                        .AttachMessageWriter(new EmailAlertHandler(fromEmail, recipients, smtp))
                                        .AttachErrorWriter(new ErrorHandlerNoAlert(logFolder))
                                        .AttachActivityWriter(new DocumentActivity())
                                        .Build();

            IMessageReader reader = new TrainingMessageReader(query, logger, connection, MILLISECONDS_PER_MINUTE);
            IMessageUpdater updater = new MessageUpdater(xmlProcessor, logger, BusinessLayer.InterfaceTypes.TrainingDocument);

            IFeedManager bfm = new FeedManager(query, reader, updater, xmlProcessor, trigger, logger);

            return bfm;
        }
    }
}
