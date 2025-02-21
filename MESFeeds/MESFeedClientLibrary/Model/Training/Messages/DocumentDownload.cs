using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Training.Messages
{
    [Serializable, XmlType("Message")]
    public class DocumentDownload : IMessage
    {
        //transaction type
        [XmlIgnore]
        public const string TransactionType = "doctrainingreqdownload";

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
        public Guid TransactionID
        {
            get;
            private set;
        }
        [XmlElement("TransactionID", Order = 2)]
        public XmlCDataSection TransactionIDCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionID.ToString());
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
        public string DocumentName
        {
            get;
            private set;
        }

        [XmlElement("DocumentIdentifier",Order = 4)]
        public XmlCDataSection DocumentNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(DocumentName);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public string DocumentRevision
        {
            get;
            private set;
        }

        [XmlElement("DocumentRevision",Order = 5)]
        public XmlCDataSection DocumentRevisionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(DocumentRevision);
            }
            set { /*Setter Intentionally Unused*/}
        }

        [XmlIgnore]
        public string Description
        {
            get;
            private set;
        }

        [XmlElement("Description",Order = 6)]
        public XmlCDataSection DescriptionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Description);
            }
            set
            { /*Setter Intentionally Unused**/}
        }

        [XmlIgnore]
        public string Identifier
        {
            get;
            private set;
        }

        [XmlElement("Identifier",Order = 7)]
        public XmlCDataSection IdentifierCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(Identifier);
            }
            set
            {
                /*Setter Intentionally Unused*/
            }
        }

        public DocumentDownload() { }
        public DocumentDownload (string docid, string docrev, string desc, string url)
        {
            this.TransactionID = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.DocumentName = docid;
            this.DocumentRevision = docrev;
            this.Description = desc;
            this.Identifier = url;
        }

        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(DocumentDownload));

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
