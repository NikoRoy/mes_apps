using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;

namespace MESFeedClientLibrary.Reader
{
    public class NightlyMessageReader : IMessageReader
    {
        private IQuery _queryCur;
        private IQuery _queryExp;
        private ILogger _logger;
        private string _connection;
        private int _mILLISECONDS_PER_MINUTE;
        private List<SqlParameter> _parameters;

        public NightlyMessageReader(IQuery queryCur, IQuery queryExp, ILogger logger, string connection, int mILLISECONDS_PER_MINUTE, List<SqlParameter> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("message", nameof(connection));
            }
            this._queryCur = queryCur;
            this._queryExp = queryExp;
            this._logger = logger;
            this._connection = connection;
            this._mILLISECONDS_PER_MINUTE = mILLISECONDS_PER_MINUTE;
            this._parameters = parameters;
        }

        public async Task<IEnumerable<IMessage>> GetRecordsAsync()
        {
            try
            {

                Task<IEnumerable<IMessage>>[] tasks = new Task<IEnumerable<IMessage>>[]
                    {
                        //Task.Run(() => _queryCur.GetDownloadRecords(_connection, this._parameters)),
                        //Task.Run(() => _queryExp.GetDownloadRecords(_connection, this._parameters))
                        Task.Run(() => _queryCur.GetRecords(_parameters)),
                        Task.Run(() => _queryExp.GetRecords(_parameters))

                    };


                Task.WaitAll(tasks);
                return AggreateTasks(tasks);
            }
            catch (Exception ex)
            {
                await this._logger.LogError(ex, "Failure in GetRecordsAsync");
                await this._logger.LogMessage(LoggingMethods.FormatExceptionMessage(ex), "Failure in GetRecordsAsync");
            }
            return null;
        }
        private IEnumerable<IMessage> AggreateTasks(Task<IEnumerable<IMessage>>[] tasks)
        {
            List<IMessage> list = new List<IMessage>();
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
