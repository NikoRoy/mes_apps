using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion.Messages
{   
    [Serializable, XmlType("Equipment")]
    public class AssetEquipment
    {
        public AssetEquipment()
        {
        }
        public AssetEquipment(string id, string desc, string status, string state, string lastmod, string scope)
        {
            this.AssetID = id;
            this.AssetDescription = desc;
            this.AssetStatus = status;
            this.AssetState = state;
            this._ahLastMod = lastmod;
            this.AssetScope = scope;
        }


        [XmlElement("AHAssetID")]
        public string AssetID { get;  set; }

        [XmlElement("AHAssetDesc")]
        public string AssetDescription { get;  set; }

        [XmlElement("AHAssetStatus")]
        public string AssetStatus { get;  set; }

        [XmlElement("AHStateName")]
        public string AssetState { get;  set; }

        [XmlElement("AHLastModified")]
        public string _ahLastMod;
        [XmlIgnore]
        public DateTime? AssetLastModified
        {
            get
            {
                DateTime ret;
                return !string.IsNullOrWhiteSpace(_ahLastMod) && DateTime.TryParse(_ahLastMod, out ret) ? (DateTime?)ret : null;
            }
        }

        [XmlElement("AHScopeName")]
        public string AssetScope { get;  set; }

        //public XmlCDataSection AssetIDCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AssetID);
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //public XmlCDataSection AssetDescriptionCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AssetDescription);
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //public XmlCDataSection AssetStatusCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AssetStatus);
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //public XmlCDataSection AssetStateCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AssetState);
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
        //public XmlCDataSection AssetScopeCData
        //{
        //    get
        //    {
        //        return new XmlDocument().CreateCDataSection(AssetScope);
        //    }
        //    set { /*Setter Intentionally Unused*/ }
        //}
    }
}
