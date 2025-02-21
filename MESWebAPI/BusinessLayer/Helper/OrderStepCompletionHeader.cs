using OracleMESFeeds.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class OrderStepCompletionHeader
    {
        private OrderStepCompletion orderStepCompletion { get; set; }
        public string TransactionID => orderStepCompletion.TransactionId;
        public DateTime TransactionDate => DateTime.Parse(orderStepCompletion.TransactionDateTime);
        public string ContainerName => orderStepCompletion.ContainerName;
        public string MfgOrderName => orderStepCompletion.MfgOrderName;
        public string Factory => orderStepCompletion.Factory;
        public float ContainerQty => orderStepCompletion.ContainerQty;
        public string UOM => orderStepCompletion.UOM;
        public float ContainerQty2 => orderStepCompletion.ContainerQty2;
        public string UOM2 => orderStepCompletion.UOM2;

        public OrderStepCompletionHeader(OrderStepCompletion osc)
        {
            orderStepCompletion = osc;
        }
    }
}
