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
using BookingApp.ViewModels;

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



        [HttpGet]
        [Route("reservation_preview")]
        public IQueryable<ReservationViewModel> GetAdminRoomReseravtions()
        {
            var username = User.Identity.GetUserName();
            var user = UserManager.FindByName(username);
            var userRole = user.Roles.FirstOrDefault();
            var role = db.Roles.SingleOrDefault(r => r.Id == userRole.RoleId);
            int appUserId = user.appUserId;

            if (role.Name.Equals("Manager"))
            {
                return from a in db.RoomReseravtions
                       join b in db.Rooms on a.Room_Id equals b.Id
                       join c in db.Accomodations on b.Accomodation_Id equals c.Id
                       join d in db.AppUsers on c.AppUser_Id equals d.Id
                       where d.Id == appUserId
                       select new ReservationViewModel
                       {
                           Id = a.Id,
                           StartTime = a.StartTime,
                           EndTime = a.EndTime,
                           AccommodationName = c.Name,
                           RoomNumber = b.RoomNumber,
                           Username = a.AppUser.UserName
                       };
            }
            else if (role.Name.Equals("Admin"))
            {
                return from a in db.RoomReseravtions
                       join b in db.Rooms on a.Room_Id equals b.Id
                       join c in db.Accomodations on b.Accomodation_Id equals c.Id
                       join d in db.AppUsers on c.AppUser_Id equals d.Id
                       where d.Id == d.Id
                       select new ReservationViewModel
                       {
                           Id = a.Id,
                           StartTime = a.StartTime,
                           EndTime = a.EndTime,
                           AccommodationName = c.Name,
                           RoomNumber = b.RoomNumber,
                           Username = a.AppUser.UserName

                       };


            }
            else
            {

                return from a in db.RoomReseravtions
                       join b in db.Rooms on a.Room_Id equals b.Id
                       join c in db.Accomodations on b.Accomodation_Id equals c.Id
                       join d in db.AppUsers on c.AppUser_Id equals d.Id
                       where a.AppUser_Id == appUserId
                       select new ReservationViewModel
                       {
                           Id = a.Id,
                           StartTime = a.StartTime,
                           EndTime = a.EndTime,
                           AccommodationName = c.Name,
                           RoomNumber = b.RoomNumber,
                           Username = a.AppUser.UserName

                       };

            }


        }


        #region Methods
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


            }
            else
            {
                var roomRes = this.db.RoomReseravtions.Where(x => x.Room_Id == roomReseravation.Room_Id).ToList();


                string msg = "Room is busy at: ";

                foreach (var roomRe in roomRes)
                {
                    msg += "  [" + roomRe.StartTime.Value.ToShortDateString() + "  -  " + roomRe.EndTime.Value.ToShortDateString() + "]";

                }

                return Content(HttpStatusCode.BadRequest, msg);
            }



            return CreatedAtRoute("ReservationApi", new { id = roomReseravation.Id }, roomReseravation);
        }

        [HttpDelete]
        [Route("reservation/{id}")]
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
            if (roomReservation.StartTime > roomReservation.EndTime)
                return false;
            List<RoomReseravtion> reservationList = db.RoomReseravtions
                .Where(reservation => reservation.Room_Id == roomReservation.Room_Id)
                .ToList();

            foreach (var reservation in reservationList)
            {
                if (Intersects(reservation, roomReservation))
                    return false;
            }

            return true;
        }


        public bool Intersects(RoomReseravtion reservation1, RoomReseravtion reservation2)
        {
            if (reservation1.StartTime > reservation1.EndTime || reservation2.StartTime > reservation2.EndTime)
                return false;

            if (reservation1.StartTime == reservation1.EndTime || reservation2.StartTime == reservation2.EndTime)
                return false; // No actual date range

            if (reservation1.StartTime == reservation2.StartTime || reservation1.EndTime == reservation2.EndTime)
                return true; // If any set is the same time, then by default there must be some overlap. 

            if (reservation1.StartTime < reservation2.StartTime)
            {
                if (reservation1.EndTime > reservation2.StartTime && reservation1.EndTime < reservation2.EndTime)
                    return true; // Condition 1

                if (reservation1.EndTime > reservation2.EndTime)
                    return true; // Condition 3
            }
            else
            {
                if (reservation2.EndTime > reservation1.StartTime && reservation2.EndTime < reservation1.EndTime)
                    return true; // Condition 2

                if (reservation2.EndTime > reservation1.EndTime)
                    return true; // Condition 4
            }

            return false;
        }
        #endregion

    }
}