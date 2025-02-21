using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OracleMESFeeds.Uploads
{
    [XmlRoot("ChangeQtyDetails")]
    public class ChangeQtyDetails
    {
        [XmlElement("Detail")]
        public List<QtyDetail> Details { get; set; }
    }
}