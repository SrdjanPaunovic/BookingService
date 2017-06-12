using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        
        [MaxLength(100)]
        public string UserName { get; set; }

        public bool IsForbidden { get; set; }
    }
}
