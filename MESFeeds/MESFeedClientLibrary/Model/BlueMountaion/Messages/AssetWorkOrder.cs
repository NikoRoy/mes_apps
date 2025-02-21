using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion.Messages
{
    [Serializable, XmlType("Work")]
    public class AssetWorkOrder
    {
        public AssetWorkOrder() {}
        public AssetWorkOrder(string lastmod, string appext, string origdue, string evid, string eventname , string hist)
        {
            this._lastModifiedDate = lastmod;
            this._approvedExtensionDate = appext;
            this._originalDueDate = origdue;
            this.M_EHEVID = evid;
            this.M_EHRTName = eventname;
            this.M_EHHistoryID = hist;
        }

        [XmlAttribute("RowID")]
        public int RowID { get; set; }

        [XmlElement("M_EHRTName")]
        public string M_EHRTName { get; set; }

        [XmlElement("M_EHHistoryID")]
        public string M_EHHistoryID { get; set; }

        [XmlElement("M_EHEVID")]
        public string M_EHEVID { get; set; }

        [XmlElement("EHStateName")]
        public string EHStateName { get; set; }

        [XmlElement("EHDueDate")]
        public string _originalDueDate;

        [XmlIgnore]
        public DateTime? EHDueDate
        {
            get
            {
                DateTime ret;
                return !string.IsNullOrWhiteSpace(_originalDueDate) && DateTime.TryParse(_originalDueDate, out ret) ? (DateTime?)ret : null;
            }
        }

        [XmlElement("EHLastModified")]
        public string _lastModifiedDate;

        [XmlIgnore]
        public DateTime? EHLastModifiedDate
        {
            get
            {
                DateTime ret;
                return !string.IsNullOrWhiteSpace(_lastModifiedDate) && DateTime.TryParse(_lastModifiedDate, out ret) ? (DateTime?)ret : null;
            }
        }

        [XmlElement("M_EH2_UDF15")]
        public string _approvedExtensionDate;

        [XmlIgnore]
        public DateTime? M_EH2_UDF15
        {
            get
            {
                DateTime ret;
                return !string.IsNullOrWhiteSpace(_approvedExtensionDate) && DateTime.TryParse(_approvedExtensionDate, out ret) ? (DateTime?)ret : null;
            }
        }

    }
}
