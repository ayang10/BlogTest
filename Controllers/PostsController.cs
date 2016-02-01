using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogTest.Models;
using System.IO;

namespace BlogTest.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create( Post post, HttpPostedFileBase fileUpload) 
        {
            post.CreationDate = new DateTimeOffset(DateTime.Now);
            if (ModelState.IsValid)
            {
                // restricting the valid file formats to images only
                if (Post.ImageUploadValidator.IsWebFriendlyImage(fileUpload))
                {
                    var fileName = System.IO.Path.GetFileName(fileUpload.FileName);
                    fileUpload.SaveAs(Path.Combine(Server.MapPath("~/img/"), fileName));
                    post.MediaUrl = "/img/" + fileName;

                }
                

                post.CreationDate = new DateTimeOffset(DateTime.Now);
                db.Posts.Add(post); //add the object
                db.SaveChanges(); //creates a sql statement and sends it out
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,UpdateDate,Title,BodyText,MediaUrl,Published,CategoryId")] Post post, HttpPostedFileBase fileUpload)
        {

            post.UpdateDate = new DateTimeOffset(DateTime.Now);
            
            var fetched = db.Posts.Find(post.Id);
            fetched.Title = post.Title;
            fetched.BodyText = post.BodyText;
            fetched.MediaUrl = post.MediaUrl;
            fetched.Published = post.Published;
            fetched.Categories = post.Categories;
            fetched.UpdateDate = post.UpdateDate;

            if (Post.ImageUploadValidator.IsWebFriendlyImage(fileUpload))
            {
                string oldfilePath = fetched.MediaUrl;
                if(fileUpload !=null && fileUpload.ContentLength > 0)
                {
                    var somefile = Path.GetFileName(fileUpload.FileName);
                    string path = System.IO.Path.Combine(
                        Server.MapPath("~/img/"), somefile);
                    fileUpload.SaveAs(path);
                    fetched.MediaUrl = "/img/" + fileUpload.FileName;
                    string fullPath = Request.MapPath("~" + oldfilePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                

            }


            if (ModelState.IsValid)
            {

                


                post.UpdateDate = new DateTimeOffset(DateTime.Now);
                db.Entry(fetched).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            

            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {

            Post post = db.Posts.Find(id);
            post.CreationDate = new DateTimeOffset(DateTime.Now);
                post.UpdateDate = new DateTimeOffset(DateTime.Now);
            db.Posts.Remove(post);
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

        [HttpPost]
        public ActionResult Upload(string ActionName)
        {
            var path = Server.MapPath("~/img/");
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item];
                if (file.ContentLength == 0)
                {
                    //Repeated upload file be skipped .
                    continue;
                }
             string savedFileName = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(savedFileName);
            }
            return RedirectToAction(ActionName);
        }


        }
}
