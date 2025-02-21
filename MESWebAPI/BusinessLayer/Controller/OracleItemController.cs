using MESFeedClientEFModel;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.Messages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BusinessLayer.Controller
{
    public class OracleItemController : OracleController, IRecordController
    {        
        public async Task<bool> Create(XElement x)
        {
            using (OracleConnection cn = new OracleConnection(Connection))
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(base.OraclePackage, cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        /*Add Command Parameters here*/
                        var item = DeserializeRequest(x);
                        
                        
                        cn.Open();


                        var res = await cmd.ExecuteNonQueryAsync();
                        if (res > 0)
                        {
                            
                            await LogWorkOrderActivityAsync(x.ToString());
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    await SqlErrorHandler.LogError(ex, "Error Creating Oracle Work Order");
                    return false;
                }
            }
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        public Item DeserializeRequest(XElement x)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Item));
            return (Item)serializer.Deserialize(x.CreateReader());
        }

        public void Read()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateAsync(string id, XElement x)
        {
            using (OracleConnection cn = new OracleConnection(Connection))
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand(this.OraclePackage, cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        /*Add Command Parameters here for meaningful update to oracle*/
                        OracleParameter p = new OracleParameter("Wip_entity_id", id);
                        cmd.Parameters.Add(p);
                        var item = DeserializeRequest(x);

                        cn.Open();

                        var res = await cmd.ExecuteNonQueryAsync();
                        if (res > 0)
                        {
                            return await LogWorkOrderActivityAsync(x.ToString());
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    await this.SqlErrorHandler.LogError(ex, "Error Updating Oracle Work Order");
                    return false;
                }
            }
        }
        protected  async Task<bool> LogWorkOrderActivityAsync(string request)
        {
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                try
                {
                    var w = new tblMEStoOracleItemLog()
                    {
                        Action = "MESPost",
                        //ID = 1,
                        //TransactionDate = TransactionDateTime,
                        //TransactionID = wo.TransactionId.ToString(),
                        TransactionType = Item.TransactionType,
                        XmlRequest = request,
                        XmlResponse = ""
                    };
                    context.tblMEStoOracleItemLogs.Add(w);
                    var i = await context.SaveChangesAsync();
                    if (i > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    await this.SqlErrorHandler.LogError(ex, "Error Logging MES Item");
                    return false;
                }
            }
        }
        public OracleItemController(string cn): base(cn) { }
        public OracleItemController() { }
    }
}