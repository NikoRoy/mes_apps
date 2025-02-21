using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BMRAMMesFeeds.Messages
{
    [Serializable, XmlType("Message")]
    public class AssetDownload : IMessage
    {
        [XmlIgnore]
        public const string TransactionType = "BlueMountainDownload";
        [XmlElement("TransactionType",Order = 1)]
        public XmlCDataSection TransactionTypeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionType);
            }
            set { }
        }

        [XmlIgnore]
        public Guid TransactionId
        {
            get;
            private set;
        }
        [XmlElement("TransactionId",Order = 2)]
        public XmlCDataSection TransactionIDCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionId.ToString());
            }
            set { }
        }

        [XmlIgnore]
        public DateTimeOffset TransactionDateTime
        {
            get;
            private set;
        }
        [XmlElement("TransactionDateTime",Order = 3)]
        public XmlCDataSection TransactionDatetimeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionDateTime.ToString("o"));
            }
            set { }
        }

        [XmlIgnore]
        public string EquipmentId
        {
            get;
            private set;
        }
        [XmlElement("EquipmentId",Order = 4)]
        public XmlCDataSection EquipmentIdCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(EquipmentId);
            }
            set { }
        }
        [XmlIgnore]
        public string EquipmentDescription
        {
            get;
            private set;
        }
        [XmlElement("EquipmentDescription",Order = 5)]
        public XmlCDataSection EquipmentDescriptionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(EquipmentDescription);
            }
            set { }
        }
        [XmlIgnore]
        public string EquipmentStatus
        {
            get;
            private set;
        }
        [XmlElement("EquipmentStatus",Order = 6)]
        public XmlCDataSection EquipmentStatusCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(EquipmentStatus);
            }
            set { }
        }

        [XmlIgnore]
        public DateTime? NextCalibrationDueDate
        {
            get;
            private set;
        }
        [XmlElement("NextCalibrationDueDate",Order = 7)]
        public XmlCDataSection NextCalibrationDueDateCData
        {
            get
            {
                return new XmlDocument().
                    CreateCDataSection
                    (
                        NextCalibrationDueDate.HasValue ?
                        NextCalibrationDueDate.Value.ToString("o") :
                        ""
                    );
            }
            set { }
        }

        public AssetDownload() { }
        public AssetDownload(string id, string desc, string status, DateTime? nextdue)
        {
            this.TransactionId = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.EquipmentId = id;
            this.EquipmentDescription = desc;
            this.EquipmentStatus = status;
            this.NextCalibrationDueDate = nextdue;
        }
        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(AssetDownload));

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
