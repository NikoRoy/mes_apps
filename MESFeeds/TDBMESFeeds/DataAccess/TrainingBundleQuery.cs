using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using MESFeedClientEFModel;
using MESFeedClientLibrary.Interfaces;
using TDBMESFeeds.Messages;

namespace TDBMESFeeds.DataAccess
{
    public class TrainingBundleQuery : IQuery
    {
        private const string Query = @"[MESFeedClient].[dbo].[spBundleDailyChangesGet]";

        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                
                entity.Database.CommandTimeout = 240;
                return ConvertCheckExpirationResult(entity.spBundleDailyChangesGet());
            }
        }

        private static IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spBundleDailyChangesGet_Result> objectResult)
        {
            List<TrainingBundleDownload> result = new List<TrainingBundleDownload>();
            
            foreach (var item in objectResult)
            {

                var a = result.Select(i => i.TrainingRequirementGroup).ToList();
                var b = a
                        .Where(i => i.TrainingRequirementGroupName == item.TrainingRequirementGroupName)
                        .Where(j => j.TrainingRequirementGroupDescription == item.Description)
                        .SingleOrDefault();

                if (b is null)
                {
                    var bundle = new TrainingBundleDownload(item.TrainingRequirementGroupName, item.Description);
                    var req = new TrainingRequirement(item.Document, item.DocCurrentRev);
                    bundle.TrainingRequirementGroup.TrainingRequirements.Add(req);
                    result.Add(bundle);
                }
                else
                {
                    b.TrainingRequirements.Add(new TrainingRequirement(item.Document, item.DocCurrentRev));
                }
            }
            
            return result;
        }


        public static List<TrainingBundleDownload> GetBundleDownloads(string instanceCN)
        {
            List<TrainingBundleDownload> ldoc = new List<TrainingBundleDownload>();

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
                                //bundle name
                                string groupname = sdr.GetString(sdr.GetOrdinal("TrainingRequirementGroupName"));
                                //bundle desc
                                string groupDesc = sdr.GetString(sdr.GetOrdinal("Description"));
                                //bundle doc
                                string dDoc = sdr.GetString(sdr.GetOrdinal("Document"));
                                string dDocCurrentRev = sdr.GetString(sdr.GetOrdinal("DocCurrentRev"));

                                ///defend against duplicate groups
                                var trgList = ldoc.Select(i => i.TrainingRequirementGroup);
                                var trg = trgList
                                        .Where(i => i.TrainingRequirementGroupName == groupname)
                                        .Where(j => j.TrainingRequirementGroupDescription == groupDesc)
                                        .SingleOrDefault();
                                if (trg is null)
                                {
                                    var bundle = new TrainingBundleDownload(groupname, groupDesc);
                                    //var req = new TrainingRequirement(dDoc, dDocCurrentRev);
                                    bundle.TrainingRequirementGroup.TrainingRequirements.Add(new TrainingRequirement(dDoc, dDocCurrentRev));
                                    ldoc.Add(bundle);
                                }
                                else
                                {
                                    ///defence against duplicate req's is covered by distinct in stored procedure
                                    trg.TrainingRequirements.Add(new TrainingRequirement(dDoc, dDocCurrentRev));
                                }
                            }
                        }
                    }
                }
            }
            return ldoc;
        }

        public IEnumerable<IMessage> GetDownloadRecords(string instanceCN, List<SqlParameter> p = null)
        {
            List<TrainingBundleDownload> ldoc = new List<TrainingBundleDownload>();

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
                                //bundle name
                                string groupname = sdr.GetString(sdr.GetOrdinal("TrainingRequirementGroupName"));
                                //bundle desc
                                string groupDesc = sdr.GetString(sdr.GetOrdinal("Description"));
                                //bundle doc
                                string dDoc = sdr.GetString(sdr.GetOrdinal("Document"));
                                string dDocCurrentRev = sdr.GetString(sdr.GetOrdinal("DocCurrentRev"));

                                ///defend against duplicate groups
                                var trgList = ldoc.Select(i => i.TrainingRequirementGroup);
                                var trg = trgList
                                        .Where(i => i.TrainingRequirementGroupName == groupname)
                                        .Where(j => j.TrainingRequirementGroupDescription == groupDesc)
                                        .SingleOrDefault();
                                if (trg is null)
                                {
                                    var bundle = new TrainingBundleDownload(groupname, groupDesc);
                                    //var req = new TrainingRequirement(dDoc, dDocCurrentRev);
                                    bundle.TrainingRequirementGroup.TrainingRequirements.Add(new TrainingRequirement(dDoc, dDocCurrentRev));
                                    ldoc.Add(bundle);
                                }
                                else
                                {
                                    ///defence against duplicate req's is covered by distinct in stored procedure
                                    trg.TrainingRequirements.Add(new TrainingRequirement(dDoc, dDocCurrentRev));
                                }
                            }
                        }
                    }
                }
            }
            return ldoc;
        }
    }
}
