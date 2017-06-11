using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookingApp.ViewModels
{
    public class AccomTypeViewModel
    {
        public List<String> TypeNames { get; set; }

        public AccomTypeViewModel(DbSet<AccommodationType> names){

            TypeNames = new List<string>();
            foreach (var type in names)
            {
                TypeNames.Add(type.Name);
            }
        }
    }
}