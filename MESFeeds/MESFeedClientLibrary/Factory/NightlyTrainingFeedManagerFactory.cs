using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Model.Training.DataAccess;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System.Configuration;

namespace MESFeedClientLibrary.Factory
{
    public class NightlyTrainingFeedManagerFactory : IFeedManagerFactory
    {
        private const int MILLISECONDS_PER_MINUTE = 60000;

        public IFeedManager Create()
        {
            var interval = ConfigurationManager.AppSettings["TrainingRecordIntervalMinutes"];
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


            IQuery queryCur = new TrainingRecordCurrency();
            IQuery queryExp = new TrainingRecordExpiration();

            IIntervalTrigger trigger = new IntervalTimer(int.Parse(interval) * MILLISECONDS_PER_MINUTE);
            IXmlProcessor xmlProcessor = new XmlProcessor(endPoint);
            ILogger logger = new LoggerBuilder()
                                        .AttachMessageWriter(new EmailAlertHandler(fromEmail, recipients, smtp))
                                        .AttachErrorWriter(new ErrorHandlerNoAlert(logFolder))
                                        .AttachActivityWriter(new TrainingActivity())
                                        .Build();

            IMessageReader reader = new NightlyMessageReader(queryCur, queryExp, logger, connection, MILLISECONDS_PER_MINUTE);
            IMessageUpdater updater = new NightlyTrainingMessageUpdater(xmlProcessor, logger, BusinessLayer.InterfaceTypes.TrainingRecord);

            IFeedManager bfm = new NightlyTrainingFeedManager(reader, updater, xmlProcessor, trigger, logger);

            return bfm;
        }

    }
}
