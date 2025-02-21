using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Reader
{
    public class TrainingMessageReader : IMessageReader
    {
        private IQuery _query;
        private ILogger _logger;
        private string _connection;
        private int _mILLISECONDS_PER_MINUTE;
        private List<SqlParameter> _parameters;

        public TrainingMessageReader(IQuery query, ILogger logger, string connection, int mILLISECONDS_PER_MINUTE, List<SqlParameter> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("message", nameof(connection));
            }
            this._query = query;
            this._logger = logger;
            this._connection = connection;
            this._mILLISECONDS_PER_MINUTE = mILLISECONDS_PER_MINUTE;
            this._parameters = parameters;
        }
        public async Task<IEnumerable<IMessage>> GetRecordsAsync()
        {
            try
            {
                var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (this._mILLISECONDS_PER_MINUTE * 5));
                var t = Task.Run(() => _query.GetRecords(_parameters));
                return await t.TimeoutAfter(timeout);
            }
            catch (Exception ex)
            {
                await this._logger.LogError(ex, "Failure in GetRecordsAsync");
                await this._logger.LogMessage(LoggingMethods.FormatExceptionMessage(ex), "Failure in GetRecordsAsync");
            }
            return null;
        }
    }
}
