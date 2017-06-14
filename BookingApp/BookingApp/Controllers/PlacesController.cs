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

namespace BookingApp.Controllers
{
    [RoutePrefix("place")]
    public class PlacesController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("places", Name = "PlaceApi")]
        public IQueryable<Place> GetPlaces()
        {
            return db.Places;
        }


        [HttpGet]
        [Route("place/{id}")]
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> GetPlace(int id)
        {
            Place place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        [HttpPut]
        [Route("place/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.Id)
            {
                return BadRequest();
            }

            if (db.Places.Any(x => (x.Name == place.Name) && (x.Id != place.Id)))
            {

                return BadRequest("Name must be unique");
            }

            place.Region = null; //jer ce kreirati jos jednog jer nije ista instanca Region kao u bazi,
                                 //dovoljno je sto ima id

            db.Entry(place).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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

        [HttpPost]
        [Route("place")]
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Places.Any(x => (x.Name == place.Name)))
            {

                return BadRequest("Name must be unique");
            }

            place.Region = null; //jer ce kreirati jos jednog jer nije ista instanca Region kao u bazi,
                                 //dovoljno je sto ima id

            db.Places.Add(place);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PlaceApi", new { id = place.Id }, place);
        }

        [HttpDelete]
        [Route("place/{id}")]
        [ResponseType(typeof(Place))]
        public async Task<IHttpActionResult> DeletePlace(int id)
        {
            Place place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            db.Places.Remove(place);
            await db.SaveChangesAsync();

            return Ok(place);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaceExists(int id)
        {
            return db.Places.Count(e => e.Id == id) > 0;
        }
    }
}