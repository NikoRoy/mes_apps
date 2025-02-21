using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Messages
{
    [Serializable, XmlType("Attributes")]
    public class AttributeList
    {
        [XmlElement("Attribute")]
        public List<Attribute> AttributeListS
        {
            get;
            private set;
        }

        public AttributeList() 
        {
            this.AttributeListS = new List<Attribute>();
        }

        public AttributeList(List<Attribute> attributeList)
        {
            this.AttributeListS = attributeList;
        }

        public void Add(Attribute attribute)
        {
            this.AttributeListS.Add(attribute);
        }

        public void AddRange(IEnumerable<Attribute> attributes)
        {
            this.AttributeListS.AddRange(attributes);
        }

    }

}
