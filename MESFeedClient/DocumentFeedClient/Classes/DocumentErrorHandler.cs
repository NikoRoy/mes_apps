using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFeedClient.Classes
{
    internal class DocumentErrorHandler : ErrorHandler
    {
        public DocumentErrorHandler(string logfolder, IAlertHandler ah) : base(logfolder, ah) { }
        public override string GetLogFile()
        {
            try
            {
                string logFileName = string.Format("Document Feed Client - {0}", DateTime.Today.ToString("yyyy-MM-dd"));
                string fullPath = Path.Combine(this.LogFolder, logFileName);
                return fullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override string FormatExceptionMessage(Exception ex)
        {
            return base.FormatExceptionMessage(ex);
        }
        public override Task LogError(Exception ex, string message)
        {
            return base.LogError(ex, message);
        }
    }
}
