using OracleMESFeeds.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class OracleTypeHelper
    {
        //Order Completion
        public static OrderStepCompletionHeader GetHeaderFromWorkOrder(OrderStepCompletion osc)
        {
            return new OrderStepCompletionHeader(osc);
        }
        public static OrderStepCompletionDetail GetDetailFromWorkOrder(OrderStepCompletion osc)
        {
            return new OrderStepCompletionDetail(osc);
        }

        //Material Removal
        public static OracleMaterialRemovalUploadHeader GetHeaderFromMaterialRemoval(RemovalUpload ru)
        {
            return new OracleMaterialRemovalUploadHeader(ru);
        }
        public static OracleMaterialRemovalUploadDetail GetDetailFromMaterialRemoval(RemovalUpload ru)
        {
            return new OracleMaterialRemovalUploadDetail(ru);
        }


        //Material Consumption
        public static OracleMaterialConsumptionUploadHeader GetHeaderFromMaterialConsumption(ConsumptionUpload cu)
        {
            return new OracleMaterialConsumptionUploadHeader(cu);
        }
        public static OracleMaterialConsumptionUploadDetail GetDetailFromMaterialConsumption(ConsumptionUpload cu)
        {
            return new OracleMaterialConsumptionUploadDetail(cu);
        }
    }
}
