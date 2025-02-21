using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Messages
{
    //XmlType overridden to generic message instead of class name
    [Serializable, XmlType("Message")]
    public class Item
    {
        [XmlIgnore]
        public const string TransactionType = "ProductDownload";

        [XmlElement("TransactionType", Order = 1)]
        public XmlCDataSection TransactionTypeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionType);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public Guid TransactionId
        {
            get;
            private set;
        }

        [XmlElement("TransactionId", Order = 2)]
        public XmlCDataSection TransactionIdCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionId.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTimeOffset TransactionDateTime
        {
            get;
            private set;
        }

        [XmlElement("TransactionDateTime", Order = 3)]
        public XmlCDataSection TransactionDateTimeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionDateTime.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlElement("Product", Order = 4)]
        public List<Product> ProductList
        {
            get;
            private set;
        }

        [XmlIgnore]
        public string Description
        {
            get;
            private set;
        }

        [XmlElement("Description", Order = 5)]
        public XmlCDataSection InventoryLotNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Description);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public int Status
        {
            get;
            private set;
        }

        [XmlElement("Status", Order = 6)]
        public XmlCDataSection StatusCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Status.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string ProductType
        {
            get;
            private set;
        }

        [XmlElement("ProductType", Order = 7)]
        public XmlCDataSection FactoryCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(ProductType);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string ProductFamily
        {
            get;
            private set;
        }

        [XmlElement("ProductFamily", Order = 8)]
        public XmlCDataSection InventoryLocationCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(ProductFamily);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public decimal StartQuantity
        {
            get;
            private set;
        }

        [XmlElement("StartQuantity", Order = 9)]
        public XmlCDataSection QtyCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(StartQuantity.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string StartQuantityUom
        {
            get;
            private set;
        }

        [XmlElement("StartQuantityUOM", Order = 10)]
        public XmlCDataSection QtyUomCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(StartQuantityUom);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlElement("Attributes", Order = 11)]
        public List<AttributeList> Attributes
        {
            get;
            private set;
        }

        public Item() { }

        public Item
        (
            List<Product> productList,
            string description,
            int status,
            string productType,
            string productFamily,
            decimal startQuantity,
            string startQuantityUom,
            List<Attribute> attributes
        )
        {
            this.TransactionId = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.ProductList = productList;
            this.Description = description;
            this.Status = status;
            this.ProductType = productType;
            this.ProductFamily = productFamily;
            this.StartQuantity = startQuantity;
            this.StartQuantityUom = startQuantityUom;
            this.Attributes = new List<AttributeList>();
            this.Attributes.Add(new AttributeList(attributes));
        }

        public Item
        (
            string productName,
            string productRevision,
            string description,
            int status,
            string productType,
            string productFamily,
            decimal startQuantity,
            string startQuantityUom,
            List<Attribute> attributes
        ) : this
        (
            Product.GetSingleProductList(productName, productRevision),
            description,
            status,
            productType,
            productFamily,
            startQuantity,
            startQuantityUom,
            attributes
        )
        {
            //No further functions
        }

        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(Item));

            using (StringWriter stream = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, xset))
                {

                    serializer.Serialize(writer, this, excludeNameSpace ? emptyNs : null);

                    return stream.ToString();
                }
            }
        }

    }
}