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
    
    public partial class FIRM_MISSIONS_DET
    {
        public decimal FIRM_MISSIONS_DET_ID { get; set; }
        public string FIRM_CODE { get; set; }
        public string FIN_YEAR { get; set; }
        public short TRAINING_PERIOD_ID { get; set; }
        public short MISSION_ID { get; set; }
        public Nullable<long> OFF_SKELETON_OFFICERS_ID { get; set; }
        public Nullable<decimal> OFF_ABS_STEPS_ID { get; set; }
        public Nullable<decimal> OFF_ABS_GROUP_ID { get; set; }
        public string NOTES { get; set; }
        public Nullable<short> DECTION { get; set; }
        public string PERSON_DATE_OWEN { get; set; }
        public string PERSON_CODE { get; set; }
        public Nullable<System.DateTime> ACT_TO_DATE { get; set; }
    
        public virtual OFF_ABS_STEPS OFF_ABS_STEPS { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual FIRM_MISSIONS_MEMBERS FIRM_MISSIONS_MEMBERS { get; set; }
    }
}
