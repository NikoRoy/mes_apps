using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.Training.Messages;

namespace MESFeedClientLibrary.Model.Training.DataAccess
{
    public class DocumentQuery : IQuery
    {
        //convert to stored procedure
        private const string Query = @"[MESFeedClient].[dbo].[spDocDailyChangesGet]";

        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                entity.Database.CommandTimeout = 240;
                return ConvertCheckExpirationResult(entity.spDocDailyChangesGet());
            }
        }

        private static IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spDocDailyChangesGet_Result> objectResult)
        {
            IList<IMessage> final = new List<IMessage>();
            foreach (var item in objectResult)
            {
                final.Add(new DocumentDownload(item.DocID, item.DocCurrentRev, item.DocDesc, item.DocPath));
            }
            return final;
        }

        public static List<DocumentDownload> GetDocumentDownloads(string instanceCN)
        {
            List<DocumentDownload> ldoc = new List<DocumentDownload>();

            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(instanceCN) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(Query, cn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string dDocID = sdr.GetString(sdr.GetOrdinal("DocID"));
                                string dDocRev = sdr.GetString(sdr.GetOrdinal("DocCurrentRev"));
                                string dDocPath = sdr.GetString(sdr.GetOrdinal("DocPath")); ;
                                string dDocDesc = sdr.GetString(sdr.GetOrdinal("DocDesc"));

                                ldoc.Add
                                (
                                    new DocumentDownload(dDocID, dDocRev, dDocDesc, dDocPath)
                                );
                            }
                        }
                    }
                }
            }
            return ldoc;
        }

        public IEnumerable<IMessage> GetDownloadRecords(string instanceCN, List<SqlParameter> p = null)
        {
            List<DocumentDownload> ldoc = new List<DocumentDownload>();

            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(instanceCN) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(Query, cn))
                {
                    cmd.CommandTimeout = 240;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (p != null)
                    {
                        foreach (SqlParameter i in p)
                        {
                            cmd.Parameters.Add(i);
                        }
                    }
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string dDocID = sdr.GetString(sdr.GetOrdinal("DocID"));
                                string dDocRev = sdr.GetString(sdr.GetOrdinal("DocCurrentRev"));
                                string dDocPath = sdr.GetString(sdr.GetOrdinal("DocPath")); ;
                                string dDocDesc = sdr.GetString(sdr.GetOrdinal("DocDesc"));

                                ldoc.Add
                                (
                                    new DocumentDownload(dDocID, dDocRev, dDocDesc, dDocPath)
                                );
                            }
                        }
                    }
                }
            }
            return ldoc;
        }

    }
}
