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
    
    public partial class tblServiceBusDtlWorkOrder
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public Nullable<int> WorkOrderRowID { get; set; }
        public string M_EHRTName { get; set; }
        public string M_EHHistoryID { get; set; }
        public string M_EHEVID { get; set; }
        public string EHStateName { get; set; }
        public Nullable<System.DateTime> M_EHDueDate { get; set; }
        public Nullable<System.DateTime> EHLastModified { get; set; }
        public Nullable<System.DateTime> M_EH2_UDF15 { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
