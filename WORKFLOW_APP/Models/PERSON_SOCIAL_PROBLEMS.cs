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
    
    public partial class PERSON_SOCIAL_PROBLEMS
    {
        public string PERSON_CODE { get; set; }
        public string PROB_DESC { get; set; }
    
        public virtual PERSON_DATA PERSON_DATA { get; set; }
    }
}
