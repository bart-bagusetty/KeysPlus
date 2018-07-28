//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KeysPlus.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyRepayment
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime StartDate { get; set; }
        public int FrequencyType { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual Property Property { get; set; }
        public virtual TargetRentType TargetRentType { get; set; }
    }
}
