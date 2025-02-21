using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Classes
{
    [Serializable]
    public class JsonResponse
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("id")]
        public Guid ID { get; set; }

        [XmlElement("request")]
        public bool Request { get; set; }

        [XmlElement("response")]
        public bool Response { get; set; }

        [XmlElement("requestid")]
        public Guid RequestID { get; set; }

        [XmlElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [XmlElement("contents")]
        public XDocument Contents { get; set; }

        [XmlElement("count")]
        public int Count { get; set; }

    }
}
