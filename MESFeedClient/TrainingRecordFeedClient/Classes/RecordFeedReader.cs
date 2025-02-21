using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDBMESFeeds.Helper;
using TDBMESFeeds.Messages;

namespace TrainingRecordFeedClient.Classes
{
    public class RecordFeedReader : IFeedReader<TrainingRecordDownload>
    {
        private readonly IErrorHandler ErrorHandler;
        private readonly string PrimaryConnection;
        private readonly string SecondaryConnection;
        private readonly int Milliseconds;
        private readonly int Attempts;

        public RecordFeedReader(IErrorHandler handler, string primary, int ms, string secondary, int retryattempt)
        {
            this.ErrorHandler = handler;
            this.PrimaryConnection = primary;
            this.SecondaryConnection = secondary;
            this.Milliseconds = ms;
            this.Attempts = retryattempt;
        }
        public async Task<IEnumerable<TrainingRecordDownload>> GetRecordsAsync()
        {
            try
            {
                Task<IEnumerable<TrainingRecordDownload>>[] tasks = new Task<IEnumerable<TrainingRecordDownload>>[]
                    {
                        //GetExpiringRecords(),
                        GetTDBTVRecords()
                        //GetCurrencyRecords()
                    };

                Task.WaitAll(tasks);
                return AggreateTasks(tasks);
            }
            catch(AggregateException e)
            {
                await ErrorHandler.LogError(e, "Task aggregation exception");
                return null;
            }
            catch (Exception ex)
            {
                await ErrorHandler.LogError(ex, "Error in GetRecordsAsync()");
                return null;
            }
            
        }
        
        private async Task<IEnumerable<TrainingRecordDownload>> GetCurrencyRecords()
        {
            try
            {

                var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * this.Milliseconds);
                var t = Task.Factory.StartNew(() => TrainingRecordBuilder.GetTrainingRecordDownloads(SecondaryConnection, TrainingRecordType.TdbCur));
                return await t.TimeoutAfter(timeout);
                //return t;//TransposeStatus( t);
            }
            catch (OperationCanceledException ex)
            {
                await ErrorHandler.LogError(ex, "OperationCanceledException - GetCurrencyRecords() Error");
                return null;
            }
            catch (TimeoutException ex)
            {
                await ErrorHandler.LogError(ex, "TimeoutException - GetCurrencyRecords() Error");
                return null;
            }
            catch (Exception ex)
            {
                await ErrorHandler.LogError(ex, "Exception - GetCurrencyRecords() Error");
                return null;
            }
            #region old cancellation 
            //var cts = new CancellationTokenSource();
            //CancellationToken ct = cts.Token;
            //var newTask = Task.Factory.StartNew(() =>
            //{
            //    cts.Token.ThrowIfCancellationRequested();
            //    bool moreToDo = true;
            //    List<TrainingRecordDownload> res = null;
            //    while (moreToDo)
            //    {
            //        // Poll on this property if you have to do
            //        // other cleanup before throwing.
            //        if (ct.IsCancellationRequested)
            //        {
            //            // Clean up here, then...
            //            ct.ThrowIfCancellationRequested();
            //        }
            //        res = TrainingRecordBuilder.GetTrainingRecordDownloads(SecondaryConnection, TrainingRecordType.OltpCurr);
            //        if (res != null)
            //        {
            //            moreToDo = false;
            //        }
            //    }
            //    return res;
            //}, cts.Token);

            //if (!newTask.Wait(120000, cts.Token))
            //{
            //    cts.Cancel();
            //}
            //try
            //{

            //    return await newTask;
            //    //var t =  Task.Factory.StartNew(() => TrainingRecordBuilder.GetTrainingRecordDownloads(SecondaryConnection, TrainingRecordType.OltpCurr));
            //    //return await t;// TransposeStatus( t);
            //}
            //catch (OperationCanceledException e)
            //{
            //    Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    await ErrorHandler.LogError(ex, "GetOLTPRecords() Error");
            //    return null;
            //}
            //finally
            //{
            //    cts.Dispose();
            //}
            #endregion
        }

        private async Task<IEnumerable<TrainingRecordDownload>> GetExpiringRecords()
        {
            try
            {
                var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * this.Milliseconds);
                var t =  Task.Factory.StartNew(() => TrainingRecordBuilder.GetTrainingRecordDownloads(SecondaryConnection ,TrainingRecordType.TdbExp));
                return await t.TimeoutAfter(timeout);
                //return t;//TransposeStatus( t);
            }
            catch (OperationCanceledException ex)
            {
                await ErrorHandler.LogError(ex, "OperationCanceledException - GetExpiringRecords() Error");
                return null;
            }
            catch (TimeoutException ex)
            {
                await ErrorHandler.LogError(ex, "TimeoutException - GetExpiringRecords() Error");
                return null;
            }
            catch (Exception ex)
            {
                await ErrorHandler.LogError(ex, "Exception - GetExpiringRecords() Error");
                return null;
            }           
        }
        private async Task<IEnumerable<TrainingRecordDownload>> GetTDBTVRecords()
        {

            try
            {
                var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * this.Milliseconds);
                var t = Task.Factory.StartNew(() => TrainingRecordBuilder.GetTrainingRecordDownloads(SecondaryConnection, TrainingRecordType.TdbAct, new List<SqlParameter>() { new SqlParameter("@attempts", this.Attempts) }));
                return await t.TimeoutAfter(timeout);
                //return t;//TransposeStatus( t);
            }
            catch (OperationCanceledException ex)
            {
                await ErrorHandler.LogError(ex, "OperationCanceledException - GetTDBTVRecords() Error");
                return null;
            }
            catch (TimeoutException ex)
            {
                await ErrorHandler.LogError(ex, "TimeoutException - GetTDBTVRecords() Error");
                return null;
            }
            catch (Exception ex)
            {
                await ErrorHandler.LogError(ex, "Exception - GetTDBTVRecords() Error");
                return null;
            }
        }
        private  IEnumerable<TrainingRecordDownload> AggreateTasks(Task<IEnumerable<TrainingRecordDownload>>[] tasks)
        {
            List<TrainingRecordDownload> list = new List<TrainingRecordDownload>();
            for (int i = 0; i < tasks.Count(); i++)
            {
                if (tasks[i].Result is null)
                    continue;
                var a = tasks[i].Result.Except(list);

                list.AddRange(a);
            }
            return list;
        }
    }
}
