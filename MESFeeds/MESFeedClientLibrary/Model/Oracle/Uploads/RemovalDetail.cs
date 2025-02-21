using System;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("Detail")]
    public class RemovalDetail
    {

        [XmlElement("RouteStepName")]
        public string RouteStepName { get;  set; }

        [XmlElement("ProductName")]
        public string ProductName { get;  set; }

        [XmlElement("LotNumRemoveToInv")]
        public string LotNumRemoveToInv { get;  set; }

        [XmlElement("InvLocToRemoveTo")]
        public string InvLocToRemoveTo { get;  set; }

        [XmlElement("QtyToRemove")]
        public float QtyToRemove { get;  set; }

        [XmlElement("ManufacturingProcedure")]
        public string ManufacturingProcedure { get; set; }

        [XmlElement("LossReason")]
        public string LossReason { get; set; }
    }
}