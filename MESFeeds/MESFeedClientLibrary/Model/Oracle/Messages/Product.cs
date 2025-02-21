using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Messages
{
    [Serializable, XmlType("Product")]
    public class Product
    {
        internal static List<Product> GetSingleProductList(string productName, string productRevision)
        {
            List<Product> productList = new List<Product>();
            productList.Add(new Product(productName, productRevision));
            return productList;
        }

        //Internal property - these are for .net to store data, and are skipped by XML Serialization
        [XmlIgnore]
        public string Name
        {
            get;
            private set;
        }

        //XML CData property - read only & depends on internal property, used to define node value during XML Serialization
        [XmlElement("Name", Order=1)]
        public XmlCDataSection NameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Name);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string Revision
        {
            get;
            private set;
        }

        [XmlElement("Revision", Order=2)]
        public XmlCDataSection RevisionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Revision);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        public Product() { }

        public Product(string name, string revision)
        {
            this.Name = name;
            this.Revision = revision;
        }
    }
}
