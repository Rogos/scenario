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
            ViewBag.StoryID = new SelectList(db.Stories, "ID", "Title");
            return View();
        }

        //
        // POST: /Characters/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Character character)
        {
            if (db.Stories.Find(character.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                character.CreatedAt = DateTime.Now;
                character.UpdatedAt = DateTime.Now;
                character.Selected = false;
                if (ModelState.IsValid)
                {
                    db.Characters.Add(character);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.StoryID = new SelectList(db.Stories, "ID", "Title", character.StoryID);
                return View(character);
            }
            else return new HttpUnauthorizedResult();
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

                ViewBag.StoryID = new SelectList(db.Stories, "ID", "Title", character.StoryID);
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
                if (db.Stories.Find(c.StoryID).LeaderId == WebSecurity.CurrentUserId)
                {
                    c.Name = character.Name;
                    c.Description = character.Description;
                    c.Selected = character.Selected;
                    c.UpdatedAt = DateTime.Now;

                    db.Entry(c).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else return new HttpUnauthorizedResult();
            }
            ViewBag.StoryID = new SelectList(db.Stories, "ID", "Title", character.StoryID);
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
            if (db.Stories.Find(character.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                DeleteCharacter(id);
                db.Characters.Remove(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return new HttpUnauthorizedResult();
        }

        [NonAction]
        [Authorize]
        public static void DeleteCharacter(int id)
        {
            Character character = db.Characters.Find(id);
            Story story = db.Stories.Find(character.StoryID);
            List<Character> cl = new List<Character>();
            if (story.LeaderId == WebSecurity.CurrentUserId)
            {
                foreach (Thread th in db.Threads)
                {
                    bool mod = false;
                    cl.Clear();
                    foreach (Character ch in th.Characters.Where(c => c.ID == character.ID))
                    {
                        cl.Add(ch);
                    }
                    foreach(Character ch in cl)
                    {
                        th.Characters.Remove(ch);
                        mod = true;
                    }
                    if(mod)
                        db.Entry(th).State = EntityState.Modified;
                }
                db.SaveChanges();
                List<CharacterRelation> crl = new List<CharacterRelation>();
                foreach (CharacterRelation cr in db.CharacterRelations)
                {
                    if ((cr.Character1ID == character.ID) || (cr.Character2ID == character.ID))
                    {
                        crl.Add(cr);
                    }

                }
                foreach (CharacterRelation cr in crl)
                {
                    db.CharacterRelations.Remove(cr);
                }
                db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}