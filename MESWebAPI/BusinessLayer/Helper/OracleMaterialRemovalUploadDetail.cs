using OracleMESFeeds.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class OracleMaterialRemovalUploadDetail
    {
        private RemovalUpload removalUpload { get; set; }
        private ComponentRemoveDetails componentDetails => removalUpload.componentRemovalDetails;

        public string TransactionID => removalUpload.TransactionId;
        public string RouteStepName { get; private set; }
        public string ProductName { get; private set; }
        public string LotNumRemoveToInv { get; private set; }
        public string InvLocToRemoveTo { get; private set; }
        public float QtyToRemove { get; private set; }
        public string ManufacturingProcedure { get; private set; }
        public string LossReason { get; private set; }
        public string OrderNumber => removalUpload.WorkOrderName;

        public OracleMaterialRemovalUploadDetail(RemovalUpload ru)
        {
            removalUpload = ru;
        }
        private OracleMaterialRemovalUploadDetail(RemovalDetail rd)
        {
            RouteStepName = rd.RouteStepName;
            ProductName = rd.ProductName;
            LotNumRemoveToInv = rd.LotNumRemoveToInv;
            InvLocToRemoveTo = rd.InvLocToRemoveTo;
            QtyToRemove = rd.QtyToRemove;
            ManufacturingProcedure = rd.ManufacturingProcedure;
            LossReason = rd.LossReason;
        }
        public IEnumerable<OracleMaterialRemovalUploadDetail> GetDetails()
        {
            foreach (RemovalDetail item in componentDetails.Details)
            {
                yield return new OracleMaterialRemovalUploadDetail(item);
            }
        }
    }
}
