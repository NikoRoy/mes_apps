using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OracleInventoryFeedClient.Classes
{
    public class OracleInventoryErrorHandler : OracleErrorHandler
    {

        public OracleInventoryErrorHandler(string log, IAlertHandler ah) : base(log, ah) {}

        public override  async Task LogError(Exception ex, string message)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString))
                {
                    XDocument xmlDoc = XDocument.Parse(ex.Message);
                    var isError = from x in xmlDoc.Root.Elements()
                                  where x.Name.LocalName == "Contents"
                                  select x.Value;

                    var h = XElement.Parse(isError.SingleOrDefault());

                    var j = h.Elements().Where(p => p.Name.LocalName == "InventoryLotName").SingleOrDefault();
                    var k = h.Elements().Where(p => p.Name.LocalName == "ErrorDescription").SingleOrDefault();

                    using (OracleCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "xxatr_mes_extracts_pkg.xxatr_insert_mes_msg";

                        //cmd.Parameters.Add(new OracleParameter("@msgdatetime", a.Timestamp));
                        cmd.Parameters.Add(new OracleParameter("in_key_field", j.Value));
                        cmd.Parameters.Add(new OracleParameter("in_msg", k.Value));

                        //cmd.CommandType = System.Data.CommandType.Text;
                        //cmd.CommandText = string.Format("insert into XXATR_MES_NOTIF" +
                        //    " (MSG_DATE_TIME, KEY_FIELD, ERROR_MESSAGE)" +
                        //    " VALUES (to_timestamp('{0}', 'YYYY-MM-DD\"T\"HH24:MI:SS.ff7\"Z\"'), '{1}', '{2}') ",
                        //    a.Timestamp, a.Contents.Element("InventoryLotName").Value, a.Contents.Element("ErrorDescription").Value.Replace("'", "''"));
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }

            //return base.LogError(ex, message);
        }

    }
}
