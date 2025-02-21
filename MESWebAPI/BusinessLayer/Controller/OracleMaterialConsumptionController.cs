using BusinessLayer.Helper;
using MESFeedClientEFModel;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.Messages;
using OracleMESFeeds.Uploads;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BusinessLayer.Controller
{
    public class OracleMaterialConsumptionController : OracleController, IRecordController 
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

                                var i = DeserializeRequest(x);

                                await LogOracleActivityAsync(x.ToString(), i);

                                var header = OracleTypeHelper.GetHeaderFromMaterialConsumption(i);
                                var hres = await IssueHeaderCommand(header, transaction);
                                var detail = OracleTypeHelper.GetDetailFromMaterialConsumption(i);
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
                                await SqlErrorHandler.LogError(ex, "MCF-0003 - "+x.ToString());
                            }
                            finally
                            {
                                cn.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await SqlErrorHandler.LogError(ex, "MCF-0002");
                    }
                }

            }
            catch (Exception ex)
            {
                await SqlErrorHandler.LogError(ex, "MCF-0001");
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
                        var cu = DeserializeRequest(x);

                        cn.Open();

                        var res = await cmd.ExecuteNonQueryAsync();
                        if (res > 0)
                        {
                            return await LogOracleActivityAsync(x.ToString(), cu);
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
        protected  async Task<bool> LogOracleActivityAsync(string request, ConsumptionUpload d )
        {
            using (var context = new MESFeedClientEFModel.MESFeedClientEntities())
            {
                var dt = DateTime.Now;
                string id = string.Empty;
                if (d != null)
                {
                    dt = DateTime.Parse(d.TransactionDateTime);
                    id = d.TransactionId;
                }
                var w = new tblUploadFeedLog()
                {
                    Action = "MES Post Request",
                    RequestXml = request,
                    Response = null,
                    TransactionDate = dt,
                    TransactionID = id.ToString(),
                    TransactionType = ConsumptionUpload.TransactionType

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
        private async Task<bool> IssueHeaderCommand(OracleMaterialConsumptionUploadHeader header, OracleTransaction tr)
        {
            try
            {
                string headersql = string.Format("insert into XXATR_MES_MC_HEADER" +
                    " (TRANSACTION_ID, TRANSACTION_DATE, MFG_ORDER_NAME)" +
                    " values ('{0}',to_date('{1}', 'DD/MM/YYYY HH24:MI:SS'), '{2}')"
                , header.TransactionID
                , header.TransactionDate.ToString("dd-MMM-yyyy hh:mm:ss")
                , header.WorkOrderName
                , header.TransactionType
                );

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

        private async Task<bool> IssueDetailCommand(OracleMaterialConsumptionUploadDetail detail, OracleTransaction tr)
        {
            try
            {
                foreach (var item in detail.GetDetails())
                {
                    string detailquery = string.Format("Insert into XXATR_MES_MC_DETAIL " +
                        " (TRANSACTION_ID, ROUTE_STEP, PRODUCT_NAME, CONSUMED_QTY, UOM, FROM_CONTAINER, ISSUE_DIFFERENCE_PROCEDURE, ISSUE_DIFFERENCE_REASON, FROM_STOCK_POINT, ORDER_NUMBER)" +
                        " Values('{0}','{1}','{2}','{3}','{4}', '{5}', '{6}', '{7}', '{8}', '{9}')"
                        , detail.TransactionID
                        , item.RouteStepName
                        , item.ProductName
                        , item.ConsumedQty
                        , item.UOM
                        , item.FromContainer
                        , item.ManufacturingProcedure
                        , item.LossReason
                        , item.FromStockPoint
                        , detail.OrderNumber
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

        public ConsumptionUpload DeserializeRequest(XElement x)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConsumptionUpload));
            return (ConsumptionUpload)serializer.Deserialize(x.CreateReader());
        }

        public OracleMaterialConsumptionController(string cn): base(cn)
        {
            this.AlertHandler = base.AlertHandler;
            this.Connection = base.Connection;
            this.SqlErrorHandler = base.SqlErrorHandler;
        }
        public OracleMaterialConsumptionController() { }

    }
}