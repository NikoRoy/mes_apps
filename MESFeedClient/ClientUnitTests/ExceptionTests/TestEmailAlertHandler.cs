using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUnitTests.ExceptionTests
{
    
    public class TestEmailAlertHandler : IAlertHandler
    {
        readonly string FromEmail;
        readonly string Recipients;
        readonly string Smtp;
        public string testfinal = "";
        public TestEmailAlertHandler(string fromemail, string recipients, string smtp)
        {
            this.FromEmail = fromemail;
            this.Recipients = recipients;
            this.Smtp = smtp;
        }
        public async Task SendAlert(string message, string subject)
        {
            await Task.Run(()=>this.testfinal = string.Concat( message, subject));
        }
    }
}
