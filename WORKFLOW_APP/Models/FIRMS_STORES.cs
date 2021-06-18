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
    
    public partial class FIRMS_STORES
    {
        public FIRMS_STORES()
        {
            this.FIRM_ITEMS_TRANSACTIONS = new HashSet<FIRM_ITEMS_TRANSACTIONS>();
            this.FIRMS_STORES_ITEMS = new HashSet<FIRMS_STORES_ITEMS>();
        }
    
        public short STORE_ID { get; set; }
        public string FIRM_CODE { get; set; }
        public Nullable<short> ST_TYPE_CODE { get; set; }
        public Nullable<decimal> STORE_LENGTH { get; set; }
        public Nullable<decimal> STORE_WIDTH { get; set; }
        public Nullable<decimal> STORE_X { get; set; }
        public Nullable<decimal> STORE_Y { get; set; }
    
        public virtual ICollection<FIRM_ITEMS_TRANSACTIONS> FIRM_ITEMS_TRANSACTIONS { get; set; }
        public virtual FIRMS FIRMS { get; set; }
        public virtual ICollection<FIRMS_STORES_ITEMS> FIRMS_STORES_ITEMS { get; set; }
    }
}
