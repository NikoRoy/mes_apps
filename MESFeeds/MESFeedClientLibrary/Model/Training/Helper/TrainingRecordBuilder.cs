using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Model.Training.Helper;
using MESFeedClientLibrary.Model.Training.Messages;

namespace MESFeedClientLibrary.Model.Training.Helper
{
    public static class TrainingRecordBuilder
    {
        // IEnumerable<IMessage
        public static List<TrainingRecordDownload> GetTrainingRecordDownloads(string cn, TrainingRecordType t, List<SqlParameter> p = null)
        {
            if (cn == null || cn == "")
                return null;

            var trh = new TrainingRecordHelper();
            var i = trh.CreateTR(t);

            //return i.GetDownloadRecords(cn, p);
            var l = i.GetTrainingRecords(cn, p);
            return GetDownloadFromInterface(l);

        }
        internal static List<TrainingRecordDownload> GetDownloadFromInterface(List<ITrainingRecord> l)
        {
            List<TrainingRecordDownload> dl = new List<TrainingRecordDownload>();
            foreach(ITrainingRecord tr in l)
            {
                dl.Add(new TrainingRecordDownload(
                            tr.Employee, tr.TrainingRecord, tr.TrainingRecordRev, tr.TrainingRecordStatus, tr.ESignature, tr.Sync, tr.LastModifiedDate, tr.ID, tr.SyncAttempt
                        )
                    );
            }
            return dl;
        }
    }
}
