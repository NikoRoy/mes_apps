using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Controller
{
    public class TDBController 
    {
        protected string Connection { get; set; }
        protected IErrorHandler ErrorHandler { get; set; }
        protected IAlertHandler AlertHandler { get; set; }

        public TDBController()
        {
            this.Connection = ConfigurationManager.ConnectionStrings["TDB"].ConnectionString;
            string fromemail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string recipients = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtp = ConfigurationManager.AppSettings["SmtpServerName"];
            string logfolder = ConfigurationManager.AppSettings["LogFolderPRD"];
            if (ConfigurationManager.AppSettings["instance"] == "local")
            {
                logfolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            this.AlertHandler = new EmailAlertHandler(fromemail, recipients, smtp);
            this.ErrorHandler = new ErrorHandler(logfolder, AlertHandler);
        }
        public TDBController(string cn)
        {
            this.Connection = cn;
            string fromemail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string recipients = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtp = ConfigurationManager.AppSettings["SmtpServerName"];
            string logfolder = ConfigurationManager.AppSettings["LogFolderPRD"];
            if (ConfigurationManager.AppSettings["instance"] == "local")
            {
                logfolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            this.AlertHandler = new EmailAlertHandler(fromemail, recipients, smtp);
            this.ErrorHandler = new ErrorHandler(logfolder, AlertHandler);
        }
    }
}
