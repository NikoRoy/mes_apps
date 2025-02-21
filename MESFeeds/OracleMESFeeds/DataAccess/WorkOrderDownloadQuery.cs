using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.Messages;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Types;

namespace OracleMESFeeds.DataAccess
{
    public class WorkOrderDownloadQuery
    {
        private const string Query =
            @"XXATR_MES_EXTRACTS_PKG.XXATR_EXTRACT_WO";

        public static IList<WorkOrder> GetWorkOrders()
        {
            IDictionary<string, WorkOrder> workOrderDict = new Dictionary<string, WorkOrder>();

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(Query, cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    OracleParameter p1 = new OracleParameter("out_data", OracleDbType.RefCursor);
                    p1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p1);

                    cn.Open();

                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Read Work Order header data                            
                            string dMfgOrderName = DataHelper.DataReaderGetString(dr, "WO_id");

                            //If WO header already exists, don't duplicate
                            if (!workOrderDict.ContainsKey(dMfgOrderName))
                            {
                                string dDescription = null;
                                string dNotes = null;
                                string dOrderStatus = DataHelper.DataReaderGetString(dr, "Orderstatus");
                                string dOrderType = DataHelper.DataReaderGetString(dr, "OrderType");
                                string dFactory = DataHelper.DataReaderGetString(dr, "factory");
                                string dProductName = DataHelper.DataReaderGetString(dr, "ProductName");
                                string dProductRevision = "-";
                                decimal dQty = DataHelper.DataReaderGetDecimal(dr, "Qty");
                                string dUom = DataHelper.DataReaderGetString(dr, "UOM");
                                decimal dQty2 = DataHelper.DataReaderGetDecimal(dr, "Qty2");
                                string dUom2 = DataHelper.DataReaderGetString(dr, "UOM2");
                                string dPriority = DataHelper.DataReaderGetString(dr, "priority");
                                DateTimeOffset? dReleaseDate = DataHelper.DataReaderGetDateTime(dr, "ReleasedDate");
                                string dCompletionSubInventory = DataHelper.DataReaderGetString(dr, "completion_subinventory");
                                DateTimeOffset? dPlannedStartDate = DataHelper.DataReaderGetDateTime(dr, "PlannedStartDate");
                                DateTimeOffset? dPlannedCompletionDate = DataHelper.DataReaderGetDateTime(dr, "scheduled_completion_date");
                                string dLotNumber = DataHelper.DataReaderGetString(dr, "LotNumber");

                                List<Messages.Attribute> dAttributes = null;

                                workOrderDict.Add(
                                    dMfgOrderName,
                                    new WorkOrder(
                                        mfgOrderName: dMfgOrderName,
                                        description: dDescription,
                                        notes: dNotes,
                                        orderStatus: dOrderStatus,
                                        orderType: dOrderType,
                                        factory: dFactory,
                                        products: Product.GetSingleProductList(dProductName, dProductRevision),
                                        qty: dQty,
                                        uom: dUom,
                                        qty2: dQty2,
                                        uom2: dUom2,
                                        priority: dPriority,
                                        releaseDate: dReleaseDate,
                                        completionSubInventory: dCompletionSubInventory,
                                        plannedStartDate: dPlannedStartDate,
                                        plannedCompletionDate: dPlannedCompletionDate,
                                        lotNumber: dLotNumber,
                                        attributes: dAttributes
                                    )
                                );
                            }

                            //Read nested component data & add to appropriate workorder
                            string nMaterialProductName = DataHelper.DataReaderGetString(dr, "componentproductname");
                            string nMaterialProductRevision = "-";

                            int nIssueControl = DataHelper.DataReaderGetInt(dr, "issue_control");
                            string nDefaultStockPullLocation = DataHelper.DataReaderGetString(dr, "supply_subinventory");
                            string nErpOperation = DataHelper.DataReaderGetString(dr, "ERPOperation");
                            decimal nQtyRequired = DataHelper.DataReaderGetDecimal(dr, "qty_req_per_unit");
                            string nUom1 = DataHelper.DataReaderGetString(dr, "UOM1");

                            MaterialItem materialItem =
                                new MaterialItem(
                                    Product.GetSingleProductList(nMaterialProductName, nMaterialProductRevision),
                                    nIssueControl,
                                    nDefaultStockPullLocation,
                                    nErpOperation,
                                    nQtyRequired,
                                    nUom1
                                );

                            workOrderDict[dMfgOrderName].MaterialList[0].MaterialListS.Add(materialItem);                   
                        }
                    }
                }
            }

            return workOrderDict.Values.Distinct().ToList();
        }

    }
}