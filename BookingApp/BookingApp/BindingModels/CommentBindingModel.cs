using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingApp.BindingModels
{
    public class CommentBindingModel
    {
        public string Text { get; set; }

        public int Rate { get; set; }

        public int Accomodation_Id { get; set; }

        public string Username { get; set; }

    }
}