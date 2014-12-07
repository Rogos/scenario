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
    public class CharactersController : Controller
    {
        private static StoryDBContext db = new StoryDBContext();

        //
        // GET: /Characters/

        public ActionResult Index()
        {
            var characters = db.Characters.Include(c => c.Story);
            return View(characters.ToList());
        }

        //
        // GET: /Characters/Details/5

        public ActionResult Details(int id = 0)
        {
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        //
        // GET: /Characters/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.StoryID = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title");
            return View();
        }

        //
        // POST: /Characters/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "AuthorId, CreatedAt, UpdatedAt, Selected")]Character character)
        {
            character.CreatedAt = DateTime.Now;
            character.UpdatedAt = DateTime.Now;
            character.AuthorId = WebSecurity.CurrentUserId;
            character.Selected = character.Story.LeaderId == WebSecurity.CurrentUserId;

            if (ModelState.IsValid)
            {
                db.Characters.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoryID = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", character.StoryID);
            return View(character);
        }

        //
        // GET: /Characters/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            if (db.Stories.Find(character.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {

                ViewBag.StoryID = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", character.StoryID);
                return View(character);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /Characters/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID, Name, Description, Selected")]Character character)
        {
            if (ModelState.IsValid)
            {
                Character c = db.Characters.Find(character.ID);

                if (c.Story.LeaderId != WebSecurity.CurrentUserId && c.AuthorId != WebSecurity.CurrentUserId) return new HttpUnauthorizedResult();
                
                c.Name = character.Name;
                c.Description = character.Description;
                c.Selected = character.Selected;
                c.UpdatedAt = DateTime.Now;

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.StoryID = new SelectList(db.Stories.Where(s => s.LeaderId == WebSecurity.CurrentUserId), "ID", "Title", character.StoryID);
            return View(character);
        }

        //
        // GET: /Characters/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            if (db.Stories.Find(character.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                return View(character);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /Characters/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = db.Characters.Find(id);

            if (character.Story.LeaderId != WebSecurity.CurrentUserId && character.AuthorId != WebSecurity.CurrentUserId) return new HttpUnauthorizedResult();
            
            db.Characters.Remove(character);
            db.SaveChanges();
            return RedirectToAction("Index");
        }        

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}