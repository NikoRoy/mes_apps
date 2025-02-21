using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion.Messages
{
    [Serializable, XmlType("History")]
    public class AssetHistory
    {
        public AssetHistory()
        {

        }
        public AssetHistory(List<AssetWorkOrder> workOrders)
        {
            this.WorkOrders = workOrders;
        }

        [XmlElement("Work")]
        public List<AssetWorkOrder> WorkOrders { get; set; }
    }
}
