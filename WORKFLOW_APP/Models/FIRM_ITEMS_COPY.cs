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
    
    public partial class FIRM_ITEMS_COPY
    {
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public Nullable<decimal> DEFAULT_QTY { get; set; }
        public Nullable<decimal> POLICY { get; set; }
        public Nullable<decimal> TOTAL_EXIST { get; set; }
        public Nullable<decimal> TOTAL_VALID { get; set; }
        public Nullable<decimal> DEFECTED_LOCAL { get; set; }
        public Nullable<decimal> DEFECTED_WORKSHOP { get; set; }
        public Nullable<decimal> TOTAL_STORED { get; set; }
        public string BOOKPAGE_NO { get; set; }
        public Nullable<decimal> STARTING_QTY { get; set; }
        public string STARTING_FIN_YEAR { get; set; }
        public Nullable<decimal> FROM_INVENTORIES { get; set; }
        public Nullable<decimal> TO_INVENTORIES { get; set; }
        public Nullable<decimal> FROM_OTHER_FIRMS { get; set; }
        public Nullable<decimal> TO_OTHER_FIRMS { get; set; }
        public Nullable<decimal> CONSUMED_QTY { get; set; }
        public Nullable<decimal> ADD_FROM_COMMITTEE { get; set; }
        public Nullable<decimal> REMOVE_FROM_COMMITTEE { get; set; }
    }
}
