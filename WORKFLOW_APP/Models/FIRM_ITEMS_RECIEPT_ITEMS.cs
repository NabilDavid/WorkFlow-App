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
    
    public partial class FIRM_ITEMS_RECIEPT_ITEMS
    {
        public FIRM_ITEMS_RECIEPT_ITEMS()
        {
            this.FIRM_ITEMS_RECIEPT_SERIALS = new HashSet<FIRM_ITEMS_RECIEPT_SERIALS>();
        }
    
        public string FIRM_CODE { get; set; }
        public short RECIEPT_NO { get; set; }
        public short DEPARTMENT_ID { get; set; }
        public string ITEM_CODE { get; set; }
        public Nullable<decimal> QTY { get; set; }
        public string EXISTANCE_PLACE { get; set; }
        public string NOTES { get; set; }
    
        public virtual FIRM_ITEMS FIRM_ITEMS { get; set; }
        public virtual FIRM_ITEMS_RECIEPTS FIRM_ITEMS_RECIEPTS { get; set; }
        public virtual ICollection<FIRM_ITEMS_RECIEPT_SERIALS> FIRM_ITEMS_RECIEPT_SERIALS { get; set; }
    }
}