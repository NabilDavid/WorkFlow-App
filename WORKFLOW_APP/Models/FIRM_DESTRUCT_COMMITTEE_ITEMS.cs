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
    
    public partial class FIRM_DESTRUCT_COMMITTEE_ITEMS
    {
        public string FIRM_CODE { get; set; }
        public string ORDER_NO { get; set; }
        public System.DateTime ORDER_DATE { get; set; }
        public string ITEM_CODE { get; set; }
        public Nullable<decimal> QTY { get; set; }
        public string NOTES { get; set; }
        public string DELIVERY_ORDER_NO { get; set; }
        public Nullable<short> IS_PROCESSED { get; set; }
    
        public virtual FIRM_DESTRUCT_COMMITTEES FIRM_DESTRUCT_COMMITTEES { get; set; }
        public virtual FIRM_ITEMS FIRM_ITEMS { get; set; }
    }
}
