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
    
    public partial class PERSON_RANKING_ROUNDS
    {
        public short ROUND_ID { get; set; }
        public string ROUND_NAME { get; set; }
        public string FIN_YEAR { get; set; }
        public Nullable<short> RANK_ID { get; set; }
        public Nullable<short> RANK_CAT_ID { get; set; }
        public Nullable<short> PERSON_CAT_ID { get; set; }
        public Nullable<System.DateTime> FROM_DATE { get; set; }
        public Nullable<System.DateTime> TO_DATE { get; set; }
    
        public virtual TRAINING_YEARS TRAINING_YEARS { get; set; }
        public virtual RANKS RANKS { get; set; }
    }
}