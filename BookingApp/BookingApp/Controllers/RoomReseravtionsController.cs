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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace BookingApp.Controllers
{
    [Authorize]
    [RoutePrefix("reservation")]
    public class RoomReseravtionsController : ApiController
    {
        private BAContext db = new BAContext();
        private object locker = new object();


        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/RoomReseravtions
        [HttpGet]
        [Route("reservations", Name = "ReservationApi")]
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
        [HttpPost]
        [Route("reservation")]
        [ResponseType(typeof(RoomReseravtion))]
        public IHttpActionResult PostRoomReseravtion(RoomReseravtion roomReseravation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var username = User.Identity.GetUserName();
            var user = UserManager.FindByName(username);
            int userId = user.appUserId;           
            roomReseravation.AppUser_Id = userId;

            //TODO logika za provjeru
            roomReseravation.Timestamp = DateTime.Now;


            if (validateReservation(roomReseravation))
            {
                lock (locker)
                {
                    db.RoomReseravtions.Add(roomReseravation);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (RoomReseravtionExists(roomReseravation.Room_Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }


            }else
            {
                return Content(HttpStatusCode.BadRequest, "Room is busy at that time period");

            }



            return CreatedAtRoute("ReservationApi", new { id = roomReseravation.Id }, roomReseravation);
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

        public bool validateReservation(RoomReseravtion roomReservation)
        {
            if (roomReservation.StartDate > roomReservation.EndTime)
                return false;
            List<RoomReseravtion> reservationList = db.RoomReseravtions
                .Where(reservation =>reservation.Room_Id == roomReservation.Room_Id)
                .ToList();

            foreach(var reservation in reservationList)
            {
                if (Intersects(reservation, roomReservation))
                    return false;
            }
            
            return true;
        }


        public bool Intersects(RoomReseravtion reservation1,RoomReseravtion reservation2)
        {
            if (reservation1.StartDate > reservation1.EndTime || reservation2.StartDate > reservation2.EndTime)
                return false;

            if (reservation1.StartDate == reservation1.EndTime || reservation2.StartDate == reservation2.EndTime)
                return false; // No actual date range

            if (reservation1.StartDate == reservation2.StartDate || reservation1.EndTime == reservation2.EndTime)
                return true; // If any set is the same time, then by default there must be some overlap. 

            if (reservation1.StartDate < reservation2.StartDate)
            {
                if (reservation1.EndTime > reservation2.StartDate && reservation1.EndTime < reservation2.EndTime)
                    return true; // Condition 1

                if (reservation1.EndTime > reservation2.EndTime)
                    return true; // Condition 3
            }
            else
            {
                if (reservation2.EndTime > reservation1.StartDate && reservation2.EndTime < reservation1.EndTime)
                    return true; // Condition 2

                if (reservation2.EndTime > reservation1.EndTime)
                    return true; // Condition 4
            }

            return false;
        }

    }
}