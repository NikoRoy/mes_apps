using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MESFeedClientLibrary.Model.Training.Helper;

namespace MESFeedClientLibrary.Model.Training.Messages
{
    [Serializable, XmlType("Message")]
    public class TrainingBundleDownload : IMessage
    {
        [XmlIgnore]
        public const string TransactionType = "TrainingBundle";

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
        [XmlElement("TransactionID",Order = 2)]
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

        [XmlElement("TrainingRequirementGroup",Order = 4)]
        public TrainingRequirementGroup TrainingRequirementGroup
        {
            get;
            set;
        }
        
        public TrainingBundleDownload() { }
        public TrainingBundleDownload(TrainingRequirementGroup group)
        {
            this.TransactionID = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.TrainingRequirementGroup = group;
        }
        public TrainingBundleDownload
            (
                string groupname,
                string groupdesc
            )
        {
            this.TransactionID = Guid.NewGuid();
            this.TransactionDateTime = DateTimeOffset.Now;
            this.TrainingRequirementGroup = new TrainingRequirementGroup(groupname, groupdesc);
        }

        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(TrainingBundleDownload));

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
