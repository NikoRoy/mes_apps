using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("Message")]
    public class RemovalUpload
    {
        [XmlElement("TransactionType")]
        public const string TransactionType = "RemovalUpload";

        [XmlElement("TransactionId")]
        public string TransactionId { get; set; }

        [XmlElement("TransactionDateTime")]
        public string TransactionDateTime { get; set; }

        [XmlElement("WorkOrderName")]
        public string WorkOrderName { get;  set; }

        [XmlElement("ComponentRemoveDetails")]
        public ComponentRemoveDetails componentRemovalDetails { get;  set; }
    }
}
