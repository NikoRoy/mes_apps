using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion.Messages
{
    [Serializable, XmlType("EVSched")]
    public class AssetEVSchedule
    {
        public AssetEVSchedule()
        {

        }
        public AssetEVSchedule(List<EVSchedule> col)
        {
            this.ScheduleList = col;
        }


        [XmlElement("Schedule")]
        public List<EVSchedule> ScheduleList
        {
            get;
            set;
        }
    }
}
