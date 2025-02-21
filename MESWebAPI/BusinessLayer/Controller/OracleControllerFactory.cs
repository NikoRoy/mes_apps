using OracleMESFeeds.DataAccess;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Controller
{
    public class OracleControllerFactory
    {
        public static IRecordController DeterminerController(OracleRecordType ort)
        {
            switch (ort)
            {
                case OracleRecordType.OrderStepCompletion:
                    return new OracleOrderCompletionController();
                case OracleRecordType.RemovalUpload:
                    return new OracleMaterialRemovalController();
                case OracleRecordType.ConsumptionUpload:
                    return new OracleMaterialConsumptionController();
                case OracleRecordType.Inventory:
                    return new OracleInventoryController();
                case OracleRecordType.Item:
                    return new OracleItemController();
                case OracleRecordType.WorkOrder:
                    return new OracleWorkOrderController();
                case OracleRecordType.ProductDownload:
                    return new OracleProductController();
                default:
                    return null;
            }
        }
    }
}
