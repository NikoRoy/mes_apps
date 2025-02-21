using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Net;
using TDBMESFeeds.Helper;
using TDBMESFeeds.Messages;

namespace TDBMESFeeds.DataAccess
{
    public class TrainingRecordCurrency : ITrainingRecord , IQuery
    {
        private const string _query = @"[MESFeedClient].[dbo].[spCheckCurrency]"; 

        public TrainingRecordType Type { get; private set; }
        public string Employee { get; set; }

        public string TrainingRecord { get; set; }

        public string TrainingRecordRev { get; set; }

        public string TrainingRecordStatus { get; set; }

        public DateTime LastModifiedDate { get ; set; }
        public int ID { get ; set ; }
        public bool Sync { get ; set ; }
        public int SyncAttempt { get; set; }
        public string Query => _query;

        public string ESignature { get ; set ; }

        public TrainingRecordCurrency()
        {
            Type = TrainingRecordType.TdbCur;
        }

        public TrainingRecordCurrency(string emp, string tr, string rev, string status, string esign, bool sync, DateTime lmd, int id, int attempt)
        {
            Type = TrainingRecordType.TdbCur;

            Employee = emp;
            TrainingRecord = tr;
            TrainingRecordRev = rev;
            TrainingRecordStatus = status;
            ESignature = esign;
            Sync = sync;
            LastModifiedDate = lmd;
            ID = id;
            SyncAttempt = attempt;
        }


        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                entity.Database.CommandTimeout = 240;
                return ConvertCheckExpirationResult(entity.spCheckCurrency());
            }
        }

        private static IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spCheckCurrency_Result> objectResult)
        {
            IList<IMessage> final = new List<IMessage>();
            foreach (var item in objectResult)
            {
                final.Add(new TrainingRecordDownload(item.UNumber, item.DocumentNumber, item.DocumentRevision, item.TrainingStatus, String.Empty, item.sync ?? false, item.LastModifiedDate, item.ID, item.SyncAttempt));
            }
            return final;
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
                                bool sync = sdr.GetBoolean(sdr.GetOrdinal("sync"));
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));
                                string unum = sdr.GetString(sdr.GetOrdinal("UNumber"));


                                ldoc.Add
                                (
                                    new TrainingRecordDownload(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
                                    //new TrainingRecordCurrency(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
                                );
                            }
                        }
                    }
                }
                return ldoc;
            }
        }

        public List<ITrainingRecord> GetTrainingRecords(string connection, List<SqlParameter> plist = null)
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
                                bool sync = sdr.GetBoolean(sdr.GetOrdinal("sync"));
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));
                                string unum = sdr.GetString(sdr.GetOrdinal("UNumber"));


                                ldoc.Add
                                (
                                   // new TrainingRecordDownload(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
                                    new TrainingRecordCurrency(unum, doc, rev, status, string.Empty, sync, lmd, id, attempt)
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
