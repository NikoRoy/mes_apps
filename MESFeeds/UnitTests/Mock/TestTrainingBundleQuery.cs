using MESFeedClientLibrary.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;

namespace UnitTests.Mock
{
    public class TestTrainingBundleQuery
    {
        private const string Query = @"[MESFeedClient].[dbo].[spBundleDailyChangesGet]";

        public IEnumerable<IMessage> GetDownloadRecords(Mock<MESFeedClientEFModel.MESFeedClientEntities> moqEntities, List<SqlParameter> p = null)
        {
            List<TrainingBundleDownload> ldoc = new List<TrainingBundleDownload>();
            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder("") { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
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
