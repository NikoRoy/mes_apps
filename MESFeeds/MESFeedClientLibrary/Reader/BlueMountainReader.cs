using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;

namespace MESFeedClientLibrary.Reader
{
    public class BlueMountainReader : IMessageReader
    {
        private IQuery _query;
        private ILogger _logger;
        private int _mILLISECONDS_PER_MINUTE = 60000;
        private List<SqlParameter> _parameters;

        public BlueMountainReader(IQuery query, ILogger logger, List<SqlParameter> parameters = null)
        {
            this._query = query;
            this._logger = logger;
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
                await this._logger.LogError(ex, "Error in GetAssetRecordsAsync()");                
            }
            return null;
        }
    }
}
