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
    
    public partial class PERSON_SECRET_REPORTS
    {
        public string PERSON_CODE { get; set; }
        public System.DateTime FROM_DATE { get; set; }
        public System.DateTime TO_DATE { get; set; }
        public string DIRECT_OFFICER { get; set; }
        public Nullable<short> DIRECT_OFFICER_GRADE_ID { get; set; }
        public string APPROVING_OFFICER { get; set; }
        public Nullable<short> APPROVING_OFFICER_GRADE_ID { get; set; }
        public Nullable<short> RANK_ID { get; set; }
        public Nullable<short> RANK_CAT_ID { get; set; }
        public Nullable<short> PERSON_CAT_ID { get; set; }
        public Nullable<decimal> PERCENTAGE { get; set; }
    
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual RANKS RANKS { get; set; }
    }
}