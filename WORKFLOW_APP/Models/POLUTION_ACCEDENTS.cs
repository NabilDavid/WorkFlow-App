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
    
    public partial class POLUTION_ACCEDENTS
    {
        public short ACCEDENT_ID { get; set; }
        public string FIN_YEAR { get; set; }
        public short TRAINING_PERIOD_ID { get; set; }
        public Nullable<System.DateTime> ACCEDENT_DATE { get; set; }
        public Nullable<short> POLUTION_TYPE { get; set; }
        public string ACCEDENT_PLACE { get; set; }
        public string POLUTION_SUBSTANCE { get; set; }
        public string ACTION_TAKEN { get; set; }
    
        public virtual TRAINING_PERIODS TRAINING_PERIODS { get; set; }
    }
}
