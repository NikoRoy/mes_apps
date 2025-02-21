using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Logger
{
    public class Logger : ILogger
    {
        //Message Delegate
        private Func<string, string, Task> WriteMessage;
        public void AttachMessageWriter(IAlertHandler ah) => this.WriteMessage += ah.SendAlert;
        public void DetachMessageWriter(IAlertHandler ah) => this.WriteMessage -= ah.SendAlert;

        public async Task LogMessage(string msg, string subject)
        {
            await this.WriteMessage(msg, subject);
        }


        //Activity Delegate
        public Func<IMessage, string, string, Task> WriteActivity;
        public void AttachActivityWriter(IActivity ftb) => WriteActivity += ftb.LogActivity;
        public void DetachActivityWriter(IActivity ftb) => WriteActivity -= ftb.LogActivity;

        public async Task LogActivity(IMessage obj, string action, string response)
        {
            await WriteActivity(obj, action, response);
        }


        //Error Delegate
        public Func<Exception,string, Task> WriteError;
        public void AttachErrorWriter(IErrorHandler eh) => WriteError += eh.LogError;
        public void DetachErrorWriter(IErrorHandler eh) => WriteError -= eh.LogError;

        public async Task LogError(Exception ex, string msg)
        {
            await WriteError(ex, msg);
        }



    }
}
