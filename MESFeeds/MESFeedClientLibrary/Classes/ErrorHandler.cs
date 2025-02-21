using System;
using System.IO;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Classes
{
    //Regular error handler to send exception via smtp and log to a file
    public class ErrorHandler : IErrorHandler
    {
        protected readonly string LogFolder;
        protected readonly IAlertHandler AlertHandler;
        public ErrorHandler(string logfolder, IAlertHandler ah)
        {
            if (!Directory.Exists(logfolder)) { throw new Exception("Log Folder Not Found"); }
            LogFolder = logfolder;
            AlertHandler = ah;
        }
        public virtual string GetLogFile()
        {
            string logFileName = string.Format("MESFeedLog-{0}", DateTime.Today.ToString("yyyy-MM-dd"));
            string fullPath = Path.Combine(this.LogFolder, logFileName);
            return fullPath;
        }

        public virtual string FormatExceptionMessage(Exception ex)
        {
            string message = $"Message: {ex.Message} StackTrace: {ex.StackTrace}";
            if (ex.InnerException != null)
            {
                message = message + $"Inner Exception: {FormatExceptionMessage(ex.InnerException)}";
            }
            return message;
        }

        public virtual async Task LogError(Exception ex, string message)
        {
            //Format message & exception
            if (message == null || message == "") { message = "Error"; }
            string formattedMessage = $"{message} - {FormatExceptionMessage(ex)}";

            //Log
            using (var sw = new StreamWriter(GetLogFile(), true))
            {
                await sw.WriteLineAsync(formattedMessage);
            }

            //Send alert
            if (this.AlertHandler != null)
            {
                await AlertHandler.SendAlert(formattedMessage, "Feed Error");
            }
        }
    }
}
