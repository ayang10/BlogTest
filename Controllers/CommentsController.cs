using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogTest.Models;

namespace BlogTest.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Editor).Include(c => c.ParentComment).Include(c => c.Post);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize] //this limit users, you need to be logged in
        public ActionResult Create(int id)
        {
            Comment model = new Comment { PostId = id };

            return View(model);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "PostId,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Created = new DateTimeOffset(DateTime.Now);
                comment.AuthorId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id; //grab the id of the current login user, assign that to the AuthorId
               
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { id = comment.PostId }); //redirect this create to Posts view Details page.
            }
            
            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize] //this limit users, you need to be logged in
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if(User.Identity.Name != comment.Author.UserName)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName", comment.EditorId);
            ViewBag.ParentCommentId = new SelectList(db.Comments, "Id", "AuthorId", comment.ParentCommentId);
            ViewBag.PostId = new SelectList(db.Posts, "Id", "title", comment.PostId);
            
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostId,AuthorId,EditorId,Body,Created,Updated,ParentCommentId,MarkForDeletion")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName", comment.EditorId);
            ViewBag.ParentCommentId = new SelectList(db.Comments, "Id", "AuthorId", comment.ParentCommentId);
            ViewBag.PostId = new SelectList(db.Posts, "Id", "title", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize (Roles = "Admin, Moderator")]
        public ActionResult Delete(int? id, int postId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.MarkForDeletion = true;
            db.SaveChanges();
            return RedirectToAction("Delete", "Comments", new { id = postId });
            //return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            comment.MarkForDeletion = true;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
