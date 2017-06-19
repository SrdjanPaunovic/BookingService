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
using BookingApp.BindingModels;

namespace BookingApp.Controllers
{

    [RoutePrefix("comment")]
    public class CommentsController : ApiController
    {
        private BAContext db = new BAContext();


        [HttpGet]
        [Route("comments", Name = "CommentApi")]
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        [HttpGet]
        [Route("comment/acc/{id}")]
        public IQueryable<Comment> GetCommentForAccomodation(int id)
        {
            return db.Comments.Where(x=>x.Accomodation_Id == id);
        }
             

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        [Route("comment")]
        public IHttpActionResult PostComment(CommentBindingModel bindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser appUser = db.AppUsers.FirstOrDefault(x => x.UserName == bindingModel.Username);
            Comment comment = new Comment()
            {
                AppUser_Id = appUser.Id,
                Text = bindingModel.Text,
                Rate = bindingModel.Rate,
                Accomodation_Id = bindingModel.Accomodation_Id
            };

            Accomodation acc = this.db.Accomodations.FirstOrDefault(x => x.Id == bindingModel.Accomodation_Id);
            
            db.Comments.Add(comment);
            db.SaveChanges();

            if (acc != null)
            {
                var commList = this.db.Comments.Where(x => x.Accomodation_Id == acc.Id);
                double sum = 0;
                foreach (var com in commList)
                {
                    sum += com.Rate;
                }

                acc.AverageGrade = sum / commList.Count();
                this.db.Entry(acc).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            return CreatedAtRoute("CommentApi", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }
    }
}