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
    
    public partial class FIRMS_GARAGE_PLAN_ITEM_SER
    {
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string SERIAL_NO { get; set; }
        public long MONTH_ID { get; set; }
        public short WEEK_NO { get; set; }
        public string FIN_YEAR { get; set; }
    
        public virtual FIRM_ITEMS_SERIALS FIRM_ITEMS_SERIALS { get; set; }
        public virtual FIRMS_GARAGE_PLAN_ITEMS FIRMS_GARAGE_PLAN_ITEMS { get; set; }
    }
}