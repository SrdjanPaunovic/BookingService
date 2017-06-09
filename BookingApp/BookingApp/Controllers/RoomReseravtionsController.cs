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
    public class RoomReseravtionsController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/RoomReseravtions
        public IQueryable<RoomReseravtion> GetRoomReseravtions()
        {
            return db.RoomReseravtions;
        }

        // GET: api/RoomReseravtions/5
        [ResponseType(typeof(RoomReseravtion))]
        public IHttpActionResult GetRoomReseravtion(int id)
        {
            RoomReseravtion roomReseravtion = db.RoomReseravtions.Find(id);
            if (roomReseravtion == null)
            {
                return NotFound();
            }

            return Ok(roomReseravtion);
        }

        // PUT: api/RoomReseravtions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoomReseravtion(int id, RoomReseravtion roomReseravtion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomReseravtion.Room_Id)
            {
                return BadRequest();
            }

            db.Entry(roomReseravtion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomReseravtionExists(id))
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

        // POST: api/RoomReseravtions
        [ResponseType(typeof(RoomReseravtion))]
        public IHttpActionResult PostRoomReseravtion(RoomReseravtion roomReseravtion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RoomReseravtions.Add(roomReseravtion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RoomReseravtionExists(roomReseravtion.Room_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = roomReseravtion.Room_Id }, roomReseravtion);
        }

        // DELETE: api/RoomReseravtions/5
        [ResponseType(typeof(RoomReseravtion))]
        public IHttpActionResult DeleteRoomReseravtion(int id)
        {
            RoomReseravtion roomReseravtion = db.RoomReseravtions.Find(id);
            if (roomReseravtion == null)
            {
                return NotFound();
            }

            db.RoomReseravtions.Remove(roomReseravtion);
            db.SaveChanges();

            return Ok(roomReseravtion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomReseravtionExists(int id)
        {
            return db.RoomReseravtions.Count(e => e.Room_Id == id) > 0;
        }
    }
}