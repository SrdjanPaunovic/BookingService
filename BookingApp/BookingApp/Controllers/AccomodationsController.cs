
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
#region usings
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookingApp.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using BookingApp.Models;
using BookingApp.Providers;
using BookingApp.Results;
using System.Security.Principal;
#endregion
namespace BookingApp.Controllers
{
    using System.IO;
    using System.Net.Http.Headers;
    using System.Web;

    using BookingApp.Hubs;

    [Authorize]
    [RoutePrefix("accommodation")]
    public class AccomodationsController : ApiController
    {
        private BAContext db = new BAContext();

        private ApplicationUserManager _userManager;

        public const string ServerLocalHost = "http://localhost:54042";

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
        public IHttpActionResult GetAccomodation(int id)
        {
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return NotFound();
            }

            return Ok(accomodation);
        }

        // GET: api/Accomodations/5
        [HttpGet]
        [Route("acc_for_user/{username}")]
        [ResponseType(typeof(Accomodation))]
        public IHttpActionResult GetAccomodationForUser(string username)
        {

            List<Accomodation> accomodations = db.Accomodations.Where(x => x.AppUser.UserName == username).ToList();

            if (accomodations == null)
            {
                return NotFound();
            }

            return this.Ok(accomodations);
        }

        // PUT: api/Accomodations/5
        [HttpPut]
        [Route("accommodation/{id}")]
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

        [HttpPut]
        [Route("approve/{id}")]
        [AllowAnonymous]
        public IHttpActionResult ApproveAccomodation(int id)
        {

            var accomodation = db.Accomodations.FirstOrDefault(x => x.Id == id);

            if (accomodation == null)
            {
                return this.NotFound();
            }

            accomodation.Approved = true;

            db.Entry(accomodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                AppUser appUser = this.db.AppUsers.FirstOrDefault(x => x.Id == accomodation.AppUser_Id);
                if (appUser != null)
                {
                    NotificationHub.NotifyManager(accomodation, appUser.UserName);
                }

                //   NotificationHub.NotifyManager(this.db.Accomodations.Where(x => x.Approved).ToList());
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

            var username = User.Identity.GetUserName();
            var user = UserManager.FindByName(username);
            int userId = user.appUserId;
            accomodation.Approved = false;
            accomodation.AppUser_Id = userId;

            db.Accomodations.Add(accomodation);
            db.SaveChanges();
            return CreatedAtRoute("AccommodationApi", new { id = accomodation.Id }, accomodation);
        }

        [HttpPost]
        [Route("image/upload/{id}")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var myUniqueFileName = Guid.NewGuid().ToString().Substring(0, 8);
                            var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + myUniqueFileName + extension);
                            var relativePath = ServerLocalHost + "/Content/Images/" + myUniqueFileName + extension;

                            Accomodation acc = this.db.Accomodations.FirstOrDefault(x => x.Id == id);

                            if (acc != null)
                            {
                                acc.ImageURLs += "#" + relativePath;
                                db.Entry(acc).State = EntityState.Modified;
                                this.db.SaveChanges();
                                postedFile.SaveAs(filePath);
                            }

                        }
                    }
                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpGet]
        [Route("images/{id}")]
        public ICollection<string> GetImages(int id)
        {

            Accomodation acc = this.db.Accomodations.FirstOrDefault(x => x.Id == id);
            if (acc.ImageURLs == null)
            {
                return null;
            }
            var filePaths = acc.ImageURLs.Split('#');

            List<string> retList = new List<string>();

            foreach (var filePath in filePaths)
            {

                var fullfilePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileName(filePath));
                if (File.Exists(fullfilePath))
                {
                    retList.Add(filePath);
                }
            }

            return retList;

            /*//S2:Read File as Byte Array
            byte[] fileData = File.ReadAllBytes(filePath);

            if (fileData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            //S3:Set Response contents and MediaTypeHeaderValue
            */
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
            var rooms = this.db.Rooms.Where(x => x.Accomodation_Id == id);

            foreach (var room in rooms)
            {
                this.db.Entry(room).State = EntityState.Deleted;
            }

            this.db.SaveChanges();
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