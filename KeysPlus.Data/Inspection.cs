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
    
    public partial class Inspection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inspection()
        {
            this.InspectionMedia = new HashSet<InspectionMedia>();
        }
    
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int StatusId { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<bool> IsViewed { get; set; }
        public bool IsActive { get; set; }
        public int RequestId { get; set; }
        public string Reason { get; set; }
        public int PercentDone { get; set; }
        public Nullable<bool> OwnerUpdate { get; set; }
    
        public virtual InspectionStatus InspectionStatus { get; set; }
        public virtual Property Property { get; set; }
        public virtual PropertyRequest PropertyRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InspectionMedia> InspectionMedia { get; set; }
    }
}
