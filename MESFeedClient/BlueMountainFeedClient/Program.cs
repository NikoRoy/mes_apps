using MESFeedClientLibrary.Classes;
using System.Configuration;
using System.Threading.Tasks;
using System.ServiceProcess;
using MESFeedClientLibrary.SBC;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Activity;
using Azure.Messaging.ServiceBus;
using BlueMountainFeedClient.Services;
using System;

namespace BlueMountainFeedClient
{
    class Program
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;
        static void Main(string[] args)
        {
            RunAsConsole();
            //RunAsService();
        }

        private static void RunAsService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BlueMountainService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        private static void RunAsConsole()
        {
            BlueMountainService i = new BlueMountainService();
            i.StartConsole();

            Console.WriteLine("Running service as console. Press any key to stop.");
            Console.ReadKey();

            i.Stop();
        }


        //private static BusManager ConstructServiceBusManager()
        //{
        //    string topic = ConfigurationManager.AppSettings["Topic"];
        //    string subscription = ConfigurationManager.AppSettings["Subscription"];
        //    string connection = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;

        //    string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
        //    string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
        //    string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];

        //    string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
        //    if (ConfigurationManager.AppSettings["instance"] == "prd")
        //    {
        //        logFolder = ConfigurationManager.AppSettings["LogFolder"];
        //    }

        //    ILogger logger = new MESFeedClientLibrary.Logger.LoggerBuilder()
        //                                .AttachMessageWriter(new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName))
        //                                .AttachErrorWriter(new ErrorHandlerNoAlert(logFolder))
        //                                .AttachActivityWriter(new ServiceBusActivity())
        //                                .Build();
        //    return new BusManager(connection, topic, subscription, logger);
        //}


        //private static IFeedManager<AssetDownload> ConstructManager()
        //{
        //    string url = ConfigurationManager.AppSettings["MESWebApiUrl"];
        //    string connectionString = ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
        //    string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
        //    if (ConfigurationManager.AppSettings["instance"] == "prd")
        //    {
        //        logFolder = ConfigurationManager.AppSettings["LogFolder"];
        //    }
        //    string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
        //    string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
        //    string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];
        //    int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["BMIntervalMinutes"]);

        //    IIntervalTrigger intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);
        //    IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);
        //    IErrorHandler errorHandler = new ErrorHandler(logFolder, alertHandler);
        //    IFeedReader<AssetDownload> reader = new AssetFeedReader(errorHandler, connectionString);
        //    IXmlProcessor processor = new XmlProcessor(url);
        //    IActivityLogger<AssetDownload> activityLogger = new AssetActivityLogger(connectionString, errorHandler);
        //    IUpdater<AssetDownload> updater = new AssetXmlUpdater(processor, activityLogger, errorHandler);

        //    return new AssetFeedManager(
        //            intervalTrigger,
        //            processor,
        //            activityLogger,
        //            errorHandler,
        //            reader,
        //            updater
        //        );
        //}
    }
}
