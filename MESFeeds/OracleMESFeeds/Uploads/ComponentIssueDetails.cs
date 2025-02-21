using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OracleMESFeeds.Uploads
{
    [XmlRoot("ComponentIssueDetails")]
    public class ComponentIssueDetails
    {
        [XmlElement("Detail")]
        public List<IssueDetail> Details { get;  set; }
    }
}