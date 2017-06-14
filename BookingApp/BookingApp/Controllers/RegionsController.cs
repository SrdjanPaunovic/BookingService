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
    [RoutePrefix("region")]
    public class RegionsController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("regions", Name = "RegionApi")]
        public IQueryable<Region> GetRegions()
        {
            return db.Regions;
        }

        [HttpGet]
        [Route("region/{id}")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult GetRegion(int id)
        {
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        [HttpPut]
        [Route("region/{id}")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult PutRegion(int id, Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != region.Id)
            {
                return BadRequest();
            }

            if (db.Regions.Any(x => (x.Name == region.Name) && (x.Id != region.Id)))
            {

                return BadRequest("Name must be unique");
            }

            db.Entry(region).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
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
        [Route("region")]
        public IHttpActionResult PostRegion(Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Regions.Any(x => (x.Name == region.Name)))
            {

                return BadRequest("Name must be unique");
            }
            db.Regions.Add(region);
            db.SaveChanges();

            return CreatedAtRoute("RegionApi", new { id = region.Id }, region);
        }

        [HttpDelete]
        [Route("region/{id}")]
        [ResponseType(typeof(Region))]
        public IHttpActionResult DeleteRegion(int id)
        {
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }

            db.Regions.Remove(region);
            db.SaveChanges();

            return Ok(region);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegionExists(int id)
        {
            return db.Regions.Count(e => e.Id == id) > 0;
        }
    }
}