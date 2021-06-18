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
    
    public partial class FIRM_FUEL_REQUESTS
    {
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string FIN_YEAR { get; set; }
        public short PURPOSE_CODE { get; set; }
        public System.DateTime ST_DATE { get; set; }
        public Nullable<System.DateTime> END_DATE { get; set; }
        public Nullable<decimal> NEEDED_QTY { get; set; }
        public Nullable<decimal> EXEC_QTY { get; set; }
        public string FUEL_ITEM_CODE { get; set; }
        public Nullable<decimal> FUEL_PRICE { get; set; }
        public short DIRECTION_CODE { get; set; }
        public short USAGE_GROUP_CODE { get; set; }
    
        public virtual FIRMS FIRMS { get; set; }
        public virtual TRAINING_YEARS TRAINING_YEARS { get; set; }
    }
}