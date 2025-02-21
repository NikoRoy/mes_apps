using MESFeedClientEFModel;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.DataAccess;
using OracleMESFeeds.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BusinessLayer.Controller
{
    public class OracleController
    {
        protected readonly string OraclePackage = "Fill in Oracle Package";
        protected string Connection { get; set; }
        protected IErrorHandler SqlErrorHandler { get; set; }
        protected IAlertHandler AlertHandler { get; set; }

        public OracleController(string oracn)
        {
            this.Connection = oracn;
            string fromemail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string recipients = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtp = ConfigurationManager.AppSettings["SmtpServerName"];
            string logfolder = ConfigurationManager.AppSettings["LogFolderPRD"];
            if(ConfigurationManager.AppSettings["instance"] == "local")
            {
                logfolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            this.AlertHandler = new EmailAlertHandler(fromemail, recipients, smtp);
            this.SqlErrorHandler = new ErrorHandler(logfolder, AlertHandler);
        }
        public OracleController()
        {
            this.Connection = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            string fromemail = ConfigurationManager.AppSettings["FromEmailAddress"];
            string recipients = ConfigurationManager.AppSettings["AlertRecipientEmailAddress"];
            string smtp = ConfigurationManager.AppSettings["SmtpServerName"];
            string logfolder = ConfigurationManager.AppSettings["LogFolderPRD"];
            if (ConfigurationManager.AppSettings["instance"] == "local")
            {
                logfolder = ConfigurationManager.AppSettings["LogFolder"];
            }
            this.AlertHandler = new EmailAlertHandler(fromemail, recipients, smtp);
            this.SqlErrorHandler = new ErrorHandler(logfolder, AlertHandler);
        }
     

        public static IRecordController GetTypeFromFactory(OracleRecordType ort)
        {
            return OracleControllerFactory.DeterminerController(ort);
        }
    }
}
