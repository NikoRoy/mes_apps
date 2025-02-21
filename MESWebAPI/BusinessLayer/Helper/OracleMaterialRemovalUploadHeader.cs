using OracleMESFeeds.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class OracleMaterialRemovalUploadHeader
    {
        private RemovalUpload removalUpload { get; set; }

        public string TransactionType => "RemovalUpload";
        public string TransactionID => removalUpload.TransactionId;
        public DateTime TransactionDate => DateTime.Parse(removalUpload.TransactionDateTime);
        public string WorkOrderName => removalUpload.WorkOrderName;

        public OracleMaterialRemovalUploadHeader (RemovalUpload ru)
        {
            removalUpload = ru;
        }
    }
}
