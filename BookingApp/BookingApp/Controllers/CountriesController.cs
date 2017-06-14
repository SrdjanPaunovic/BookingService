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
    [RoutePrefix("country")]
    public class CountriesController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("countries", Name = "CountryApi")]
        public IQueryable<Country> GetCountries()
        {
            var l = this.db.Countries.ToList();
            return db.Countries;
        }

        [HttpGet]
        [Route("countries/{id}")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult GetCountry(int id)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }


        [HttpPut]
        [Route("country/{id}")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult PutCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != country.Id)
            {
                return BadRequest();
            }

            if (db.Countries.Any(x => (x.Name == country.Name) && (x.Id != country.Id)))
            {
                
                return BadRequest("Name must be unique");
            }

            db.Entry(country).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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
        [Route("country")]
        public IHttpActionResult PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(db.Countries.Any(x => x.Name == country.Name))
            {
                return BadRequest("Name must be unique");
            }
            db.Countries.Add(country);
            db.SaveChanges();
            var v = CreatedAtRoute("CountryApi", new { id = country.Id }, country);
            return v;
        }

        [HttpDelete]
        [Route("country/{id}")]
        public IHttpActionResult DeleteCountry(int id)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            db.Countries.Remove(country);
            db.SaveChanges();

            return Ok(country);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountryExists(int id)
        {
            return db.Countries.Count(e => e.Id == id) > 0;
        }
    }
}