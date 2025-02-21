using OracleMESFeeds.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class OrderStepCompletionDetail
    {
        private OrderStepCompletion orderStepCompletion { get; set; }
        private ChangeQtyDetails ChangeQtyDetails => orderStepCompletion.changeQtyDetails;
        public string TransactionID => orderStepCompletion.TransactionId;
        public string WipEntity => orderStepCompletion.MfgOrderName;
        public DateTimeOffset TransactionDate { get; private set; }
        public float LossQty { get; private set; }
        public string LossQtyUOM { get; private set; }
        public string LossReason { get; private set; }
        public string ManufacturingProcedure { get; set; }
        public string QtyDeducted { get; set; }

        public OrderStepCompletionDetail(OrderStepCompletion osc)
        {
            orderStepCompletion = osc;
        }

        private OrderStepCompletionDetail(QtyDetail d)
        {
            TransactionDate = d.TransactionDateTime;
            LossQty = d.LossQty;
            LossQtyUOM = d.LossQtyUOM;
            LossReason = d.LossReason;
            ManufacturingProcedure = d.ManufacturingProcedure;
            QtyDeducted = d.QtyDeducted;
        }

        public IEnumerable<OrderStepCompletionDetail> GetDetails()
        {
            foreach (QtyDetail item in ChangeQtyDetails.Details)
            {
                yield return new OrderStepCompletionDetail(item);
            }
        }
    }

}
