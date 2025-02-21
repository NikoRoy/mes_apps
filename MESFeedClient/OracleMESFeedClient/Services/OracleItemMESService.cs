using MESFeedClientLibrary.Interfaces;
using MESFeedClient.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Model.Oracle.Messages;

namespace MESFeedClient.Services
{
    partial class OracleItemMESService : ServiceBase 
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;

        private readonly IFeedManager<Item> FeedManager;
        public OracleItemMESService()
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

        private IFeedManager<Item> ConstructFeedManager()
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
            IErrorHandler errorHandler = new OracleItemErrorHandler(logFolder, alertHandler);

            int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["OracleIntervalMinutes"]);
            var intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);

            string Url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            string connectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            IActivityLogger<Item> activityLogger = new OracleItemActivityLogger(connectionString, errorHandler);
            IXmlProcessor processor = new XmlProcessor(Url);
            IFeedReader<Item> feedreader = new OracleItemFeedReader(errorHandler, connectionString);
            IUpdater<Item> xmlupdater = new OracleItemXmlUpdater(processor, activityLogger, errorHandler);

            return new
                OracleItemFeedManager
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
