using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Classes
{
    public class ErrorHandlerNoAlert : IErrorHandler
    {
        protected readonly string LogFolder;
        public ErrorHandlerNoAlert(string logfolder)
        {
            if (!Directory.Exists(logfolder)) { throw new Exception("Log Folder Not Found"); }
            LogFolder = logfolder;
        }
        public string GetLogFile()
        {
            string logFileName = string.Format("MESFeedLog-{0}", DateTime.Today.ToString("yyyy-MM-dd"));
            string fullPath = Path.Combine(this.LogFolder, logFileName);
            return fullPath;
        }

        public string FormatExceptionMessage(Exception ex)
        {
            string message = $"Message: {ex.Message} StackTrace: {ex.StackTrace}";
            if (ex.InnerException != null)
            {
                message = message + $"Inner Exception: {FormatExceptionMessage(ex.InnerException)}";
            }
            return message;
        }

        public async Task LogError(Exception ex, string message)
        {
            //Format message & exception
            if (message == null || message == "") { message = "Error"; }
            string formattedMessage = $"{message} - {FormatExceptionMessage(ex)}";

            //Log
            using (var sw = new StreamWriter(GetLogFile(), true))
            {
                await sw.WriteLineAsync(formattedMessage);
                sw.Flush();
            }
        }

    }
}
