using MESFeedClientEFModel;
using MESFeedClientLibrary;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace TrainingRecordFeedClient.Classes
{
    public class RecordXmlUpdater : IUpdater<TrainingRecordDownload>
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<TrainingRecordDownload> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;

        public RecordXmlUpdater(IXmlProcessor processor, IActivityLogger<TrainingRecordDownload> logger, IErrorHandler handler) 
        {
            this.Processor = processor;
            this.ActivityLogger = logger;
            this.ErrorHandler = handler;
        }

        public async Task UpdateAsync(IEnumerable<TrainingRecordDownload> objlist, DateTime date = default(DateTime))
        {
            if (objlist.Count() <= 0) { return; }

            foreach (TrainingRecordDownload rec in objlist)
            {
                try
                {
                    Console.WriteLine(rec.ToXml());
                    var response = await Processor.Execute(rec.ToXml());
                    Console.WriteLine(response);
                    await ActivityLogger.LogActivity(rec, "Post Request", response);
                    if (!WasResponseSuccessful(response))
                    {
                        await SyncBackTrainingRecord(rec, false);
                        if (UNumberException.IsUNumberException(response))
                            throw new UNumberException(response);
                        await ErrorHandler.LogError(new Exception(response), "Training Record Feed Was Unsuccessful");
                    }
                    else
                    {
                        await SyncBackTrainingRecord(rec, true);
                    }
                }
                catch (UNumberException)
                {
                    //ignore u number not found. 
                }
                catch (Exception ex)
                {
                    //await Logger.LogError(ex, "Error Processing MES Training Record");
                    await ErrorHandler.LogError(ex, "Error Processing MES Training Record");
                }
            }
        }
        public bool WasResponseSuccessful(string response)
        {
            return !response.Contains("__errorDescription") || !response.Contains("__errorCode") || !response.Contains("__failureContext") || !response.Contains("__exceptionData");
        }

        private async Task SyncBackTrainingRecord(TrainingRecordDownload obj, bool sync)
        {
            if (obj.ID == 0)
                return;
            using (var context = new MESFeedClientEntities())
            {
                try
                {
                    if(sync == true)
                        context.spLatestMESAction_Update(obj.ID, true, obj.SyncAttempt);
                    else
                        context.spLatestMESAction_Update(obj.ID, false, obj.SyncAttempt + 1);

                }
                catch (Exception ex)
                {
                    await ErrorHandler.LogError(ex, "Error adding Training Record Activity");
                }
            }
        }

        tblRecordFeedLog GenerateActivity(TrainingRecordDownload rec, string action, string response)
        {
            return new tblRecordFeedLog()
            {
                Action = action,
                TransactionDate = rec.TransactionDateTime,
                TransactionID = Convert.ToString(rec.TransactionID),
                TransactionType = TrainingRecordDownload.TransactionType,
                XmlRequest = rec.ToXml(),
                XmlResponse = response
            };
        }

    }
}
