using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion.Messages
{
    [Serializable, XmlType("Schedule")]
    public class EVSchedule
    {
        public EVSchedule()
        {

        }
        public EVSchedule(int rowId, string scheduleType, string eventType, string eventid, string dueDate)
        {
            this.RowID = rowId;
            this.ScheduleType = scheduleType;
            this.EventType = eventType;
            this.EventID = eventid;
            this._aeDueDate = dueDate;
        }
        [XmlAttribute("RowID")]
        public int RowID { get; set; }

        //[XmlIgnore]
        [XmlElement("M_AEScheduleType")]
        public string ScheduleType { get; set; }

        //[XmlIgnore]
        [XmlElement("M_EVRTName")]
        public string EventType { get; set; }

        [XmlElement("M_EVEventID")]
        public string EventID { get; set; }


        [XmlElement("AEDueDate")]
        public string _aeDueDate;

        [XmlIgnore]
        public DateTime? AEDueDate
        {
            get
            {
                DateTime ret;
                return !string.IsNullOrWhiteSpace(_aeDueDate) && DateTime.TryParse(_aeDueDate, out ret) ? (DateTime?)ret : null;
            }
        }


        //[XmlElement("M_AEScheduleType", Order = 1)]
        //public XmlCDataSection ScheduleTypeCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(ScheduleType.ToString());
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //[XmlElement("M_EVRTName", Order = 2)]
        //public XmlCDataSection EventTypeCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(EventType.ToString());
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //[XmlElement("AEDueDate", Order = 3)]
        //public XmlCDataSection AEDueDateCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AEDueDate.ToString());
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
    }
}
