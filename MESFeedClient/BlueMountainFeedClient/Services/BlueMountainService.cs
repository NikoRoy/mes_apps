using Azure.Messaging.ServiceBus;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Factory;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.SBC;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BlueMountainFeedClient.Services
{
    partial class BlueMountainService : ServiceBase
    {
        public BusManager busManager;
        public IFeedManager blueMountainFeedManager;
        public BlueMountainService()
        {
            InitializeComponent();
            this.busManager = ConstructServiceBusManager();
            this.blueMountainFeedManager = ConstructBlueMountainManager();
        }


        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            //base.OnStart(args);

            ///
            /// Can call processor start async directly here (unblocking) and return control to service control manager 
            /// //try
                //{
                //    ServiceBusProcessor processor = FeedManager.GetSBProcessorInstance();
                //    Task startTask = Task.Run(async () => await processor.StartProcessingAsync(), new System.Threading.CancellationToken());
                //}
                //finally
                //{
                //    FeedManager.Dispose();
                //}
            ///
            try
            {
                Task.Run(async () => await StartProcessing());//.ConfigureAwait(false);
            }
            catch (System.Exception)
            {
                throw;
            }
            

        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            Task.Run(async () => await StopProcessing()).Wait();

            //busManager.Dispose();
            //blueMountainFeedManager.Dispose();
            base.OnStop();
        }

        public void StartConsole()
        {
            try
            {
                var t = Task.Run(async () => await StartProcessing());
                t.Wait();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        private async Task StartProcessing()
        {
            var processor = busManager.GetSBProcessorInstance();
            try
            {
                await processor.StartProcessingAsync();
                this.blueMountainFeedManager.Start();

                //Console.WriteLine("Wait for a minute and then press any key to end the processing");
                //Console.ReadKey();

                // stop processing 
                //Console.WriteLine("\nStopping the receiver...");
                //await processor.StopProcessingAsync();
                //await StopProcessing();
                //Console.WriteLine("Stopped receiving messages");
                
            }
            catch(Exception)
            {
                //await processor.DisposeAsync();
                //await client.DisposeAsync();
                throw;
            }
        }
        private async Task StopProcessing()
        {
            var processor = busManager.GetSBProcessorInstance();
            try
            {
                // all in-flight messages will be awaited before processing ends
                if (processor.IsProcessing)
                    await processor.StopProcessingAsync();

                this.blueMountainFeedManager.Stop();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await processor.DisposeAsync();
                blueMountainFeedManager.Dispose();
            }
        }
        private IFeedManager ConstructBlueMountainManager()
        {
            //return new BlueMountainFeedManagerFactory().MakeManager();
            string url = ConfigurationManager.AppSettings["MESWebApiUrl"];
            IXmlProcessor processor = new XmlProcessor(url);


            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];

            string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            if (ConfigurationManager.AppSettings["instance"] == "prd")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolder"];
            }

            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);
            IErrorHandler errorHandler = new ErrorHandlerNoAlert(logFolder);
            ILogger meslogger = new LoggerBuilder()
                            .AttachMessageWriter(alertHandler)
                            .AttachErrorWriter(errorHandler)
                            .AttachActivityWriter(new BlueMountainOutboundActivity())
                            .Build();

            return new BlueMountainManager(processor, meslogger);
        }


        private BusManager ConstructServiceBusManager()
        {
            string topic = ConfigurationManager.AppSettings["Topic"];
            string subscription = ConfigurationManager.AppSettings["Subscription"];
            string connection = ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;

            string fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string alertRecipient = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtpServerName = ConfigurationManager.AppSettings["SmtpServerName"];

            string logFolder = ConfigurationManager.AppSettings["LogFolderLocal"];
            if (ConfigurationManager.AppSettings["instance"] == "prd")
            {
                logFolder = ConfigurationManager.AppSettings["LogFolder"];
            }

            IAlertHandler alertHandler = new EmailAlertHandler(fromEmail, alertRecipient, smtpServerName);
            IErrorHandler errorHandler = new ErrorHandlerNoAlert(logFolder);

            ILogger logger = new LoggerBuilder()
                                        .AttachMessageWriter(alertHandler)
                                        .AttachErrorWriter(errorHandler)
                                        .AttachActivityWriter(new IntakeActivityAdapter<MessageAdapter>(new ServiceBusActivity()))
                                        .AttachActivityWriter(new IntakeActivityAdapter<MessageAdapter>(new ServiceBusActivityDetail()))
                                        //.AttachActivityWriter(new BlueMountainOutboundActivity())
                                        //.AttachActivityWriter(new MessageAdapter())
                                        .Build();



            return new BusManager(connection, topic, subscription, logger);
        }
        
    }
}
