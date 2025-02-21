using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Mock
{
    public delegate Task MessageReceivedAsync(string message, string operationId, string parentId, CancellationToken cancellationToken);
    
    public interface IConsumerAzureServiceBus
    {
        Task StartReceivingMessagesAsync(string queueOrTopic, string subscriptionName, CancellationToken token);
    }
    public interface IConsumerService
    {

        event MessageReceivedAsync ProcessMessageAsync;

        Task StartConsumingAsync(CancellationToken token);
        Task StopConsumingAsync(CancellationToken token);
    }
    public abstract class BaseAzrueServiceBusConfiguration
    {
        public string ConnectionString { get; set; }

        public string QueueOrTopic { get; set; }
    }
    public class AzureServiceBusConsumerConfiguration : BaseAzrueServiceBusConfiguration
    {
        public const string Position = "AzureServiceBus:Consumer";

        public string SubscriptionName { get; set; }
    }
    public class ConsumerAzureServiceBus : IConsumerService, IConsumerAzureServiceBus
    {
        //private readonly ILogger<ConsumerAzureServiceBus> _logger;
        //private readonly TelemetryClient _telemetryClient;
        public string ErrorMessage { get; set; }
        private readonly AzureServiceBusConsumerConfiguration _azureServiceBusConfiguration;

        private ServiceBusProcessor _processor;
        private CancellationToken _cancellationToken;

        #region Event Delegate

        /// <summary>
        /// Note: I typically don't do this since microservices are atomic in design.
        /// However, for this demo, I will share a common service bus consumer between
        /// projects. In theory this could be done and would work fine but it creates a
        /// dependency between the microservices through shared code. This technique
        /// is something to carefuly consider.
        /// </summary>

        // delegates and events for message processing
        //public delegate Task MessageReceivedAsync(string message, string operationId, string parentId, CancellationToken cancellationToken);
        public event Mock.MessageReceivedAsync ProcessMessageAsync;

        // object lock
        object objectLock = new object();

        event MessageReceivedAsync IConsumerService.ProcessMessageAsync
        {
            add
            {
                lock (objectLock)
                {
                    ProcessMessageAsync += value;
                }
            }

            remove
            {
                lock (objectLock)
                {
                    ProcessMessageAsync -= value;
                }
            }
        }

        #endregion

        //public ConsumerAzureServiceBus(ILogger<ConsumerAzureServiceBus> logger, TelemetryClient telemetryClient, AzureServiceBusConsumerConfiguration azureServiceBusConsumerConfiguration)
        //{
        //    _logger = logger;
        //    _telemetryClient = telemetryClient;
        //    _azureServiceBusConfiguration = azureServiceBusConsumerConfiguration;
        //}
        public ConsumerAzureServiceBus(AzureServiceBusConsumerConfiguration azureServiceBusConsumerConfiguration)
        {
            _azureServiceBusConfiguration = azureServiceBusConsumerConfiguration;
        }

        //public void LogStartupInformation()
        //{
        //    _logger.LogInformation("Azure Service Bus Consumer Starting.");
        //}

        public Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            //LogStartupInformation();
            return StartReceivingMessagesAsync(_azureServiceBusConfiguration.QueueOrTopic, _azureServiceBusConfiguration.SubscriptionName, cancellationToken);
        }

        [ExcludeFromCodeCoverage]
        public virtual ServiceBusClient GetServiceBusClient()
        {
            return new ServiceBusClient(_azureServiceBusConfiguration.ConnectionString);
        }

        public async Task StartReceivingMessagesAsync(string queueOrTopic, string subscriptionName, CancellationToken cancellationToken)
        {
            try
            {
                // get service bus client
                var client = GetServiceBusClient();

                // create a processor
                _processor = client.CreateProcessor(queueOrTopic, subscriptionName, new ServiceBusProcessorOptions());

                // cancellation token
                _cancellationToken = cancellationToken;

                // process message handler
                _processor.ProcessMessageAsync += MessageHandler;

                // error handler
                _processor.ProcessErrorAsync += ErrorHandler;

                // start processing
                await _processor.StartProcessingAsync(_cancellationToken);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"ConsumerAzureServiceBus->ReceiveMessageAsync(queueOrTopic: ({queueOrTopic}), subscriptionName: ({subscriptionName}).", ex);
            }
        }

        public Task StopConsumingAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public virtual async Task MessageHandler(ProcessMessageEventArgs args)
        {
            // message body
            var body = args.Message.Body.ToString();

            // extract root operation id and parent id
            var rootOperationId = args.Message.ApplicationProperties["OperationId"];
            var parentId = args.Message.ApplicationProperties["ParentId"];

            //using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("ServiceBusProcessor.ProcessMessage", rootOperationId, parentId))
            //{
            //    // log information
            //    //_logger.LogInformation($"Received Message (queueOrTopic: ({_azureServiceBusConfiguration.QueueOrTopic}), subscriptionName: ({_azureServiceBusConfiguration.SubscriptionName}), Operation ID: ({rootOperationId}), Parent ID: ({parentId}).");
            //    //_logger.LogInformation($"{body}");

            //    // update parent id
            //    parentId = operation.Telemetry.Id;

            //    // process message
            //    if (ProcessMessageAsync != null)
            //    {
            //        await ProcessMessageAsync?.Invoke(body, rootOperationId, parentId, _cancellationToken);
            //    }

            //    // we can evaluate application logic and use that to determine how to settle the message.
            //    await args.CompleteMessageAsync(args.Message);
            //}
            if (ProcessMessageAsync != null)
            {
                await ProcessMessageAsync?.Invoke(body, rootOperationId.ToString(), parentId.ToString(), _cancellationToken);
            }

            // we can evaluate application logic and use that to determine how to settle the message.
            await args.CompleteMessageAsync(args.Message);

        }

        public virtual Task ErrorHandler(ProcessErrorEventArgs args)
        {
            var ex = args?.Exception;
            this.ErrorMessage = ex.Message;
            // log error
            //_logger.LogError($"ErrorHandler: {args.Exception.Message}");

            //// the error source tells me at what point in the processing an error occurred
            //_logger.LogInformation(args.ErrorSource.ToString());
            //_logger.LogInformation(args.FullyQualifiedNamespace);
            //_logger.LogInformation(args.EntityPath);
            //_logger.LogInformation(args.Exception.ToString());

            if (ex is ServiceBusException)
            {
                var asbEx = (ServiceBusException)ex;

                if (asbEx.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
                {
                    // queue or topic does not exist
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }


    }
}
