using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Training.Helper;
using MESFeedClientLibrary.Model.Training.Messages;

namespace MESFeedClientLibrary.Model.Training.DataAccess
{
    public class TrainingRecordExpiration : ITrainingRecord, IQuery
    {
        private const string _query = @"[MESFeedClient].[dbo].[spCheckExpiration]";
        public string Query => _query;

        public string Employee { get; set; }
        public string TrainingRecord { get; set; }
        public string TrainingRecordRev { get; set; }
        public string TrainingRecordStatus { get; set; }
        public DateTime LastModifiedDate { get ; set ; }
        public int ID { get ; set ; }
        public bool Sync { get ; set ; }
        public string ESignature { get ; set ; }
        public int SyncAttempt { get; set; }

        public TrainingRecordExpiration() { }
        public TrainingRecordExpiration(string emp, string tr, string rev, string status, string Esign, bool sync, DateTime lmd, int id, int attempt)
        {
            Employee = emp;
            TrainingRecord = tr;
            TrainingRecordRev = rev;
            TrainingRecordStatus = status;
            ESignature = Esign;
            Sync = sync;
            LastModifiedDate = lmd;
            ID = id;
            SyncAttempt = attempt;
        }

        public IEnumerable<IMessage> GetDownloadRecords(string connection, List<SqlParameter> plist = null)
        {
            
            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(connection) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                List<TrainingRecordDownload> ldoc = new List<TrainingRecordDownload>();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(Query, cn))
                {
                    cmd.CommandTimeout = 240;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (plist != null)
                    {
                        foreach (SqlParameter p in plist)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string emp = sdr.GetString(sdr.GetOrdinal("EmployeeNumber"));
                                string doc = sdr.GetString(sdr.GetOrdinal("DocumentNumber"));
                                string rev = sdr.GetString(sdr.GetOrdinal("DocumentRevision"));
                                string status = sdr.GetString(sdr.GetOrdinal("TrainingStatus"));
                                int a = sdr.GetOrdinal("sync");
                                bool sync = sdr.GetBoolean(a);
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));
                                string unum = sdr.GetString(sdr.GetOrdinal("UNumber"));
                                ldoc.Add
                                (
                                    new TrainingRecordDownload(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
                                );
                            }
                        }
                    }
                }
                return ldoc;
            }
        }

        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                entity.Database.CommandTimeout = 240;
                return ConvertCheckExpirationResult(entity.spCheckExpiration());
            }
        }

        private static IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spCheckExpiration_Result> objectResult)
        {
            IList<IMessage> final = new List<IMessage>();
            foreach (var item in objectResult)
            {
                final.Add(new TrainingRecordDownload(item.UNumber, item.DocumentNumber, item.DocumentRevision, item.TrainingStatus, String.Empty, item.sync ?? false , item.LastModifiedDate, item.ID, item.SyncAttempt));
            }
            return final;
        }

        public List<ITrainingRecord> GetTrainingRecords(string connection, List<SqlParameter> p = null)
        {
            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(connection) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                List<ITrainingRecord> ldoc = new List<ITrainingRecord>();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(Query, cn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (p != null)
                    {
                        foreach (SqlParameter pp in p)
                        {
                            cmd.Parameters.Add(pp);
                        }
                    }
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string emp = sdr.GetString(sdr.GetOrdinal("EmployeeNumber"));
                                string doc = sdr.GetString(sdr.GetOrdinal("DocumentNumber"));
                                string rev = sdr.GetString(sdr.GetOrdinal("DocumentRevision"));
                                string status = sdr.GetString(sdr.GetOrdinal("TrainingStatus"));
                                int a = sdr.GetOrdinal("sync");
                                bool sync = sdr.GetBoolean(a);
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));
                                string unum = sdr.GetString(sdr.GetOrdinal("UNumber"));
                                ldoc.Add
                                (
                                    new TrainingRecordExpiration(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
                                );
                            }
                        }
                    }
                }
                return ldoc;
            }
        }
    }
}
