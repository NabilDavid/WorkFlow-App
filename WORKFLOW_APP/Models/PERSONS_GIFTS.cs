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
    
    public partial class PERSONS_GIFTS
    {
        public string FIRM_CODE { get; set; }
        public string PERSON_CODE { get; set; }
        public short GIFT_TYPE_ID { get; set; }
        public string GIFT { get; set; }
        public System.DateTime GIFT_DATE { get; set; }
        public Nullable<short> RANK_ID { get; set; }
        public Nullable<short> RANK_CAT_ID { get; set; }
        public Nullable<short> PERSON_CAT_ID { get; set; }
        public string GIFT_REASON { get; set; }
        public string NOTES { get; set; }
    
        public virtual FIRMS FIRMS { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
        public virtual PERSONS_GIFTS_TYPES PERSONS_GIFTS_TYPES { get; set; }
        public virtual RANKS RANKS { get; set; }
    }
}
