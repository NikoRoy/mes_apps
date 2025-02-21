using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;

using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace MESFeedClient.Classes
{
    public class OracleItemErrorHandler : OracleErrorHandler
    {
        public OracleItemErrorHandler(string log, IAlertHandler ah) : base(log, ah) { }

        public override async Task LogError(Exception ex, string message)
        {
            //if (message == null || message == "") { message = "Error"; }
            //string formattedMessage = $"{message} - {base.FormatExceptionMessage(ex)}";

            try
            {
                XDocument xmlDoc = XDocument.Parse(ex.Message);
                var isError = from x in xmlDoc.Root.Elements()
                              where x.Name.LocalName == "Contents"
                              select x.Value;

                var h = XElement.Parse(isError.SingleOrDefault());

                var j = h.Elements().Where(p => p.Name.LocalName == "Product").Elements().Where(m => m.Name.LocalName == "Name").SingleOrDefault();
                var k = h.Elements().Where(p => p.Name.LocalName == "ErrorDescription").SingleOrDefault();

                using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString))
                {
                    cn.Open();
                    using (OracleCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "xxatr_mes_extracts_pkg.xxatr_insert_mes_msg";
                        cmd.BindByName = true;
                        //cmd.Parameters.Add(new OracleParameter("@msgdatetime", a.Timestamp));
                        cmd.Parameters.Add(new OracleParameter("in_key_field", j.Value));
                        cmd.Parameters.Add(new OracleParameter("in_msg", k.Value));

                        //cmd.CommandText = string.Format("insert into XXATR_MES_NOTIF" +
                        //    " (MSG_DATE_TIME, KEY_FIELD, ERROR_MESSAGE)" +
                        //    " VALUES (to_timestamp('{0}', 'YYYY-MM-DD\"T\"HH24:MI:SS.ff7\"Z\"'), '{1}', '{2}') ",
                        //    a.Timestamp, a.Contents.Element("Product").Element("Name").Value, a.Contents.Element("ErrorDescription").Value.Replace("'", "''"));
                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
