using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookingApp.Models;
using System.Threading.Tasks;
using BookingApp.BindingModels;
using Microsoft.AspNet.Identity;


namespace BookingApp.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Accommodation")]
    public class AccomodationsController : ApiController
    {
        private BAContext db = new BAContext();


      //  [AllowAnonymous]
        [Route("AddAccommodation")]
        [HttpPost]

        public async Task<IHttpActionResult> AddAccommodation(AccomodationBindingModel entryAccommodation)
        {
            string[] cleanName = entryAccommodation.Username.Split('\n');
            var owner1 = db.AppUsers.FirstOrDefault<AppUser>(a => a.UserName == "admin");
            var uname = cleanName[0];
            var owner = db.AppUsers.FirstOrDefault<AppUser>(a => a.UserName== uname);
            var list = db.AppUsers.ToList();
          //  var place = db.Places.FirstOrDefault<Place>(a => a.Name ==entryAccommodation.PlaceName);

            Accomodation accommodation = new Accomodation()
            {
                Name = entryAccommodation.Name,
                Description = entryAccommodation.Description,
                Address = entryAccommodation.Address,
                Latitude = entryAccommodation.Latitude,
                Longitude = entryAccommodation.Longitude,
                AppUser_Id = owner.Id,
                Place_Id = 6,// inicijalno 
                AccommodationType_Id=1 // inicijalno 

            };

            //var owner = from user in db.AppUsers
            //                           where user.FullName==entryAccommodation.Username
            //                           select user;
           /* var owner = db.AppUsers.FirstOrDefault<AppUser>(a => a.FullName == entryAccommodation.Username);
           accommodation.AppUser = owner;*/

            db.Accomodations.Add(accommodation);

            try
            {

                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.OK);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(accommodation.Id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                    throw;

                }
            }

        }

        private bool AccommodationExists(int id)
        {
            return db.Accomodations.Count(e => e.Id == id) > 0;
        }


    }
}