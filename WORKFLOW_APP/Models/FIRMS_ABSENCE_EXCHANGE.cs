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
    
    public partial class FIRMS_ABSENCE_EXCHANGE
    {
        public string FIRM_CODE { get; set; }
        public string FROM_PERSON_CODE { get; set; }
        public string TO_PERSON_CODE { get; set; }
        public short ABSENCE_TYPE_ID { get; set; }
        public System.DateTime FROM_DATE { get; set; }
        public Nullable<short> FROM_RANK_ID { get; set; }
        public Nullable<short> FROM_RANK_CAT_ID { get; set; }
        public Nullable<short> FROM_PERSON_CAT_ID { get; set; }
        public Nullable<short> TO_RANK_ID { get; set; }
        public Nullable<short> TO_RANK_CAT_ID { get; set; }
        public Nullable<short> TO_PERSON_CAT_ID { get; set; }
        public Nullable<System.DateTime> EXCHANGE_DATE { get; set; }
        public Nullable<System.DateTime> TO_DATE { get; set; }
        public string OPENION1 { get; set; }
        public string SEC_COMMAND_OPENION { get; set; }
        public string COMMAND_DECISION { get; set; }
        public Nullable<short> IS_APPROVED { get; set; }
        public string APPROVAL_NO { get; set; }
        public Nullable<System.DateTime> APPROVAL_DATE { get; set; }
        public Nullable<short> OTHER_PER_DECS { get; set; }
        public Nullable<short> PLANNING_DECESION { get; set; }
        public string PLANNING_NOTES { get; set; }
        public Nullable<short> VICE_COMMAND_DECESION { get; set; }
        public string VICE_COMMAND_NOTES { get; set; }
        public Nullable<decimal> SEQ { get; set; }
    
        public virtual ABSENCE_TYPES ABSENCE_TYPES { get; set; }
        public virtual FIRMS FIRMS { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual PERSON_DATA PERSON_DATA1 { get; set; }
        public virtual RANKS RANKS { get; set; }
        public virtual RANKS RANKS1 { get; set; }
    }
}
