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
    
    public partial class FIRM_ITEMS_TRANSACTION_SERIALS
    {
        public FIRM_ITEMS_TRANSACTION_SERIALS()
        {
            this.FIRM_ITEMS_TRANS_CONT_ITEMS = new HashSet<FIRM_ITEMS_TRANS_CONT_ITEMS>();
        }
    
        public string FIRM_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string DOC_NO { get; set; }
        public short DOC_TYPE_ID { get; set; }
        public short DEPARTMENT_ID { get; set; }
        public string FIN_YEAR { get; set; }
        public string SERIAL_NO { get; set; }
        public short DELIVERY_STATUS_CODE { get; set; }
        public Nullable<short> IS_PROCESSED { get; set; }
    
        public virtual ICollection<FIRM_ITEMS_TRANS_CONT_ITEMS> FIRM_ITEMS_TRANS_CONT_ITEMS { get; set; }
        public virtual FIRM_ITEMS_TRANSACTIONS FIRM_ITEMS_TRANSACTIONS { get; set; }
    }
}