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
    
    public partial class tblMEStoOracleInventoryLog
    {
        public int ID { get; set; }
        public string TransactionID { get; set; }
        public System.DateTimeOffset TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Action { get; set; }
        public string XmlRequest { get; set; }
        public string XmlResponse { get; set; }
    }
}
