using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary;
using System.Timers;
using TDBModelLibrary;
using System.Configuration;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;

namespace TrainingActionCloneClient
{
    partial class TrainingActionCloneService : ServiceBase
    {
        protected Timer Timer;
        protected CloneManager CloneManager;
        public TrainingActionCloneService()
        {
            InitializeComponent();

            ConfigureLogandAlerter();
            CloneManager = ConfigureManager();

            Timer = ConfigureTimer();
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Timer elasped: " + DateTime.Now);
                //Timer.Stop();
                CloneManager.CloneActions();
                //Timer.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Training Action Clone Error");
            }        
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            Timer.Start();
            //Timer_Elapsed(this, null);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            Timer.Stop();
            Timer.Elapsed -= Timer_Elapsed;
            Timer.Dispose();
        }

        internal void StartService()
        {
            Timer.Start();
            //Timer_Elapsed(this, null);
        }

        private CloneManager ConfigureManager()
        {
            var destinationCn = ConfigurationManager.ConnectionStrings["EmployeeTrainingEntitiesDEST"].ConnectionString;
            return new CloneManager(destinationCn);
        }
        private void ConfigureLogandAlerter()
        {
            var smtp = ConfigurationManager.AppSettings["smtp"];
            var recip = ConfigurationManager.AppSettings["recipients"];
            var fromemail = ConfigurationManager.AppSettings["fromemail"];
            IAlertHandler ah = new EmailAlertHandler(fromemail, recip, smtp);
            var log = ConfigurationManager.AppSettings["log"];
            FileLogger l = new FileLogger(log, ah);
        }
        private Timer ConfigureTimer()
        {
            //configured polling minutes
            int pi = int.Parse(ConfigurationManager.AppSettings["pollinginterval"]);
            //interval in milliseconds
            double interval = (pi * 60000); 
            Timer t = new Timer(interval);
            t.Elapsed += Timer_Elapsed;
            return t;
        }
    }
}
