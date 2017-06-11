using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingApp.BindingModels
{
    public class AccomodationBindingModel
        
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string Address { get; set; }

        public double AverageGrade { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public string Approved { get; set; }

        public string ImageURL { get; set; }

        public string Username { get; set; } // po njemu trazimo vlasnika i setujemno ID


    }
}