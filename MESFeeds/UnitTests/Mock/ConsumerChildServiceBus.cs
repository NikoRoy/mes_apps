using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MESFeedClientLibrary.Classes;

namespace UnitTests.Mock
{
    internal class ConsumerChildServiceBus : ConsumerAzureServiceBus
    {
        public ConsumerChildServiceBus(AzureServiceBusConsumerConfiguration azureServiceBusConsumerConfiguration) : base(azureServiceBusConsumerConfiguration)
        {
        }

        public override async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var ma = new MessageAdapter(args.Message);
            //await this._logger.LogActivity(ma, "", "");

            await args.CompleteMessageAsync(args.Message);
        }
    }
}
