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
using System.Text;

namespace scenario.Controllers
{
    [InitializeSimpleMembership]
    public class ThreadsController : Controller
    {
        private static StoryDBContext db = new StoryDBContext();

        //
        // GET: /Threads/

        [AllowAnonymous]
        public ActionResult Index(string search)
        {
            var threads = db.Threads.Include(t => t.Author);

            if (!String.IsNullOrEmpty(search))
            {
                threads = threads.Where(t => t.Text.Contains(search));
            }

            return View(threads.ToList());
        }

        //
        // GET: /Threads/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Thread thread = db.Threads.Find(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            return View(thread);
        }

        [AllowAnonymous]
        public ActionResult NameSearch(string name, int id = 0)
        {
            Thread thread = db.Threads.Find(id);
            if (thread == null)
            {
                return HttpNotFound();
            }

            ViewBag.name = name;
            ViewBag.text = thread.Text;
            return View(thread);

        }

        //
        // GET: /Threads/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.UserProfiles, "ID", "UserName");
            ViewBag.StoryId = new SelectList(db.Stories, "ID", "Title");
            ViewBag.Characters = new MultiSelectList(db.Characters, "ID", "Name");
            ViewBag.Parents = new MultiSelectList(db.Threads, "ID", "Title");
            return View();
        }

        //
        // POST: /Threads/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "Parents, Characters, AuthorId, CreatedAt, UpdatedAt")]Thread thread, int[] Parents, int[] Characters)
        {
            thread.AuthorId = WebSecurity.CurrentUserId;
            thread.CreatedAt = DateTime.Now;
            thread.UpdatedAt = DateTime.Now;
            thread.Selected = false;
            if (ModelState.IsValid)
            {
                if (Parents != null) foreach (var ParentId in Parents) thread.Parents.Add(db.Threads.Find(ParentId));
                if (Characters != null) foreach (var CharacterId in Characters) thread.Characters.Add(db.Characters.Find(CharacterId));
                db.Threads.Add(thread);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.UserProfiles, "ID", "UserName");
            ViewBag.StoryId = new SelectList(db.Stories, "ID", "Title");
            ViewBag.Characters = new MultiSelectList(db.Characters, "ID", "Name");
            ViewBag.Parents = new MultiSelectList(db.Threads, "ID", "Title");
            return View(thread);
        }

        //
        // GET: /Threads/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Thread thread = db.Threads.Find(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            if ((thread.AuthorId == WebSecurity.CurrentUserId) || (thread.Story.LeaderId == WebSecurity.CurrentUserId))
            {
                ViewBag.StoryId = new SelectList(db.Stories, "ID", "Title", thread.StoryId);
                ViewBag.Characters = new MultiSelectList(db.Characters, "ID", "Name", thread.Characters.Select(s => s.ID));
                ViewBag.Parents = new MultiSelectList(db.Threads, "ID", "Title", thread.Parents.Select(s => s.ID));
                return View(thread);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // POST: /Threads/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Exclude = "Parents, Characters, AuthorId, CreatedAt, UpdatedAt")]Thread thread, int[] Parents, int[] Characters)
        {
            Thread t = db.Threads.Find(thread.ID);
            if ((t.AuthorId == WebSecurity.CurrentUserId) || (t.Story.LeaderId == WebSecurity.CurrentUserId))
            {
                if (ModelState.IsValid)
                {
                    t.Title = thread.Title;
                    t.Description = thread.Description;
                    t.Text = thread.Text;
                    t.Selected = thread.Selected;
                    t.Main = thread.Main;
                    t.StoryId = thread.StoryId;
                    t.UpdatedAt = DateTime.Now;
                    t.Parents.Clear();
                    if (Parents != null) foreach (var ParentId in Parents) t.Parents.Add(db.Threads.Find(ParentId));
                    t.Characters.Clear();
                    if (Characters != null) foreach (var CharacterId in Characters) t.Characters.Add(db.Characters.Find(CharacterId));

                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }


                ViewBag.AuthorId = new SelectList(db.UserProfiles, "ID", "UserName", t.AuthorId);
                ViewBag.StoryId = new SelectList(db.Stories, "ID", "Title", t.StoryId);
                ViewBag.Characters = new MultiSelectList(db.Characters, "ID", "Name", t.Characters.Select(s => s.ID));
                ViewBag.Parents = new MultiSelectList(db.Threads, "ID", "Title", t.Parents.Select(s => s.ID));
                return View(thread);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // GET: /Threads/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Thread thread = db.Threads.Find(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            if ((thread.AuthorId == WebSecurity.CurrentUserId) || (thread.Story.LeaderId == WebSecurity.CurrentUserId))
            {
                return View(thread);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // POST: /Threads/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Thread thread = db.Threads.Find(id);
            if ((thread.AuthorId == WebSecurity.CurrentUserId) || (thread.Story.LeaderId == WebSecurity.CurrentUserId))
            {
                db.Threads.Remove(thread);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return new HttpUnauthorizedResult();
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}