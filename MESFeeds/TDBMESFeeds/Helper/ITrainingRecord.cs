using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.DataAccess;
using TDBMESFeeds.Messages;

namespace TDBMESFeeds.Helper
{
    public interface ITrainingRecord 
    {
        string Query { get;  }

        string Employee { get; set; }
        string TrainingRecord { get; set; }
        string TrainingRecordRev { get; set; }
        string TrainingRecordStatus { get; set; } // maybe make enum?
        string ESignature { get; set; }
        DateTime LastModifiedDate { get; set; }
        int ID { get; set; }
        bool Sync { get; set; }

        int SyncAttempt { get; set; }
        List<ITrainingRecord> GetTrainingRecords(string connection, List<SqlParameter> p = null);
    }
}
