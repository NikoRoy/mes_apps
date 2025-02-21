using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.BlueMountaion.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.BlueMountaion
{
    public enum ScheduleType
    {
        Weekly, 
        Monthly
    }
    public enum EventType
    {
        [Description("Maintenance Event")]
        Maintenance_Event,
        [Description("Calibration Event")]
        Calibration_Event
    }
    [Serializable, XmlType("Asset")]
    public class ServiceBusDownload : IMessage
    {
        public ServiceBusDownload()
        {
       
        }
        public ServiceBusDownload(string action, AssetEquipment eq, AssetEVSchedule ev, AssetHistory h)
        {
            this.Action = action;
            this.Equipment = eq;
            this.Schedule = ev;
            this.History = h;
        }

        [XmlElement("Action", Order=1)]
        public string Action { get; set; }

        [XmlElement("Equipment", Order = 2)]
        public AssetEquipment Equipment { get; set; }



        [XmlElement("EVSched", Order =3)]
        public AssetEVSchedule Schedule { get; set; }



        [XmlElement("History", Order =4)]
        public AssetHistory History { get; set; }


        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(ServiceBusDownload));

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
