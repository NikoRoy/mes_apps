using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IQuery
    {
        IEnumerable<IMessage> GetDownloadRecords(string instanceCN, List<SqlParameter> p = null);

        IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null);
    }
}
