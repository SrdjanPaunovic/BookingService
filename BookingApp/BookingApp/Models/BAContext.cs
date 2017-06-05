using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingApp.Models
{
    public class BAContext: IdentityDbContext<BAIdentityUser>
    {   
        public virtual DbSet<AppUser> AppUsers { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Accomodation> Accomodations { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<AccommodationType> AccommodationTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomReseravtion> RoomReseravtions { get; set; }

        public BAContext() : base("name=ContextDb")
        {            
        }

        public static BAContext Create()
        {
            return new BAContext();
        }
    }
}