using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleMESFeeds.DataAccess
{
    public enum OracleRecordType
    {
        WorkOrder,
        Item,
        Inventory,
        ProductDownload,
        OrderStepCompletion,
        RemovalUpload,
        ConsumptionUpload

    }
    public class DataHelper
    {
        public static string DataReaderGetString(OracleDataReader dr, string columnName, string defaultVal = null)
        {
            int iOrd = dr.GetOrdinal(columnName);
            string result = defaultVal;
            if (!dr.IsDBNull(iOrd))
            {
                result = dr.GetString(iOrd);
            }

            return result;
        }

        public static DateTime? DataReaderGetDateTime(OracleDataReader dr, string columnName, DateTime? defaultVal = null)
        {
            int iOrd = dr.GetOrdinal(columnName);
            DateTime? result = defaultVal;
            if (!dr.IsDBNull(iOrd))
            {
                result = dr.GetDateTime(iOrd);
            }

            return result;
        }

        public static Decimal DataReaderGetDecimal(OracleDataReader dr, string columnName, decimal defaultVal = 0)
        {
            try
            {
                int iOrd = dr.GetOrdinal(columnName);
                decimal result = defaultVal;
                if (!dr.IsDBNull(iOrd))
                {
                    result = dr.GetDecimal(iOrd);
                }

                return result;
            }
            catch(InvalidCastException ex)
            {
                try
                {
                    return DataReaderGetOracleDecimal(dr, columnName, defaultVal);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public static decimal DataReaderGetOracleDecimal(OracleDataReader dr, string columnName, decimal defaultVal = 0)
        {
            int iOrd = dr.GetOrdinal(columnName);
            decimal result = defaultVal;
            if (!dr.IsDBNull(iOrd))
            {
                result = decimal.Parse( dr.GetString(iOrd).Substring(0, 20));
            }

            return result;
        }

        public static int DataReaderGetInt(OracleDataReader dr, string columnName, int defaultVal = 0)
        {
            int iOrd = dr.GetOrdinal(columnName);
            int result = defaultVal;
            if (!dr.IsDBNull(iOrd))
            {
                result = dr.GetInt32(iOrd);
            }

            return result;
        }
    }
}
