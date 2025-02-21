using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TDBMESFeeds.Messages
{
    [Serializable, XmlType("Message")]
    public class TrainingRecordDownloadLite
    {
        public string EmployeeName { get; set; }
        public string TrainingRequirementName { get; set; }
    }
}
