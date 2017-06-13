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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TypeLite;
    public partial class Accomodation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accomodation()
        {
            this.Comments = new HashSet<Comment>();
            this.Rooms = new HashSet<Room>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        public string AverageGrade { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageURL { get; set; }
        public string Approved { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [ForeignKey("AccommodationType")]
        public int AccommodationType_Id { get; set; }
        public virtual AccommodationType AccommodationType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }


        [ForeignKey("AppUser")]
        public int AppUser_Id { get; set; }
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("Place")]
        public int Place_Id { get; set; }
        public virtual Place Place { get; set; }
    }
}
