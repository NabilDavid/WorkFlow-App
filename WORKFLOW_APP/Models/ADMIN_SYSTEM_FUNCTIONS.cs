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
    
    public partial class ADMIN_SYSTEM_FUNCTIONS
    {
        public ADMIN_SYSTEM_FUNCTIONS()
        {
            this.ADMIN_GROUPS_SYSTEM_FUNCTIONS = new HashSet<ADMIN_GROUPS_SYSTEM_FUNCTIONS>();
            this.ADMIN_SYSTEM_FUNCTIONS1 = new HashSet<ADMIN_SYSTEM_FUNCTIONS>();
        }
    
        public short FUNCTION_ID { get; set; }
        public Nullable<short> SYSTEM_DEPT_ID { get; set; }
        public Nullable<short> PARENT_FUNCTION_ID { get; set; }
        public string FUNCTION_ENAME { get; set; }
        public string FUNCTION_ANAME { get; set; }
        public Nullable<short> IS_AVAILABLE { get; set; }
        public Nullable<short> IS_FOLDER { get; set; }
        public string WINDOW_NAME { get; set; }
        public string WINDOW_PARMS { get; set; }
        public Nullable<short> SINGLE_FIRM { get; set; }
        public Nullable<short> CHILD_FIRM { get; set; }
        public Nullable<short> PARENT_FIRM { get; set; }
        public Nullable<short> DIVISION_FIRM { get; set; }
        public Nullable<short> REGION_FIRM { get; set; }
        public Nullable<short> DEPARTMENT_FIRM { get; set; }
        public Nullable<short> DISPLAY_ORDER { get; set; }
        public Nullable<decimal> APP_FLAG { get; set; }
    
        public virtual ICollection<ADMIN_GROUPS_SYSTEM_FUNCTIONS> ADMIN_GROUPS_SYSTEM_FUNCTIONS { get; set; }
        public virtual ICollection<ADMIN_SYSTEM_FUNCTIONS> ADMIN_SYSTEM_FUNCTIONS1 { get; set; }
        public virtual ADMIN_SYSTEM_FUNCTIONS ADMIN_SYSTEM_FUNCTIONS2 { get; set; }
    }
}
