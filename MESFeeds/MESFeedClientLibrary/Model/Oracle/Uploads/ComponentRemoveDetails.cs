using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Uploads
{
    [XmlRoot("ComponentRemoveDetails")]
    public class ComponentRemoveDetails
    {
        [XmlElement("Detail")]
        public List<RemovalDetail> Details { get;  set; }
    }
}