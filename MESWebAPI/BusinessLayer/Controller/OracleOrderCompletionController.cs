using BusinessLayer.Helper;
using MESFeedClientEFModel;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.Messages;
using OracleMESFeeds.Uploads;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static BusinessLayer.Helper.OracleTypeHelper;

namespace BusinessLayer.Controller
{
    internal class OracleOrderCompletionController : OracleController, IRecordController
    {
        public async Task<bool> Create(XElement x)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(base.Connection))
                {
                    try
                    {
                        cn.Open();
                        using (var transaction = cn.BeginTransaction())
                        {
                            try
                            {
                                //await LogOracleActivityAsync(x.ToString(), null);

                                var wo = DeserializeRequest(x);
                                
                                await LogOracleActivityAsync(x.ToString(), wo);

                                var header = OracleTypeHelper.GetHeaderFromWorkOrder(wo);
                                var hres = await IssueHeaderCommand(header, transaction);
                                var detail = OracleTypeHelper.GetDetailFromWorkOrder(wo);
                                var dres = await IssueDetailCommand(detail, transaction);
                                if (hres & dres)
                                {
                                    transaction.Commit();
                                    return true;
                                }
                                transaction.Rollback();
                                return false;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                await SqlErrorHandler.LogError(ex, "OCF-0003 - "+ x.ToString());
                            }
                            finally
                            {
                                cn.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await SqlErrorHandler.LogError(ex, "OCF-0002");
                    }
                }
            }
            catch (Exception ex)
            {
                await SqlErrorHandler.LogError(ex, "OCF-0001");
            }
            return false;
        }


        public void Delete()
        {
            throw new System.NotImplementedException();
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
                        var o = DeserializeRequest(x);

                        await LogOracleActivityAsync(x.ToString(), o);

                        cn.Open();

                        var res = await cmd.ExecuteNonQueryAsync();
                        if (res > 0)
                        {
                            return true;
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
        protected  async Task<bool> LogOracleActivityAsync(string request, OrderStepCompletion o)
        {
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                var dt = DateTime.Now;
                string id = string.Empty;
                if (o != null)
                {
                    dt = DateTime.Parse(o.TransactionDateTime);
                    id = o.TransactionId;
                }

                var w = new tblUploadFeedLog()
                {
                    Action = "MES Post Request",
                    RequestXml = request,
                    Response = null,

                    TransactionDate = dt,
                    TransactionID = id.ToString(),
                    TransactionType = OrderStepCompletion.TransactionType

                };
                context.tblUploadFeedLogs.Add(w);
                var i = await context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public OrderStepCompletion DeserializeRequest(XElement x)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OrderStepCompletion));
            using (var xml = x.CreateReader())
            {
                return (OrderStepCompletion)serializer.Deserialize(xml);
            }
        }

        private async Task<bool> IssueHeaderCommand(OrderStepCompletionHeader header, OracleTransaction tr)
        {
            try
            {
                string headersql = string.Format("insert into XXATR_MES_WO_HEADER " +
                    "(WIP_ENTITY_NAME, TRANSACTION_DATE, COMPLETED_QUANTITY, TRANSACTION_ID) " +
                    "Values ('{0}', to_date('{1}', 'DD/MM/YYYY HH24:MI:SS'),  '{2}', '{3}')"
                , header.MfgOrderName
                , header.TransactionDate.ToString("dd-MMM-yyyy hh:mm:ss")
                , header.ContainerQty
                , header.TransactionID
                , header.ContainerName
                , header.Factory
                , header.UOM
                , header.ContainerQty2
                , header.UOM2);

                OracleCommand cmd = new OracleCommand(headersql, tr.Connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Transaction = tr;
                if (await cmd.ExecuteNonQueryAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        } 
        private async Task<bool> IssueDetailCommand(OrderStepCompletionDetail detail, OracleTransaction tr)
        {
            try
            {
                foreach (var item in detail.GetDetails())
                {
                    string detailquery = string.Format("Insert into XXATR_MES_WO_DETAIL " +
                        "(WIP_ENTITY_NAME, TRANSACTION_DATE, SCRAP_PROCEDURE, SCRAP_CODE, QUANTITY, LOSS_UOM, QUANTITY_DEDUCTED, TRANSACTION_ID) " +
                        "Values ('{0}',to_date('{1}', 'DD/MM/YYYY HH24:MI:SS'),'{2}','{3}','{4}', '{5}', '{6}', '{7}') "
                        , detail.WipEntity
                        , item.TransactionDate.DateTime.ToString("dd-MMM-yyyy hh:mm:ss")
                        , item.ManufacturingProcedure
                        , item.LossReason
                        , item.LossQty
                        , item.LossQtyUOM
                        , item.QtyDeducted
                        , detail.TransactionID
                        );

                    using (OracleCommand cmd = new OracleCommand(detailquery))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = tr.Connection;
                        cmd.Transaction = tr;


                        var res = await cmd.ExecuteNonQueryAsync();
                        if (res == 0)
                        {
                            //await LogWorkOrderActivityAsync(x.ToString());
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public OracleOrderCompletionController(string cn) : base(cn)
        {
            this.AlertHandler = base.AlertHandler;
            this.Connection = base.Connection;
            this.SqlErrorHandler = base.SqlErrorHandler;
        }
        public OracleOrderCompletionController() { }
    }
}