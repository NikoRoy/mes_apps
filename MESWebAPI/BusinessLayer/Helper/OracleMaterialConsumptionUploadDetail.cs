using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleMESFeeds.Uploads;

namespace BusinessLayer.Helper
{
    public class OracleMaterialConsumptionUploadDetail
    {
        private ConsumptionUpload consumptionUpload { get; set; }
        private ComponentIssueDetails componentIssueDetails => consumptionUpload.componentIssueDetails;

        public string TransactionID => consumptionUpload.TransactionId;
        public string RouteStepName { get; private set; }
        public string ProductName { get; private set; }
        public float ConsumedQty { get; private set; }
        public string UOM { get; private set; }
        public string FromContainer { get; private set; }
        public string IssueDifferenceReason { get; private set; }
        public string FromStockPoint { get; private set; }
        public string ManufacturingProcedure { get; private set; }
        public string LossReason { get; private set; }
        public string OrderNumber => consumptionUpload.WorkOrderName;
        public OracleMaterialConsumptionUploadDetail(ConsumptionUpload cu)
        {
            consumptionUpload = cu;
        }
        private OracleMaterialConsumptionUploadDetail (IssueDetail id)
        {
            RouteStepName = id.RouteStepName;
            ProductName = id.ProductName;
            ConsumedQty = id.ConsumedQty;
            UOM = id.UOM;
            FromContainer = id.FromContainer;
            IssueDifferenceReason = id.IssueDifferenceReason;
            FromStockPoint = id.FromStockPoint;
            ManufacturingProcedure = id.ManufacturingProcedure;
            LossReason = id.LossReason;
        }
        public IEnumerable<OracleMaterialConsumptionUploadDetail> GetDetails()
        {
            foreach (IssueDetail item in componentIssueDetails.Details)
            {
                yield return new OracleMaterialConsumptionUploadDetail(item);
            }
        }
    }
}
