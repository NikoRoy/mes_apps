using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("ChangeQtyDetails")]
    public class ChangeQtyDetails
    {
        [XmlElement("Detail")]
        public List<QtyDetail> Details { get; set; }
    }
}