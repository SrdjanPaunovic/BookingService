using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingApp.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        public System.DateTime? StartTime { get; set; }


        public System.DateTime? EndTime { get; set; }

        

        public string AccommodationName { get; set; }

        public string RoomNumber { get; set; }
    }
}