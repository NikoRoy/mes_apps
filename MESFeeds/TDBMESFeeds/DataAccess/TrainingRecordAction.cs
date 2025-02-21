using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using TDBMESFeeds.Helper;
using TDBMESFeeds.Messages;

namespace TDBMESFeeds.DataAccess
{
    public class TrainingRecordAction : ITrainingRecord , IQuery
    {
        private const string _query = @"[MESFeedClient].[dbo].[spCheckLastActions]";
        public string Query => _query;

        public string Employee { get; set; }
        public string TrainingRecord { get; set; }
        public string TrainingRecordRev { get; set; }
        public string TrainingRecordStatus { get; set; }
        public bool Sync { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int ID { get; set; }
        public string ESignature { get ; set; }
        public int SyncAttempt { get; set; }


        public TrainingRecordAction() { }
        public TrainingRecordAction(string emp, string tr, string rev, string status, string ESign, bool sync, DateTime lmd, int id, int attempt)
        {
            Employee = emp;
            TrainingRecord = tr;
            TrainingRecordRev = rev;
            TrainingRecordStatus = status;
            ESignature = ESign;
            Sync = sync;
            LastModifiedDate = lmd;
            ID = id;
            SyncAttempt = attempt;
        }

        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = new MESFeedClientEntities())
            {
                int? a = 3;
                if (pl != null)
                {
                    var attempt = pl.Where(n => n.ParameterName == "@attempts").FirstOrDefault();

                    if (attempt != null)
                    {
                        a = (int?)attempt.Value;
                    }
                }
                entity.Database.CommandTimeout = 240;
                return ConvertCheckExpirationResult(entity.spCheckLastActions(a));
            }
        }

        private static IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spCheckLastActions_Result> objectResult)
        {
            IList<IMessage> final = new List<IMessage>();
            foreach (var item in objectResult)
            {
                var email = item.EMAIL_ADDRESS;
                if (email.Contains("@"))
                {
                    UserPrincipal up = GetUserPrincipal(email);
                    email = up.SamAccountName;
                }
                final.Add(new TrainingRecordDownload(email, item.DocumentNumber, item.DocumentRevision, item.TrainingStatus, null, item.sync, item.LastModifiedDate ?? default(DateTime), item.ID, item.SyncAttempt));
            }
            return final;
        }

        public IEnumerable<IMessage> GetDownloadRecords(string connection, List<SqlParameter> plist = null)
        {
            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(connection) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                List<TrainingRecordDownload> ldoc = new List<TrainingRecordDownload>();
                cn.Open();
                
                using (SqlCommand cmd = new SqlCommand(Query, cn) { CommandTimeout = 240, CommandType = System.Data.CommandType.StoredProcedure })
                {
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

                                string email = sdr.GetString(sdr.GetOrdinal("EMAIL_ADDRESS"));
                                if (email.Contains("@"))
                                {
                                    UserPrincipal up = GetUserPrincipal(email);
                                    email = up.SamAccountName;
                                }

                                string doc = sdr.GetString(sdr.GetOrdinal("DocumentNumber"));
                                string rev = sdr.GetString(sdr.GetOrdinal("DocumentRevision"));
                                string status = sdr.GetString(sdr.GetOrdinal("TrainingStatus"));
                                string esig = "All eSig";
                                if (status != "Training")
                                    esig = string.Empty;
                                bool sync = sdr.GetBoolean(sdr.GetOrdinal("Sync"));
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));

                                ldoc.Add
                                (
                                    new TrainingRecordDownload(email, doc, rev, status, esig, sync, lmd, id, attempt)
                                    //new TrainingRecordAction(email, doc, rev, status, esig, sync, lmd, id, attempt)
                                );
                            }
                        }
                    }

                }
                return ldoc;
            }
        }

        private static UserPrincipal GetUserPrincipal(string email)
        {
            //Create a user serach prototype in the current domain
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = new UserPrincipal(ctx);

            user.EmailAddress = email;

            //Perform the search
            PrincipalSearcher searcher = new PrincipalSearcher(user);
            var results = searcher.FindAll();

            //Only return exact matches - there might be more than one John Smith
            if (results.Count() == 1)
            {
                return (UserPrincipal)results.First();
            }

            return null;
        }

        public List<ITrainingRecord> GetTrainingRecords(string connection, List<SqlParameter> plist = null)
        {
            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(connection) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                List<ITrainingRecord> ldoc = new List<ITrainingRecord>();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(Query, cn) { CommandTimeout = 240, CommandType = System.Data.CommandType.StoredProcedure })
                {
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

                                string email = sdr.GetString(sdr.GetOrdinal("EMAIL_ADDRESS"));
                                if (email.Contains("@"))
                                {
                                    UserPrincipal up = GetUserPrincipal(email);
                                    email = up.SamAccountName;
                                }

                                string doc = sdr.GetString(sdr.GetOrdinal("DocumentNumber"));
                                string rev = sdr.GetString(sdr.GetOrdinal("DocumentRevision"));
                                string status = sdr.GetString(sdr.GetOrdinal("TrainingStatus"));
                                string esig = "All eSig";
                                if (status != "Training")
                                    esig = string.Empty;
                                bool sync = sdr.GetBoolean(sdr.GetOrdinal("Sync"));
                                DateTime lmd = sdr.GetDateTime(sdr.GetOrdinal("LastModifiedDate"));
                                int id = sdr.GetInt32(sdr.GetOrdinal("ID"));
                                int attempt = sdr.GetInt32(sdr.GetOrdinal("SyncAttempt"));

                                ldoc.Add
                                (
                                    //new TrainingRecordDownload(email, doc, rev, status, esig, sync, lmd, id, attempt)
                                    new TrainingRecordAction(email, doc, rev, status, esig, sync, lmd, id, attempt)
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
