using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OracleMESFeeds.Messages
{
    //XmlType overridden to generic message instead of class name
    [Serializable, XmlType("Message")]
    public class WorkOrder
    {
        [XmlIgnore]
        public const string TransactionType = "WorkOrderDownload";

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

        [XmlIgnore]
        public String MfgOrderName
        {
            get;
            private set;
        }

        [XmlElement("WorkOrderID", Order = 4)]
        public XmlCDataSection MfgOrderNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(MfgOrderName);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String Description
        {
            get;
            private set;
        }

        [XmlElement("Description", Order = 5)]
        public XmlCDataSection DescriptionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Description);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String Notes
        {
            get;
            private set;
        }

        [XmlElement("Notes", Order = 6)]
        public XmlCDataSection NotesCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Notes);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String OrderStatus
        {
            get;
            private set;
        }

        [XmlElement("OrderStatus", Order = 7)]
        public XmlCDataSection OrderStatusCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(OrderStatus);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String OrderType
        {
            get;
            private set;
        }

        [XmlElement("OrderType", Order = 8)]
        public XmlCDataSection OrderTypeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(OrderType);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String Factory
        {
            get;
            private set;
        }

        [XmlElement("Factory", Order = 9)]
        public XmlCDataSection FactoryCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Factory);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlElement("Product", Order = 10)]
        public List<Product> Product
        {
            get;
            private set;
        }

        [XmlIgnore]
        public decimal Qty
        {
            get;
            private set;
        }

        [XmlElement("Qty", Order = 11)]
        public XmlCDataSection QtyCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Qty.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String QtyUOM
        {
            get;
            private set;
        }

        [XmlElement("QtyUOM", Order = 12)]
        public XmlCDataSection UOMCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(QtyUOM);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public decimal Qty2
        {
            get;
            private set;
        }

        [XmlElement("Qty2", Order = 13)]
        public XmlCDataSection Qty2CData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Qty2.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String QtyUOM2
        {
            get;
            private set;
        }

        [XmlElement("QtyUOM2", Order = 14)]
        public XmlCDataSection UOM2CData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(QtyUOM2);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String Priority
        {
            get;
            private set;
        }

        [XmlElement("Priority", Order = 15)]
        public XmlCDataSection PriorityCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Priority);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTimeOffset? ReleaseDate
        {
            get;
            private set;
        }

        [XmlElement("ReleaseDate", Order = 16)]
        public XmlCDataSection ReleaseDateCData
        {
            get
            {
                if (!ReleaseDate.HasValue) return new XmlDocument().CreateCDataSection("");
                return new XmlDocument().CreateCDataSection(ReleaseDate.Value.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String CompletionSubInventory
        {
            get;
            private set;
        }

        [XmlElement("CompletionSubInventory", Order = 17)]
        public XmlCDataSection CompletionSubInventoryCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(CompletionSubInventory);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTimeOffset? PlannedStartDate
        {
            get;
            private set;
        }

        [XmlElement("PlannedStartDate", Order = 18)]
        public XmlCDataSection PlannedStartDateCData
        {
            get
            {
                if (!PlannedStartDate.HasValue) return new XmlDocument().CreateCDataSection("");
                return new XmlDocument().CreateCDataSection(PlannedStartDate.Value.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTimeOffset? PlannedCompletionDate
        {
            get;
            private set;
        }

        [XmlElement("PlannedCompletionDate", Order = 19)]
        public XmlCDataSection PlannedCompletionDateCData
        {
            get
            {
                if (!PlannedCompletionDate.HasValue) return new XmlDocument().CreateCDataSection("");
                return new XmlDocument().CreateCDataSection(PlannedCompletionDate.Value.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public String LotNumber
        {
            get;
            private set;
        }

        [XmlElement("LotNumber", Order = 20)]
        public XmlCDataSection LotNumberCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(LotNumber);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlElement("MaterialList", Order = 21)]
        public List<MaterialList> MaterialList
        {
            get;
            private set;
        }

        [XmlElement("Attributes", Order = 30)]
        public List<Attribute> Attributes
        {
            get;
            private set;
        }

        public WorkOrder() 
        {
            this.MaterialList = new List<MaterialList>();
            this.MaterialList.Add(new MaterialList());
        }

        public WorkOrder
        (
            string mfgOrderName,
            string description,
            string notes,
            string orderStatus,
            string orderType,
            string factory,
            List<Product> products,
            decimal qty,
            string uom,
            decimal qty2,
            string uom2,
            string priority,
            DateTimeOffset? releaseDate,
            string completionSubInventory,
            DateTimeOffset? plannedStartDate,
            DateTimeOffset? plannedCompletionDate,
            string lotNumber,
            List<Attribute> attributes
        )
        {
            this.TransactionId = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.MfgOrderName = mfgOrderName;
            this.Description = description;
            this.Notes = notes;
            this.OrderStatus = orderStatus;
            this.OrderType = orderType;
            this.Factory = factory;
            this.Product = products;
            this.Qty = qty;
            this.QtyUOM = uom;
            this.Qty2 = qty2;
            this.QtyUOM2 = uom2;
            this.Priority = priority;
            this.ReleaseDate = releaseDate;
            this.CompletionSubInventory = completionSubInventory;
            this.PlannedStartDate = plannedStartDate;
            this.PlannedCompletionDate = plannedCompletionDate;
            this.LotNumber = lotNumber;
            this.Attributes = attributes;

            this.MaterialList = new List<MaterialList>();
            this.MaterialList.Add(new MaterialList());
        }

        public void AddMaterialItem(MaterialItem materialItem)
        {

            this.MaterialList[0].Add(materialItem);
        }

        public void AddMaterialItem(
            List<Product> materialList,
            int issueControl,
            string defaultStockPullLocation,
            string erpOperation,
            int qtyRequired,
            string uom1
        )
        {
            this.AddMaterialItem(
                new MaterialItem(
                    productList: materialList,
                    issueControl: issueControl,
                    defaultStockPullLocation: defaultStockPullLocation,
                    erpOperation: erpOperation,
                    qtyRequired: qtyRequired,
                    uom: uom1
                )
            );
        }

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