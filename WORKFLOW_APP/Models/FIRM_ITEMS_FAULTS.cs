//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WORKFLOW_APP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FIRM_ITEMS_FAULTS
    {
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string FIN_YEAR { get; set; }
        public short MAINTAINANCE_TYPE_ID { get; set; }
        public System.DateTime PLANNED_DATE { get; set; }
        public short FAULT_CATEGORY_ID { get; set; }
        public Nullable<int> PLANNED_QTY { get; set; }
        public Nullable<int> FINISHED_QTY { get; set; }
        public string ACTION_TAKEN { get; set; }
        public Nullable<short> SEQ { get; set; }
    
        public virtual FIRM_ITEMS_MAINTAINANCE FIRM_ITEMS_MAINTAINANCE { get; set; }
    }
}
