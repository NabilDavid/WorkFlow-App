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
    
    public partial class PERSON_JOBS
    {
        public short SEQ { get; set; }
        public string PERSON_CODE { get; set; }
        public Nullable<short> JOB_TYPE_ID { get; set; }
        public Nullable<short> RANK_ID { get; set; }
        public Nullable<short> RANK_CAT_ID { get; set; }
        public Nullable<short> PERSON_CAT_ID { get; set; }
        public string FIRM_CODE { get; set; }
        public Nullable<System.DateTime> FROM_DATE { get; set; }
        public Nullable<System.DateTime> TO_DATE { get; set; }
        public Nullable<short> JOB_FLAG { get; set; }
        public Nullable<decimal> APP_FLAG { get; set; }
    
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual RANKS RANKS { get; set; }
    }
}
