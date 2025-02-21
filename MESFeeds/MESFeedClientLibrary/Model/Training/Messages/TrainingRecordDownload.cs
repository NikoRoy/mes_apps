using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Training.Messages
{
    [Serializable, XmlType("Message")]
    public class TrainingRecordDownload : IEquatable<TrainingRecordDownload>, IMessage
    {
        [XmlIgnore]
        public const string TransactionType = "TrainingRequirement";

        [XmlElement("TransactionType", Order = 1)]
        public XmlCDataSection TransactionTypeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionType);
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public Guid TransactionID
        {
            get;
            private set;
        }
        [XmlElement("TransactionID", Order = 2)]
        public XmlCDataSection TransactionIDCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionID.ToString());
            }
            set { /*Setter Intentionally Unused*/ }
        }

        [XmlIgnore]
        public DateTimeOffset TransactionDateTime
        {
            get;
            private set;
        }

        [XmlElement("TransactionDateTime", Order = 3)]
        public XmlCDataSection TransactionDateTimeCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TransactionDateTime.ToString("o"));
            }
            set { /*Setter Intentionally Unused*/ }
        }
        [XmlIgnore]
        public string EmployeeName
        {
            get;
            private set;
        }
        [XmlElement("EmployeeName", Order = 4)]
        public XmlCDataSection EmployeeNameCData
        {
            get
            {               
                return new XmlDocument().CreateCDataSection(EmployeeName);
            }
            set { }
        }
        [XmlIgnore]
        public string TrainingRequirementName
        {
            get;
            private set;
        }
        [XmlElement("TrainingRequirementName",Order = 5)]
        public XmlCDataSection TrainingRequirementNameCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementName);
            }
            set { }
        }
        [XmlIgnore]
        public string TrainingRequirementRevision
        {
            get;
            private set;
        }
        [XmlElement("TrainingRequirementRevision", Order = 6)]
        public XmlCDataSection TrainingRequirementRevisionCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRequirementRevision);
            }
            set { }
        }
        [XmlIgnore]
        public string TrainingRecordStatus
        {
            get;
            private set;
        }
        [XmlElement("TrainingRecordStatus",Order = 7)]
        public XmlCDataSection TrainingRecordStatusCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(TrainingRecordStatus);
            }
            set { }
        }
        [XmlIgnore]
        public string ESignature
        {

            get;
            private set;
        }
        [XmlElement("ESigRequirement", Order = 8)]
        public XmlCDataSection ESignatureCData
        {
            get
            {
                return new XmlDocument().CreateCDataSection(ESignature);
            }
            set { }
        }

        [XmlIgnore]
        public bool Sync
        {
            get;
            private set;
        }
        [XmlIgnore]
        public DateTime LastModifiedDate
        {
            get;
            private set;
        }
        [XmlIgnore]
        public int ID
        {
            get;
            private set;
        }
        [XmlIgnore]
        public int SyncAttempt
        {
            get;
            private set;
        }

        public TrainingRecordDownload() { }
        public TrainingRecordDownload(string emp, string doc, string rev, string status, string esign, bool sync, DateTime lmd, int id, int attempt)
        {
            try
            {
                this.TransactionID = Guid.NewGuid();
                this.TransactionDateTime = DateTimeOffset.Now;

                if (emp.Contains("@"))
                {
                    var a = GetUserPrincipalByEmailAddress(emp);
                    emp = a.SamAccountName;
                }

                this.EmployeeName = emp;
                this.TrainingRequirementName = doc;
                this.TrainingRequirementRevision = rev;
                this.TrainingRecordStatus = status;
                this.ESignature = esign;
                this.Sync = sync;
                this.LastModifiedDate = lmd;
                this.ID = id;
                this.SyncAttempt = attempt;
            }
            catch (Exception ex)
            {

            }
            
            
        }
        public string ToXml(bool excludeNameSpace = true, bool excludeDeclaration = true)
        {
            //Xml Namespace - remove from Message node
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add(string.Empty, string.Empty);

            XmlWriterSettings xset = new XmlWriterSettings();
            xset.Indent = true;
            xset.OmitXmlDeclaration = excludeDeclaration;

            XmlSerializer serializer = new XmlSerializer(typeof(TrainingRecordDownload));

            using (StringWriter stream = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, xset))
                {

                    serializer.Serialize(writer, this, excludeNameSpace ? emptyNs : null);

                    return stream.ToString();
                }
            }
        }
        private UserPrincipal GetUserPrincipalByEmailAddress(string emailAddress)
        {
            //Create a user serach prototype in the current domain
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = new UserPrincipal(ctx);

            user.EmailAddress = emailAddress;

            //Perform the search
            PrincipalSearcher searcher = new PrincipalSearcher(user);
            var results = searcher.FindAll();

            //Only return exact matches - there might be more than one John Smith
            if (results.Count() == 1)
            {
                return (UserPrincipal)results.First();
            }

            return null;
        }
        private UserPrincipal GetUserPrincipal(string accountName)
        {
            //Create a user serach prototype in the current domain
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = new UserPrincipal(ctx);

            //Strip any domain part out of the username
            user.SamAccountName = accountName.Split('\\')[1];
            //Perform the search
            PrincipalSearcher searcher = new PrincipalSearcher(user);
            var results = searcher.FindAll();

            //Only return exact matches - there might be more than one John Smith
            if (results.Count() == 1)
            {
                return (UserPrincipal)results.First();
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TrainingRecordDownload);
        }

        public bool Equals(TrainingRecordDownload download)
        {           
            return download != null &&
                   EmployeeName == download.EmployeeName &&
                   TrainingRequirementName == download.TrainingRequirementName &&
                   TrainingRequirementRevision == download.TrainingRequirementRevision;
            //&& TrainingRecordStatus == download.TrainingRecordStatus;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -47;
                hashCode = hashCode * -17 + EqualityComparer<string>.Default.GetHashCode(EmployeeName);
                hashCode = hashCode * -17 + EqualityComparer<string>.Default.GetHashCode(TrainingRequirementName);
                hashCode = hashCode * -17 + EqualityComparer<string>.Default.GetHashCode(TrainingRequirementRevision);
                //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrainingRecordStatus);
                return hashCode;
            }
            
        }

    }
}
