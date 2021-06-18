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
    
    public partial class FIRM_ITEMS_DOCUMENTS
    {
        public FIRM_ITEMS_DOCUMENTS()
        {
            this.FIRM_ITEMS_TRANSACTIONS = new HashSet<FIRM_ITEMS_TRANSACTIONS>();
        }
    
        public short DEPARTMENT_ID { get; set; }
        public string FIRM_CODE { get; set; }
        public short DOC_TYPE_ID { get; set; }
        public string FIN_YEAR { get; set; }
        public string DOC_NO { get; set; }
        public Nullable<System.DateTime> DOC_DATE { get; set; }
        public string OTHER_FIRM_CODE { get; set; }
        public string RESPONSIBLE_CODE { get; set; }
        public Nullable<short> IS_PROCESSED { get; set; }
        public string ORDER_NO { get; set; }
        public Nullable<System.DateTime> ORDER_DATE { get; set; }
        public string OTHER_ORDER_NO { get; set; }
        public Nullable<System.DateTime> OTHER_ORDER_DATE { get; set; }
    
        public virtual FINANCIAL_YEAR FINANCIAL_YEAR { get; set; }
        public virtual FIRMS FIRMS { get; set; }
        public virtual FIRMS FIRMS1 { get; set; }
        public virtual ICollection<FIRM_ITEMS_TRANSACTIONS> FIRM_ITEMS_TRANSACTIONS { get; set; }
        public virtual PERSON_DATA PERSON_DATA { get; set; }
    }
}