//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MESFeedClientEFModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblServiceBusIntake
    {
        public int Id { get; set; }
        public bool DeletedFromQueue { get; set; }
        public bool DeadLettered { get; set; }
        public string DeadLetterReason { get; set; }
        public string DeadLetterSource { get; set; }
        public string DeadLetterErrorDescription { get; set; }
        public Nullable<int> DeliveryCount { get; set; }
        public string ReplyTo { get; set; }
        public string State { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string CorrelationId { get; set; }
        public string ReplyToSessionId { get; set; }
        public string SessionId { get; set; }
        public string TransactionPartitionKey { get; set; }
        public string PartitionKey { get; set; }
        public Nullable<long> TimeToLive { get; set; }
        public Nullable<System.DateTimeOffset> ExpiresAt { get; set; }
        public Nullable<System.DateTimeOffset> EnqueuedTime { get; set; }
        public Nullable<long> EnqueuedSequenceNumber { get; set; }
        public Nullable<long> SequenceNumber { get; set; }
        public Nullable<System.DateTimeOffset> ScheduledEnqueueTime { get; set; }
        public Nullable<System.DateTimeOffset> LockedUntil { get; set; }
        public string LockToken { get; set; }
        public string InboundXML { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string MessageId { get; set; }
    }
}
