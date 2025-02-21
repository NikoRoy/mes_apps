
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using OracleWorkOrderFeedClient.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using MESFeedClientLibrary.Model.Oracle.Messages;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClient.Services
{
    partial class OracleWorkOrderMESService : ServiceBase
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;

        private readonly IFeedManager<WorkOrder> FeedManager;
        public OracleWorkOrderMESService()
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
        private IFeedManager<WorkOrder> ConstructFeedManager()
        {
            //instantiate FeedManager object
            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];
            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);

            string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            if(ConfigurationManager.AppSettings["instance"] == "prd")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            IErrorHandler errorHandler = new OracleWorkOrderErrorHandler(logFolder, alertHandler);

            int IntervalMinutes = int.Parse(ConfigurationManager.AppSettings["OracleIntervalMinutes"]);
            var intervalTrigger = new IntervalTimer(IntervalMinutes * MILLISECONDS_PER_MINUTE);

            string Url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            string connectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            IActivityLogger<WorkOrder> activityLogger = new OracleWorkOrderActivityLogger(connectionString, errorHandler);
            XmlProcessor xml = new XmlProcessor(Url);
            IFeedReader<WorkOrder> feedreader = new OracleWorkOrderFeedReader(errorHandler, connectionString); 
            IUpdater<WorkOrder> xmlupdater = new OracleWorkOrderXmlUpdater(xml, activityLogger, errorHandler);

            return new
                OracleWorkOrderFeedManager
                (
                    intervalTrigger,
                    xml,
                    activityLogger,
                    errorHandler,
                    feedreader, 
                    xmlupdater
                );
        }
        public void StartService()
        {
            Console.WriteLine("starting service");
            this.FeedManager.Start();
            
        }
    }
}
