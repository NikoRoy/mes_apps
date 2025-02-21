
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using OracleInventoryFeedClient.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace OracleInventoryFeedClient.Services
{
    partial class OracleInventoryMESService : ServiceBase
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;

        private readonly IFeedManager<InventoryDownload> FeedManager;
        public OracleInventoryMESService()
        {
            InitializeComponent();
            this.FeedManager = ConstructFeedManager();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            this.FeedManager.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            this.FeedManager.Stop();
        }
        private IFeedManager<InventoryDownload> ConstructFeedManager()
        {
            //instantiate FeedManager object
            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];
            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);

            string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            if (ConfigurationManager.AppSettings["instance"] == "prd")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            IErrorHandler errorHandler = new OracleInventoryErrorHandler(logFolder, alertHandler);

            int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["OracleIntervalMinutes"]);
            var intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);

            string Url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            string connectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            IActivityLogger<InventoryDownload> activityLogger = new OracleInventoryActivityLogger(connectionString, errorHandler);
            IXmlProcessor processor = new XmlProcessor(Url);
            IFeedReader<InventoryDownload> feedreader = new OracleInventoryFeedReader(errorHandler, connectionString);
            IUpdater<InventoryDownload> xmlupdater = new OracleInventoryXmlUpdater(processor, activityLogger, errorHandler);

            return new
                OracleInventoryFeedManager
                (
                    intervalTrigger,
                    processor,
                    activityLogger,
                    errorHandler,
                    feedreader,
                    xmlupdater
                );
        }
        public void StartService()
        {
            this.FeedManager.Start();
            Console.WriteLine("starting service");
        }
    }
}
