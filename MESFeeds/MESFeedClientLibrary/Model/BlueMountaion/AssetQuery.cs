using MESFeedClientEFModel;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Model.BlueMountaion
{
    public class AssetQuery : IQuery
    {

        private const string Query = @"[MesFeedClient].[dbo].[spGetBlueMountainAssets]";


        private IEnumerable<IMessage> ConvertCheckExpirationResult(ObjectResult<spGetBlueMountainAssets_Result> objectResult)
        {
            List<AssetDownload> final = new List<AssetDownload>();
            foreach (var item in objectResult)
            {
                final.Add(new AssetDownload(item.AHAssetID, item.AHAssetDesc, item.AHStateName, item.AENextDueDate));
            }
            return final;
        }


        public IEnumerable<IMessage> GetDownloadRecords(string instanceCN, List<SqlParameter> p = null)
        {
            List<AssetDownload> ldoc = new List<AssetDownload>();

            using (SqlConnection cn = new SqlConnection(new SqlConnectionStringBuilder(instanceCN) { ConnectRetryCount = 3, ConnectRetryInterval = 10 }.ToString()))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(Query, cn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string eID = sdr.GetString(sdr.GetOrdinal("AHAssetID"));
                                string eDesc = sdr.GetString(sdr.GetOrdinal("AHAssetDesc"));
                                string eStatus = sdr.GetString(sdr.GetOrdinal("AHStateName"));
                                int ord = sdr.GetOrdinal("AENextDueDate");
                                DateTime? eNextDue = null;
                                if (!sdr.IsDBNull(ord))
                                {
                                    //Using a local datetime variable for the date because DateTime.TryParse doesn't like nullable dates
                                    DateTime lNextDue;
                                    DateTime.TryParse(sdr.GetString(ord), out lNextDue);
                                    eNextDue = lNextDue;
                                }
                                ldoc.Add
                                (
                                    new AssetDownload(eID, eDesc, eStatus, eNextDue)
                                );
                            }
                        }
                    }
                }
            }
            return ldoc;
        }

        public IEnumerable<IMessage> GetRecords(List<SqlParameter> pl = null)
        {
            using (var entity = EntityFactory.GenerateContext())
            {
                entity.Database.CommandTimeout = 120;
                return ConvertCheckExpirationResult(entity.spGetBlueMountainAssets());
            }
        }
    }

}
