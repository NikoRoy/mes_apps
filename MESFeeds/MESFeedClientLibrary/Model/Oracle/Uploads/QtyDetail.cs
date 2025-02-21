using System;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("Detail")]
    public class QtyDetail
    {
        [XmlElement("TransactionDate")]
        public DateTime TransactionDateTime { get;  set; }

        [XmlElement("LossQty")]
        public float LossQty { get;  set; }

        [XmlElement("LossQtyUOM")]
        public string LossQtyUOM { get;  set; }

        [XmlElement("LossReason")]
        public string LossReason { get;  set; }

        [XmlElement("ManufacturingProcedure")]
        public string ManufacturingProcedure { get; set; }

        [XmlElement("QtyDeducted")]
        public string QtyDeducted { get; set; }
    }
}