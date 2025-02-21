using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OracleMESFeeds.Messages
{
    [Serializable, XmlType("MaterialItem")]
    public class MaterialItem
    {
        internal static List<MaterialItem> GetSingleMaterialItemList(
            string materialName, 
            string materialRevision,
            int dIssueControl,
            string dDefaultStockPullLocation,
            string dErpOperation,
            decimal dQtyRequired,
            string dUom
        )
        {
            List<MaterialItem> matItemList = new List<MaterialItem>();
            matItemList.Add(
                new MaterialItem(
                        materialName,
                        materialRevision,
                        dIssueControl,
                        dDefaultStockPullLocation,
                        dErpOperation,
                        dQtyRequired,
                        dUom
                    )
                );
            return matItemList;
        }

        internal static List<MaterialItem> GetSingleMaterialItemList(
            List<Product> productList,
            int dIssueControl,
            string dDefaultStockPullLocation,
            string dErpOperation,
            decimal dQtyRequired,
            string dUom
        )
        {
            List<MaterialItem> matItemList = new List<MaterialItem>();
            matItemList.Add(
                new MaterialItem(
                        productList,
                        dIssueControl,
                        dDefaultStockPullLocation,
                        dErpOperation,
                        dQtyRequired,
                        dUom
                    )
                );
            return matItemList;
        }


        [XmlElement("Product", Order = 19)]
        public List<Product> ProductList
        {
            get;
            private set;
        }

        [XmlIgnore]
        public int IssueControl
        {
            get;
            private set;
        }

        [XmlElement("IssueControl", Order = 20)]
        public XmlCDataSection IssueControlCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(IssueControl.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string DefaultStockPullLocation
        {
            get;
            private set;
        }

        [XmlElement("DefaultStockPullLocation", Order = 21)]
        public XmlCDataSection DefaultStockPullLocationCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(DefaultStockPullLocation);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string ErpOperation
        {
            get;
            private set;
        }

        [XmlElement("ErpOperation", Order = 23)]
        public XmlCDataSection ErpOperationCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(ErpOperation);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public decimal QtyRequired
        {
            get;
            private set;
        }

        [XmlElement("QtyRequired", Order = 24)]
        public XmlCDataSection QtyRequiredCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(QtyRequired.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }
        [XmlIgnore]
        public String UOM
        {
            get;
            private set;
        }

        [XmlElement("UOM", Order = 25)]
        public XmlCDataSection UOMCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(UOM);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        public MaterialItem() { }

        public MaterialItem
        (
            List<Product> productList,
            int issueControl,
            string defaultStockPullLocation,
            string erpOperation,
            decimal qtyRequired,
            string uom
        )
        {
            this.ProductList = productList;
            this.IssueControl = issueControl;
            this.DefaultStockPullLocation = defaultStockPullLocation;
            this.ErpOperation = erpOperation;
            this.QtyRequired = qtyRequired;
            this.UOM = uom;
        }

        public MaterialItem
        (
            string materialProductName,
            string materialProductRevision,
            int issueControl,
            string defaultStockPullLocation,
            string erpOperation,
            decimal qtyRequired,
            string uom
        ) : this(
            Product.GetSingleProductList(materialProductName, materialProductRevision),
            issueControl,
            defaultStockPullLocation,
            erpOperation,
            qtyRequired,
            uom
        )
        { /*No further Operations*/ }

        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(WorkOrder));

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
