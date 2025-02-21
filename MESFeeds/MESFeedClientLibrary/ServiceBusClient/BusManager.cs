using System;
using MESFeedClientLibrary.Logger;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MESFeedClientLibrary.Model.BlueMountaion;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.SBC
{
    public class BusManager : IDisposable
    {
        private readonly object synclockclient = new object();
        private readonly object synclockprocessor = new object();
        private static volatile ServiceBusClient _client = null;
        private static ServiceBusProcessor _processor = null;
        ILogger _logger;
        private bool _disposed;
        private readonly string connection = null;
        private readonly string topic = null;
        private readonly string subscription = null;

        public BusManager(string namespaceCn, string topicName, string subscriptionName, ILogger servicelogger)
        {
            this.connection = namespaceCn;
            this.topic = topicName;
            this.subscription = subscriptionName;
            this._logger = servicelogger;
        }
        ~BusManager()
        {
            Dispose(false);
        }

        public ServiceBusClient GetSBClientInstance()
        {
            if (_client == null)
            {
                lock (synclockclient)
                {
                    if (_client == null)
                        _client = new ServiceBusClient(connection, new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
                }
            }
            return _client;
        }
        public ServiceBusProcessor GetSBProcessorInstance()
        {
            if (_processor == null)
            {
                lock (synclockprocessor)
                {
                    if (_processor == null)
                    {
                        _processor = GetSBClientInstance().CreateProcessor(topic, subscription, new ServiceBusProcessorOptions() { AutoCompleteMessages = false});
                        _processor.ProcessMessageAsync += MessageHandler;
                        _processor.ProcessErrorAsync += ErrorHandler;
                    }
                }
            }
            return _processor;
        }


        //received message handler
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                if (args.Message.DeliveryCount > 10)
                {
                    await args.DeadLetterMessageAsync(args.Message);
                }
                var ma = new MessageAdapter(args.Message);
                await this._logger.LogActivity(ma, "","");

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                await args.AbandonMessageAsync(args.Message);
                await this._logger.LogError(ex, args.Identifier);
                await this._logger.LogMessage(ex.Message, "Service Bus Message Handler error - Check log file for the identifier.");
            }
        }

        //receiving messages error handler
        async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            await _logger.LogError(args.Exception, args.Identifier + " - " +args.ErrorSource.ToString());
            await this._logger.LogMessage(args.Exception.Message, "Service Bus Message in flight error - Check log file for the identifier.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Task.Run(async () => await _processor.DisposeAsync());
                    Task.Run(async () => await _client.DisposeAsync());
                }
            }
            _disposed = true;
        }

        /*
 *         private async Task<bool> UpdateMES(MessageAdapter ma)
            {
                try
                {
                    var mesDownload = BlueMountainController.GetAssetDownload(ma._body);

                    var response = await _mesProcessor.Execute(mesDownload.ToXml());
                    await this._mesLogger.LogActivity(mesDownload, ma._received.MessageId, response);
                    if (!BlueMountainController.WasResponseSuccessful(response))
                    {
                        await this._mesLogger.LogError(new Exception(response), "Blue Mountain Feed Was Unsuccessful");
                        return false;
                    }
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            } 

 */
    }
}
