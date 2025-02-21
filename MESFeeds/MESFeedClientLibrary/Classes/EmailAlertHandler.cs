using System;
using System.Net.Mail;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Classes
{
    public class EmailAlertHandler : IAlertHandler
    {
        readonly string FromEmail;
        readonly string Recipients;
        readonly string Smtp;
        public EmailAlertHandler(string fromemail,string recipients, string smtp)
        {
            this.FromEmail = fromemail;
            this.Recipients = recipients;
            this.Smtp = smtp;
        }
        

        public async Task SendAlert(string alertMessage, string subject)
        {
            using (MailMessage message = new MailMessage(this.FromEmail, this.Recipients, subject, alertMessage))
            {
                using (SmtpClient smtpClient = new SmtpClient(this.Smtp))
                {
                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    
                }
            }
        }
    }
}
