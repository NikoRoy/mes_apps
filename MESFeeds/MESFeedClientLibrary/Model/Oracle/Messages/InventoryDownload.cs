using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Messages
{
    //XmlType overridden to generic message instead of class name
    [Serializable, XmlType("Message")]
    public class InventoryDownload
    {
        [XmlIgnore]
        public const string TransactionType = "InventoryDownload";

        [XmlElement("TransactionType", Order=1)]
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

        [XmlElement("TransactionId", Order=2)]
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

        [XmlElement("TransactionDateTime", Order=3)]
        public XmlCDataSection TransactionDateTimeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionDateTime.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string InventoryLotName
        {
            get;
            private set;
        }

        [XmlElement("InventoryLotName", Order=4)]
        public XmlCDataSection InventoryLotNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(InventoryLotName);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlElement("Product", Order=5)]
        public List<Product> ProductList
        {
            get;
            private set;
        }


        [XmlIgnore]
        public const string Status = "Open";

        [XmlElement("Status", Order=6)]
        public XmlCDataSection StatusCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Status);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public const string Factory = "Merrimack NH";

        [XmlElement("Factory", Order=7)]
        public XmlCDataSection FactoryCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Factory);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string InventoryLocation
        {
            get;
            private set;
        }

        [XmlElement("InventoryLocation", Order=8)]
        public XmlCDataSection InventoryLocationCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(InventoryLocation);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public decimal Qty
        {
            get;
            private set;
        }

        [XmlElement("Qty", Order=9)]
        public XmlCDataSection QtyCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Qty.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string QtyUom
        {
            get;
            private set;
        }

        [XmlElement("QtyUOM", Order=10)]
        public XmlCDataSection QtyUomCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(QtyUom);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTime? ExpirationDate
        {
            get;
            private set;
        }

        [XmlElement("ExpirationDate", Order=11)]
        public XmlCDataSection ExpirationDateCData
        {
            get
            {
                return new XmlDocument().
                    CreateCDataSection
                    (
                        ExpirationDate.HasValue ? 
                        ExpirationDate.Value.ToString("o") : 
                        ""
                    );
            }
            set { /*Setter Intentionally Unused*/ }
        }

        public InventoryDownload() { }

        public InventoryDownload
        (
            string inventoryLotName,
            List<Product> productList,
            string inventoryLocation,
            decimal qty,
            string qtyuom,
            DateTime? expirationDate
        )
        {
            this.TransactionId = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.InventoryLotName = inventoryLotName;
            this.ProductList = productList;
            this.InventoryLocation = inventoryLocation;
            this.Qty = qty;
            this.QtyUom = qtyuom;
            this.ExpirationDate = expirationDate;
        }

        public InventoryDownload
        (
            string inventoryLotName,
            string productName,
            string productRevision,
            string inventoryLocation,
            decimal qty,
            string qtyuom,
            DateTime? expirationDate
        ) : this
        (
            inventoryLotName,
            Product.GetSingleProductList(productName, productRevision),
            inventoryLocation,
            qty,
            qtyuom,
            expirationDate
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

            XmlSerializer serializer = new XmlSerializer(typeof(InventoryDownload));

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
