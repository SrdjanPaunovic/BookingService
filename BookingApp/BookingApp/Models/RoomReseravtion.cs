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
    public partial class RoomReseravtion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public System.DateTime? StartTime { get; set; }


        public System.DateTime? EndTime { get; set; }

        public System.DateTime? Timestamp { get; set; }

        [ForeignKey("Room")]
        public int Room_Id { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("AppUser")]
        public int AppUser_Id { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
