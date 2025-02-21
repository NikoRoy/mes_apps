using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Alert
{
    public class InterfaceTypeAlertHandler : IAlertHandler 
    {
        public string FromEmail { get; }
        public string Recipients { get; }
        public string Smtp { get; }
        public InterfaceTypes Type { get; }

        public InterfaceTypeAlertHandler(string fromemail, string recipients, string smtp, BusinessLayer.InterfaceTypes type)
        {
            this.FromEmail = fromemail;
            this.Recipients = recipients;
            this.Smtp = smtp;
            this.Type = type;
        }

        public async Task SendAlert(string alertMessage, string subject)
        {
            using (MailMessage message = new MailMessage(this.FromEmail, this.Recipients, FormatSubjectLine(subject), alertMessage))
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
        private string FormatSubjectLine(string subject)
        {
            return Type.ToString() + " - " + subject;
        }
    }
}

