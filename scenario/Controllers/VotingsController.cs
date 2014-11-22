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

namespace scenario.Controllers
{
    public class VotingsController : Controller
    {
        private StoryDBContext db = new StoryDBContext();

        //
        // GET: /Votings/

        public ActionResult Index()
        {
            var votings = db.Votings.Include(v => v.Story);
            return View(votings.ToList());
        }

        //
        // GET: /Votings/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Voting voting = db.Votings.Find(id);
            if (voting == null)
            {
                return HttpNotFound();
            }
            return View(voting);
        }

        //
        // GET: /Votings/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.StoryId = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title");
            ViewBag.Threads = new MultiSelectList(db.Threads.Where(t => t.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Title");
            return View();
        }

        //
        // POST: /Votings/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "Threads,CreatedAt, UpdatedAt")]Voting voting, int[] Threads)
        {
            voting.CreatedAt = DateTime.Now;
            voting.UpdatedAt = DateTime.Now;
            if (db.Stories.Find(voting.StoryId).LeaderId == WebSecurity.CurrentUserId)
            {
                if (ModelState.IsValid)
                {
                    if (Threads != null) foreach (var ThreadId in Threads) voting.Threads.Add(db.Threads.Find(ThreadId));
                    db.Votings.Add(voting);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.StoryId = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.StoryId);
                ViewBag.Threads = new MultiSelectList(db.Threads.Where(t => t.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.Threads.Select(t => t.ID));
                return View(voting);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // GET: /Votings/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Voting voting = db.Votings.Find(id);
            if (voting == null)
            {
                return HttpNotFound();
            }

            if (db.Stories.Find(voting.StoryId).LeaderId == WebSecurity.CurrentUserId)
            {
                ViewBag.StoryId = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.StoryId);
                ViewBag.Threads = new MultiSelectList(db.Threads.Where(t => t.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.Threads.Select(t => t.ID));
                return View(voting);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // POST: /Votings/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Exclude = "Threads")]Voting v, int[] Threads)
        {
            Voting voting = db.Votings.Find(v.ID);
            
            if (db.Stories.Find(voting.StoryId).LeaderId == WebSecurity.CurrentUserId)
            {
                if (ModelState.IsValid)
                {
                    voting.UpdatedAt = DateTime.Now;
                    voting.ID = v.ID;
                    voting.CreatedAt = v.CreatedAt;
                    voting.Description = v.Description;
                    voting.Open = v.Open;
                    voting.Threads.Clear();
                    if (Threads != null) foreach (var ThreadId in Threads) voting.Threads.Add(db.Threads.Find(ThreadId));
                    db.Entry(voting).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.StoryId = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.StoryId);
                ViewBag.Threads = new MultiSelectList(db.Threads.Where(t => t.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", voting.Threads.Select(t => t.ID));
                return View(voting);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // GET: /Votings/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Voting voting = db.Votings.Find(id);
            if (voting == null)
            {
                return HttpNotFound();
            }
            if (db.Stories.Find(voting.StoryId).LeaderId == WebSecurity.CurrentUserId)
            {
                return View(voting);
            }
            else
                return new HttpUnauthorizedResult();
        }

        //
        // POST: /Votings/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Voting voting = db.Votings.Find(id);
            if (db.Stories.Find(voting.StoryId).LeaderId == WebSecurity.CurrentUserId)
            {
                db.Votings.Remove(voting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return new HttpUnauthorizedResult();
        }

        // GET: /Votings/Edit/5
        [Authorize]
        public ActionResult Vote(int id, int thread_id)
        {
            Voting voting = db.Votings.Find(id);
            Thread thread = db.Threads.Find(thread_id);
            if (voting == null || thread == null || voting.Votes.Where(v => v.UserId == WebSecurity.CurrentUserId).Count() != 0 || !voting.Open)
            {
                return HttpNotFound();
            }

            Vote vote = new Vote();
            vote.Thread = thread;
            vote.UserId = WebSecurity.CurrentUserId;
            vote.Voting = voting;

            db.Votes.Add(vote);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = voting.ID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}