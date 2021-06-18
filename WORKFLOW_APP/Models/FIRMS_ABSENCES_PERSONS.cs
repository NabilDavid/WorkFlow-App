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
    
    public partial class FIRMS_ABSENCES_PERSONS
    {
        public FIRMS_ABSENCES_PERSONS()
        {
            this.FIRMS_ABSENCES_PERSONS_DET = new HashSet<FIRMS_ABSENCES_PERSONS_DET>();
        }
    
        public string PERSON_CODE { get; set; }
        public string FIN_YEAR { get; set; }
        public short TRAINING_PERIOD_ID { get; set; }
        public string FIRM_CODE { get; set; }
        public System.DateTime FROM_DATE { get; set; }
        public short ABSENCE_TYPE_ID { get; set; }
        public short RANK_CAT_ID { get; set; }
        public short PERSON_CAT_ID { get; set; }
        public short COMMANDER_FLAG { get; set; }
        public string ABSENCE_NOTES { get; set; }
        public Nullable<System.DateTime> TO_DATE { get; set; }
        public Nullable<short> ABSENCE_STATUS { get; set; }
        public Nullable<System.DateTime> FORCE_DELETE_DATE { get; set; }
        public string ESCAPE_ORDER_NO { get; set; }
        public string RETURN_ORDER_NO { get; set; }
        public Nullable<short> DAY_STATUS { get; set; }
        public Nullable<System.DateTime> ACT_DATE { get; set; }
        public Nullable<decimal> ABS_REF { get; set; }
    
        public virtual ICollection<FIRMS_ABSENCES_PERSONS_DET> FIRMS_ABSENCES_PERSONS_DET { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
    }
}
