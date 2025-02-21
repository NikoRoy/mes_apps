using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Reader
{
    public class MessageReader : IMessageReader
    {
        private readonly IQuery _query;
        private readonly ILogger _logger;
        private readonly string _connection;
        private readonly int _milSec;
        private readonly List<SqlParameter> _params;


        public MessageReader(IQuery query, ILogger logger, string connection, int milSec, List<SqlParameter> spl = null)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("message", nameof(connection));
            }

            this._query = query;
            this._logger = logger;
            this._connection = connection;
            this._milSec = milSec;
            this._params = spl;
        }
        public async Task<IEnumerable<IMessage>> GetRecordsAsync()
        {
            try
            {
                var timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * (this._milSec*5));
                var t = Task.Run(() => _query.GetDownloadRecords(_connection, this._params));
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
