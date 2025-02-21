using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientEFModel;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.Messages;

namespace DocumentFeedClient.Classes
{
    public class DocumentActivityLogger : ActivityLogger<DocumentDownload>
    {
        private readonly string ConnectionString;
        private readonly IErrorHandler ErrorHandler;
        
        public DocumentActivityLogger(string connectionString, IErrorHandler errorHandler)
            : base(connectionString, errorHandler)
        {
            this.ConnectionString = connectionString;
            this.ErrorHandler = errorHandler;
        }

        public override async Task LogActivity(DocumentDownload o, string action, string response)
        {
            using (var context = new MESFeedClientEntities())
            {
                try
                {
                    var i = new tblDocumentFeedLog()
                    {
                        Action = action,
                        TransactionDate = o.TransactionDateTime,
                        TransactionID = Convert.ToString(o.TransactionID),
                        TransactionType = DocumentDownload.TransactionType,
                        XmlRequest = o.ToXml(),
                        XmlResponse = response
                    };
                    context.tblDocumentFeedLogs.Add(i);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    await this.ErrorHandler.LogError(ex, "Error adding Document Activity");
                }
            }
        }
    }
}
