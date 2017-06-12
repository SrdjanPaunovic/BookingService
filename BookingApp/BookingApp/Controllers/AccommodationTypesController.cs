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

        public async Task<IHttpActionResult> AddAccommodationType(AccommodationType entryType)
        {
            AccommodationType accommodationType = new AccommodationType();
            accommodationType.Name = entryType.Name;
            db.AccommodationTypes.Add(accommodationType);

            try
            {

                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.OK);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationTypeExists(accommodationType.Id))
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

     

        private bool AccommodationTypeExists(int id)
        {
            return db.AccommodationTypes.Count(e => e.Id == id) > 0;
        }
    }
}