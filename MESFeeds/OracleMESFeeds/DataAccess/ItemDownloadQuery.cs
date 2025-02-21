using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

using System.Configuration;
using System.Data;
using MESFeedClientLibrary.Model.Oracle.Messages;
using Attribute = MESFeedClientLibrary.Model.Oracle.Messages.Attribute;

namespace DataAccess
{
    public class ItemDownloadQuery
    {
        private const string Query =
            @"XXATR_MES_EXTRACTS_PKG.XXATR_EXTRACT_ITEMS";

        private const string attributeQuery =
            @"XXATR_MES_EXTRACTS_PKG.XXATR_EXTRACT_ITEM_ATTRIBUTES";

        public static IList<Item> GetItemTransactions()
        {
            IList<Item> itemDownload = new List<Item>();

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
                            string dProductName = dr.GetString(dr.GetOrdinal("Product_name"));
                            string dProductRevision = "-";
                            string dDescription = dr.GetString(dr.GetOrdinal("productdescription"));
                            int dStatus = dr.GetInt32(dr.GetOrdinal("status"));
                            string dProductType = dr.GetString(dr.GetOrdinal("producttype"));
                            string dProductFamily = dr.GetString(dr.GetOrdinal("productfamily"));
                            int dStartQty = 0;
                            string dStartQtyUom = "EA";
                            List<Attribute> dAttributes = new List<Attribute>();

                            //Retrieve attributes
                            using (OracleCommand cmdAttr= new OracleCommand(attributeQuery, cn))
                            {
                                cmdAttr.CommandType = System.Data.CommandType.StoredProcedure;

                                OracleParameter p2 = new OracleParameter("item_number", OracleDbType.Varchar2);
                                p2.Value = dProductName;
                                cmdAttr.Parameters.Add(p2);

                                OracleParameter p3 = new OracleParameter("out_data", OracleDbType.RefCursor);
                                p3.Direction = ParameterDirection.Output;
                                cmdAttr.Parameters.Add(p3);

                                using (OracleDataReader drAttr = cmdAttr.ExecuteReader())
                                {
                                    while (drAttr.Read())
                                    {
                                        string attrName = drAttr.GetString(drAttr.GetOrdinal("NAME"));
                                        string attrDataType = drAttr.GetString(drAttr.GetOrdinal("DATA_TYPE"));
                                        string attrValue = drAttr.GetString(drAttr.GetOrdinal("ATTRIBUTE_VALUE"));
                                        string attrIsExpression = drAttr.GetString(drAttr.GetOrdinal("IS_EXPRESSION"));

                                        Attribute attr =
                                            new Attribute(
                                                attrName,
                                                attrDataType,
                                                attrValue,
                                                attrIsExpression
                                            );

                                        dAttributes.Add(attr);
                                    }
                                }
                            }

                            itemDownload.Add
                            (
                                new Item
                                (
                                    productName: dProductName,
                                    productRevision: dProductRevision,
                                    description: dDescription,
                                    status: dStatus,
                                    productType: dProductType,
                                    productFamily: dProductFamily,
                                    startQuantity: dStartQty,
                                    startQuantityUom: dStartQtyUom,
                                    attributes: dAttributes
                                )
                            );
                        }
                    }

                }
            }

            return itemDownload;
        }

    }

}