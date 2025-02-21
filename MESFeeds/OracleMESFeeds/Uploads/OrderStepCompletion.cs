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
    public class OrderStepCompletion
    {
        [XmlElement("TransactionType")]
        public const string TransactionType = "OrderStepCompletion";

        [XmlElement("TransactionId")]
        public string TransactionId { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDateTime { get; set; }

        

        [XmlElement("ContainerName")]
        public string ContainerName { get;  set; }

        [XmlElement("MfgOrderName")]
        public string MfgOrderName { get;  set; }

        [XmlElement("Factory")]
        public string Factory { get;  set; }

        [XmlElement("ContainerQty")]
        public float ContainerQty { get;  set; }

        [XmlElement("UOM")]
        public string UOM { get;  set; }

        [XmlElement("ContainerQty2")]
        public float ContainerQty2 { get;  set; }

        [XmlElement("UOM2")]
        public string UOM2 { get;  set; }

        [XmlElement("ChangeQtyDetails")]
        public ChangeQtyDetails changeQtyDetails { get;  set; }
    }
}
