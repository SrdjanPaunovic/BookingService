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

namespace BookingApp.Controllers
{
    [RoutePrefix("accommodation")]
    public class AccomodationsController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/Accomodations
        [HttpGet]
        [Route("accommodations", Name = "AccommodationApi")]
        public IQueryable<Accomodation> GetAccomodations()
        {
            return db.Accomodations;
        }

        // GET: api/Accomodations/5
        [HttpGet]
        [Route("accomodations/{id}")]
        [ResponseType(typeof(Accomodation))]
        [ResponseType(typeof(Accomodation))]
        public IHttpActionResult GetAccomodation(int id)
        {
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return NotFound();
            }

            return Ok(accomodation);
        }

        // PUT: api/Accomodations/5
        [HttpPut]
        [Route("accommodation/{id}")]
        [ResponseType(typeof(Country))]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccomodation(int id, Accomodation accomodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accomodation.Id)
            {
                return BadRequest();
            }

            db.Entry(accomodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccomodationExists(id))
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

        // POST: api/Accomodations
        [HttpPost]
        [Route("accommodation")]
        [ResponseType(typeof(Accomodation))]
        public IHttpActionResult PostAccomodation(Accomodation accomodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accomodations.Add(accomodation);
            db.SaveChanges();

            return CreatedAtRoute("AccommodationApi", new { id = accomodation.Id }, accomodation);
        }

        // DELETE: api/Accomodations/5
        [HttpDelete]
        [Route("accommodation/{id}")]
        [ResponseType(typeof(Accomodation))]
        public IHttpActionResult DeleteAccomodation(int id)
        {
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return NotFound();
            }

            db.Accomodations.Remove(accomodation);
            db.SaveChanges();

            return Ok(accomodation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccomodationExists(int id)
        {
            return db.Accomodations.Count(e => e.Id == id) > 0;
        }
    }
}