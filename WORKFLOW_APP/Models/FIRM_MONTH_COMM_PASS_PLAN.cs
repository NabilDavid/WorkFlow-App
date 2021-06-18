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
    
    public partial class FIRM_MONTH_COMM_PASS_PLAN
    {
        public FIRM_MONTH_COMM_PASS_PLAN()
        {
            this.FIRM_MONTH_COMM_ITEMS_NOTES = new HashSet<FIRM_MONTH_COMM_ITEMS_NOTES>();
        }
    
        public string FIRM_CODE { get; set; }
        public string FIN_YEAR { get; set; }
        public short COMMITTEE_MONTH { get; set; }
        public short DEPARTMENT_ID { get; set; }
        public string OFF_PERSON_CODE { get; set; }
        public string SOL_PERSON_CODE { get; set; }
        public Nullable<System.DateTime> PLANNED_DATE { get; set; }
        public Nullable<System.DateTime> ACTUAL_DATE { get; set; }
        public string ADVANTAGE_NOTES { get; set; }
        public string DISADVANTAGE_NOTES { get; set; }
        public short COMMITTEE_TYPE { get; set; }
        public System.DateTime COMMITTEE_DATE { get; set; }
    
        public virtual ICollection<FIRM_MONTH_COMM_ITEMS_NOTES> FIRM_MONTH_COMM_ITEMS_NOTES { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual PERSON_DATA PERSON_DATA1 { get; set; }
        public virtual FIRM_MONTHLY_COMMITTEES FIRM_MONTHLY_COMMITTEES { get; set; }
    }
}