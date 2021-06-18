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
    
    public partial class PERSON_UNIV_STUDIES
    {
        public int SEQ { get; set; }
        public string PERSON_CODE { get; set; }
        public string SITE_CODE { get; set; }
        public string FIRM_CODE { get; set; }
        public Nullable<short> RANK_ID { get; set; }
        public Nullable<short> RANK_CAT_ID { get; set; }
        public Nullable<short> PERSON_CAT_ID { get; set; }
        public Nullable<System.DateTime> STUDY_FROM { get; set; }
        public Nullable<System.DateTime> STUDY_TO { get; set; }
        public Nullable<short> STUDY_TYPE { get; set; }
        public Nullable<short> RELEASE_TYPE { get; set; }
        public string APPROVAL_NO { get; set; }
        public Nullable<System.DateTime> APPROVAL_DATE { get; set; }
        public Nullable<short> IS_PLANNED { get; set; }
        public string STUDY_SUBJECT { get; set; }
        public string NOTES { get; set; }
    
        public virtual FIRMS FIRMS { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual RANKS RANKS { get; set; }
    }
}
