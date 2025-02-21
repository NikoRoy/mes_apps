using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientEFModel;
using Azure.Messaging.ServiceBus;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Model.BlueMountaion;
using System.Linq;

namespace MESFeedClientLibrary.Activity
{
    public class ServiceBusActivity : IActivityLogger<MessageAdapter>
    {
        public async Task LogActivity(MessageAdapter obj, string action, string response)
        {
            using (var context = EntityFactory.GenerateContext())
            {
                var i = new tblServiceBusIntake()
                {
                    CorrelationId = obj._received.CorrelationId,
                    CreationDate = DateTime.Now,
                    DeadLettered = false,
                    DeadLetterErrorDescription = obj._received.DeadLetterErrorDescription,
                    DeadLetterReason = obj._received.DeadLetterReason,
                    DeadLetterSource = obj._received.DeadLetterSource,
                    DeliveryCount = obj._received.DeliveryCount,
                    DeletedFromQueue = false,
                    EnqueuedSequenceNumber = obj._received.EnqueuedSequenceNumber,
                    EnqueuedTime = obj._received.EnqueuedTime,
                    ExpiresAt = obj._received.ExpiresAt,
                    InboundXML = obj.ToXml(),
                    LockedUntil = obj._received.LockedUntil,
                    LockToken = obj._received.LockToken,
                    MessageId = obj._received.MessageId,
                    PartitionKey = obj._received.PartitionKey,
                    TransactionPartitionKey = obj._received.TransactionPartitionKey,
                    ReplyTo = obj._received.ReplyTo,
                    ReplyToSessionId = obj._received.ReplyToSessionId,
                    ScheduledEnqueueTime = obj._received.ScheduledEnqueueTime,
                    SequenceNumber = obj._received.SequenceNumber,
                    SessionId = obj._received.SessionId,
                    State = obj._received.State.ToString(),
                    Subject = obj._received.Subject,
                    TimeToLive = obj._received.TimeToLive.Ticks,
                    To = obj._received.To,
                    UpdateDate = DateTime.Now
                };
                context.tblServiceBusIntakes.Add(i);
                await context.SaveChangesAsync();
            }
        }
    }
}
