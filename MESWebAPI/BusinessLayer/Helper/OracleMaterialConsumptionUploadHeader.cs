using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleMESFeeds.Uploads;

namespace BusinessLayer.Helper
{
    public class OracleMaterialConsumptionUploadHeader
    {
        private ConsumptionUpload consumptionUpload { get; set; }

        public string TransactionType => "ConsumptionUpload";
        public string TransactionID => consumptionUpload.TransactionId;
        public DateTime TransactionDate => DateTime.Parse(consumptionUpload.TransactionDateTime);
        public string WorkOrderName => consumptionUpload.WorkOrderName; 

        public OracleMaterialConsumptionUploadHeader (ConsumptionUpload cu)
        {
            consumptionUpload = cu;
        }
    }
}
