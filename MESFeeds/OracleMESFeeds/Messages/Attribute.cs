using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace OracleMESFeeds.Messages
{
    [Serializable, XmlType("Attribute")]
    public class Attribute
    {
        //Internal property - these are for .net to store data, and are skipped by XML Serialization
        [XmlIgnore]
        public string Name
        {
            get;
            private set;
        }

        //XML CData property - read only & depends on internal property, used to define node value during XML Serialization
        [XmlElement("Name", Order = 1)]
        public XmlCDataSection NameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Name);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string DataType
        {
            get;
            private set;
        }

        private enum DataTypeEnum
        { 
            Integer = 1,
            Float = 2,
            Fixed = 3,
            String = 4,
            Timestamp = 6,
            Boolean = 7,
            Decimal = 9
        }

        [XmlIgnore]
        private int DataTypeInt
        {
            get
            {
                DataTypeEnum enm = (DataTypeEnum)Enum.Parse(typeof(DataTypeEnum), this.DataType);
                return (int)enm;
            }
        }

        [XmlElement("DataType", Order = 2)]
        public XmlCDataSection RevisionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(DataTypeInt.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string AttributeValue
        {
            get;
            private set;
        }

        [XmlElement("AttributeValue", Order = 3)]
        public XmlCDataSection AttributeValueCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(AttributeValue);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string IsExpression
        {
            get;
            private set;
        }

        [XmlElement("IsExpression", Order = 4)]
        public XmlCDataSection IsExpressionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(IsExpression);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        public Attribute() { }

        public Attribute(string name, string dataType, string attributeValue, string isExpression)
        {
            this.Name = name;
            this.DataType = dataType;
            this.AttributeValue = attributeValue;
            this.IsExpression = isExpression;
        }
    }

}
