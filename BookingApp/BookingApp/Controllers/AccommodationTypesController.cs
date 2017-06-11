using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookingApp.Models;
using  BookingApp.ViewModels;
namespace BookingApp.Controllers
{
    [RoutePrefix("api/AccommodationType")]
    public class AccommodationTypesController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/AccommodationTypes

        [AllowAnonymous]
        [Route("GetTypes")]
        [HttpPost]
        public async Task<AccomTypeViewModel> GetAccommodationTypes()
        {


            return new AccomTypeViewModel(db.AccommodationTypes);
        }

        

        [AllowAnonymous]
        [Route("AddType")]
        [HttpPost]

        public async Task<IHttpActionResult> PutAccommodationType(AccommodationType accommodationType)
        {
            AccommodationType accommType = new AccommodationType();
            accommType.Name = accommodationType.Name;

            db.Entry(accommodationType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationTypeExists(accommodationType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

     

        private bool AccommodationTypeExists(int id)
        {
            return db.AccommodationTypes.Count(e => e.Id == id) > 0;
        }
    }
}