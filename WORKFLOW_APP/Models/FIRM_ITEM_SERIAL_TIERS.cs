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
    
    public partial class FIRM_ITEM_SERIAL_TIERS
    {
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string CAR_SERIAL_NO { get; set; }
        public string TIER_ITEM_CODE { get; set; }
        public string TIER_SERIAL_NO { get; set; }
        public Nullable<short> ITEM_STATUS_CODE { get; set; }
        public Nullable<System.DateTime> DELIVERY_DATE { get; set; }
        public Nullable<System.DateTime> RETURN_DATE { get; set; }
        public Nullable<int> DELIVERY_COUNTER_READ { get; set; }
        public Nullable<short> IS_AVAILABLE { get; set; }
        public Nullable<int> PREVIOUS_KILOMETERS { get; set; }
    
        public virtual FIRM_ITEMS_SERIALS FIRM_ITEMS_SERIALS { get; set; }
        public virtual FIRM_ITEMS_SERIALS FIRM_ITEMS_SERIALS1 { get; set; }
    }
}
