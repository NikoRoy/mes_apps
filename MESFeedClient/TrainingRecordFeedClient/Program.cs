
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System.Configuration;
using TDBMESFeeds.Messages;
using TrainingRecordFeedClient.Classes;



namespace TrainingRecordFeedClient
{
    class Program
    {
        private const int MILLISECONDS_PER_MINUTE = 1000 * 60;
        static void Main(string[] args)
        {
            //var manager = ConstructManager();
            //manager.Start();

            IFeedManager manager = new MESFeedClientLibrary.Factory.TrainingFeedManagerFactory().Create();
            manager.Start();
        }
        private static IFeedManager<TrainingRecordDownload> ConstructManager()
        {
            string url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            string tdb = ConfigurationManager.ConnectionStrings["TDB"].ConnectionString;
            string mes = ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
            string logFolder = ConfigurationManager.AppSettings["LogFolder"];
            if (ConfigurationManager.AppSettings["instance"] == "local")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            }
            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];
            int.TryParse(ConfigurationManager.AppSettings["RetryAttempts"], out int attempts);
            int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["TrainingRecordIntervalMinutes"]);

            IIntervalTrigger intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);
            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);
            //FileLogger errorHandler = new FileLogger( LoggingMethods.LogTypes.TrainingRecord.ToString() ,logFolder);
            IErrorHandler errorHandler = new RecordErrorHandler(logFolder, alertHandler);
            IFeedReader<TrainingRecordDownload> reader = new RecordFeedReader(errorHandler, tdb, MILLISECONDS_PER_MINUTE * 5, mes, attempts);
            IXmlProcessor processor = new XmlProcessor(url);
            //TrainingRecordLogger entityLogger = new TrainingRecordLogger(LoggingMethods.LogTypes.TrainingRecord.ToString());
            IActivityLogger<TrainingRecordDownload> activityLogger = new RecordActivityLogger(mes, errorHandler);
            IUpdater<TrainingRecordDownload> updater = new RecordXmlUpdater(processor, activityLogger, errorHandler);

            return new RecordFeedManager(
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
