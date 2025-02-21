using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TDBMESFeeds.Messages
{
    [Serializable, XmlType("TrainingRequirementGroup")]
    public class TrainingRequirementGroup
    {
        [XmlIgnore]
        public string TrainingRequirementGroupName { get; private set; }

        [XmlElement("TrainingRequirementGroupName", Order = 1)]
        public XmlCDataSection TrainingRequirementGroupNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementGroupName);
            }
            set { }
        }

        [XmlIgnore]
        public string TrainingRequirementGroupDescription { get; private set; }

        [XmlElement("TrainingRequirementGroupDescription", Order = 2)]
        public XmlCDataSection TrainingRequirementGroupDescriptionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementGroupDescription);
            }
            set { }
        }

        [XmlElement("TrainingRequirement", Order = 3)]
        public List<TrainingRequirement> TrainingRequirements { get; private set; }

        public TrainingRequirementGroup() { }
        public TrainingRequirementGroup(string name, string desc)
        {
            this.TrainingRequirementGroupName = name;
            this.TrainingRequirementGroupDescription = desc;
            this.TrainingRequirements = new List<TrainingRequirement>();
        }
        public TrainingRequirementGroup (string rgName, string rgDesc, string reqName, string reqRev)
        {
            this.TrainingRequirementGroupName = rgName;
            this.TrainingRequirementGroupDescription = rgDesc;
            this.TrainingRequirements = GetTrainingRequirementList(reqName, reqRev);
        }

        private List<TrainingRequirement> GetTrainingRequirementList(string reqName, string reqRev)
        {
            return new List<TrainingRequirement>() { new TrainingRequirement(reqName, reqRev) };
        }
    }
}
