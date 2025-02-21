using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WebApplication1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected ErrorHandler ErrorHandler { get; private set; }
        
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //this.ErrorHandler = new ErrorHandler(ConfigurationManager.AppSettings["logFolder"], new EmailAlertHandler(ConfigurationManager.AppSettings["fromEmail"], ConfigurationManager.AppSettings["recipients"], ConfigurationManager.AppSettings["smtp"]));
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            this.ErrorHandler.LogError(Server.GetLastError(), "Global Error Handler");
            return;
        }
        protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }
        }
    }
}
