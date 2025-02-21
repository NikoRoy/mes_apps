using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OracleMESFeeds.Messages;
using System.Configuration;
using System.Data;

namespace OracleMESFeeds.DataAccess
{
    public class InventoryDownloadQuery
    {
        //XXATR_MES_EXTRACTS_PKG.XXATR_EXTRACT_INVENTORY
        //XXATR_MES_EXTRACTS_PKG.xxatr_init_inv_load
        private const string Query =
            @"XXATR_MES_EXTRACTS_PKG.xxatr_init_inv_load"; 

        public static IList<InventoryDownload> GetOpenInventoryTransactions()
        {
            IList<InventoryDownload> inventoryDownload = new List<InventoryDownload>();

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
                            string dInventoryLotName = dr.GetString(dr.GetOrdinal("lot_number"));
                            string dProductName = dr.GetString(dr.GetOrdinal("Product_name"));
                            string dProductRevision = "-";
                            string dInventoryLocation = dr.GetString(dr.GetOrdinal("inventory_location"));
                            decimal dQty = dr.GetDecimal(dr.GetOrdinal("qty"));
                            string dQtyUom = dr.GetString(dr.GetOrdinal("qtyuom"));
                            int iExp = dr.GetOrdinal("expiration_date");
                            DateTime? dExpirationDate = null;
                            if (!dr.IsDBNull(iExp))
                            {
                                dExpirationDate = dr.GetDateTime(iExp);
                            }

                            inventoryDownload.Add
                            (
                                new InventoryDownload
                                (
                                    inventoryLotName: dInventoryLotName,
                                    productName: dProductName,
                                    productRevision: dProductRevision,
                                    inventoryLocation: dInventoryLocation,
                                    qty: dQty,
                                    qtyuom: dQtyUom,
                                    expirationDate: dExpirationDate
                                )
                            );
                        }
                    }
                    
                }
            }

            return inventoryDownload;
        }

    }
}
