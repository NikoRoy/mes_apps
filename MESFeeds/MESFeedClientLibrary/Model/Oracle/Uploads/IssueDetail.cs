using System;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("Detail")]
    public class IssueDetail
    {
        [XmlElement("RouteStepName")]
        public string RouteStepName { get;  set; }

        [XmlElement("ProductName")]
        public string ProductName { get;  set; }

        [XmlElement("ConsumedQty")]
        public float ConsumedQty { get;  set; }

        [XmlElement("UOM")]
        public string UOM { get;  set; }

        [XmlElement("FromContainer")]
        public string FromContainer { get;  set; }

        [XmlElement("IssueDifferenceReason")]
        public string IssueDifferenceReason { get;  set; }

        [XmlElement("FromStockPoint")]
        public string FromStockPoint { get;  set; }

        [XmlElement("ManufacturingProcedure")]
        public string ManufacturingProcedure { get; set; }

        [XmlElement("LossReason")]
        public string LossReason { get; set; }
    }
}