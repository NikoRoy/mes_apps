using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Training.Messages
{
    [Serializable, XmlType("TrainingRequirement")]
    public class TrainingRequirement
    {
        [XmlIgnore]
        public string TrainingRequirementName { get; private set; }

        [XmlElement("TrainingRequirementName", Order = 1)]
        public XmlCDataSection TrainingRequirementNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementName);
            }
            set { }
        }
        [XmlIgnore]
        public string TrainingRequirementRevision { get; private set; }

        [XmlElement("TrainingRequirementRevision", Order = 2)]
        public XmlCDataSection TrainingRequirementRevisionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementRevision);
            }
            set { }
        }

        public TrainingRequirement() { }
        public TrainingRequirement(string reqName, string reqRev)
        {
            this.TrainingRequirementName = reqName;
            this.TrainingRequirementRevision = reqRev;
        }
    }
}
