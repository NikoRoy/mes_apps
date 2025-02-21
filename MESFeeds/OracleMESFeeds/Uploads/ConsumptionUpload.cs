using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OracleMESFeeds.Uploads
{
    [XmlRoot("Message")]
    public class ConsumptionUpload
    {
        [XmlElement("TransactionType")]
        public const string TransactionType = "ConsumptionUpload";

        [XmlElement("TransactionId")]
        public string TransactionId { get; set; }

        [XmlElement("TransactionDateTime")]
        public string TransactionDateTime { get; set; }

        [XmlElement("WorkOrderName")]
        public string WorkOrderName { get;  set; }

        [XmlElement("ComponentIssueDetails")]
        public ComponentIssueDetails componentIssueDetails { get;  set; }
    }
}
