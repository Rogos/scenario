using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scenario.Models;
using scenario.DAL;
using WebMatrix.WebData;
using scenario.Filters;

namespace scenario.Controllers
{
    [InitializeSimpleMembership]
    public class StoriesController : Controller
    {
        private StoryDBContext db = new StoryDBContext();

        //
        // GET: /Stories/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Stories.ToList());
        }

        //
        // GET: /Stories/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        //
        // GET: /Stories/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Stories/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Story story)
        {
            story.CreatedAt = DateTime.Now;
            story.UpdatedAt = DateTime.Now;
            story.LeaderId = WebSecurity.CurrentUserId;
            if (ModelState.IsValid)
            {
                db.Stories.Add(story);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(story);
        }

        //
        // GET: /Stories/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            if (story.LeaderId == WebSecurity.CurrentUserId)
            {
                return View(story);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /Stories/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Story story)
        {


            story.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                Story s = db.Stories.Find(story.ID);
                if (s.LeaderId == WebSecurity.CurrentUserId)
                {
                    s.Title = story.Title;
                    s.Description = story.Description;
                    s.UpdatedAt = DateTime.Now;

                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else return new HttpUnauthorizedResult();
            }
            return View(story);


        }

        //
        // GET: /Stories/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            if (story.LeaderId == WebSecurity.CurrentUserId)
            {
                return View(story);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /Stories/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = db.Stories.Find(id);
            if (story.LeaderId == WebSecurity.CurrentUserId)
            {
                db.Stories.Remove(story);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return new HttpUnauthorizedResult();
        }


        public ActionResult Search(string searchString)
        {
            var stories = from s in db.Stories
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                stories = stories.Where(st => st.Title.Contains(searchString));
            }

            return View(stories);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}