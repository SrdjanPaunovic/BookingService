
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using TypeLite;
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            this.RoomReseravtions = new HashSet<RoomReseravtion>();
        }
    
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedCount { get; set; }
        public string Description { get; set; }
        public double PricePerNight { get; set; }

        [ForeignKey("Accomodation")]
        public int Accomodation_Id { get; set; }
        public virtual Accomodation Accomodation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomReseravtion> RoomReseravtions { get; set; }
    }
}
