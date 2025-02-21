using Azure.Messaging.ServiceBus;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.BlueMountaion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Classes
{
    public class MessageAdapter : IMessage
    {
        public ServiceBusReceivedMessage _received;
        public ServiceBusDownload _body;

        public MessageAdapter(ServiceBusReceivedMessage rm)
        {
            this._received = rm;
            this._body = this.DeserializeMessage(rm);
        }

        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            if (_body == null)
                _body = DeserializeMessage(this._received);
            return _body.ToXml();
        }
        private ServiceBusDownload DeserializeMessage(ServiceBusReceivedMessage message)
        {
            XmlSerializer xser = new XmlSerializer(typeof(ServiceBusDownload));
            return (ServiceBusDownload)xser.Deserialize(message.Body.ToStream());
        }

    }
}
