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
    public class CharacterRelationsController : Controller
    {
        private StoryDBContext db = new StoryDBContext();

        //
        // GET: /CharacterRelations/

        public ActionResult Index()
        {
            var characterrelations = db.CharacterRelations.Include(c => c.Character1).Include(c => c.Character2);
            return View(characterrelations.ToList());
        }

        //
        // GET: /CharacterRelations/Details/5

        public ActionResult Details(int id = 0)
        {
            CharacterRelation characterrelation = db.CharacterRelations.Find(id);
            if (characterrelation == null)
            {
                return HttpNotFound();
            }
            return View(characterrelation);
        }

        //
        // GET: /CharacterRelations/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Character1ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name");
            ViewBag.Character2ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name");
            return View();
        }

        //
        // POST: /CharacterRelations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CharacterRelation characterrelation)
        {
            if ((characterrelation.Character1 != null && characterrelation.Character1.Story.LeaderId == WebSecurity.CurrentUserId)
                || (characterrelation.Character2 != null && characterrelation.Character2.Story.LeaderId == WebSecurity.CurrentUserId))
                return new HttpUnauthorizedResult();

            var c1 = db.Characters.Find(characterrelation.Character1ID);
            var c2 = db.Characters.Find(characterrelation.Character2ID);
            if (c1 == null || c2 == null || c1.StoryID != c2.StoryID)
                ModelState.AddModelError("Character2ID", "Obie postacie muszą należeć do tego samego opowiadania.");

            if (ModelState.IsValid)
            {
                CharacterRelation c = new CharacterRelation();

                c.Character1ID = characterrelation.Character1ID;
                c.Description = characterrelation.Description;
                c.Character2ID = characterrelation.Character2ID;
                c.CreatedAt = DateTime.Now;
                c.UpdatedAt = DateTime.Now;

                db.CharacterRelations.Add(c);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Character1ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character1ID);
            ViewBag.Character2ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character2ID);
            return View(characterrelation);
        }

        //
        // GET: /CharacterRelations/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            CharacterRelation characterrelation = db.CharacterRelations.Find(id);
            if (characterrelation == null)
            {
                return HttpNotFound();
            }
            if (db.Stories.Find(characterrelation.Character1.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                ViewBag.Character1ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character1ID);
                ViewBag.Character2ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character2ID);
                return View(characterrelation);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /CharacterRelations/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(CharacterRelation characterrelation)
        {
            if ((characterrelation.Character1 != null && characterrelation.Character1.Story.LeaderId == WebSecurity.CurrentUserId)
                || (characterrelation.Character2 != null && characterrelation.Character2.Story.LeaderId == WebSecurity.CurrentUserId))
                return new HttpUnauthorizedResult();

            var c1 = db.Characters.Find(characterrelation.Character1ID);
            var c2 = db.Characters.Find(characterrelation.Character2ID);
            if (c1 == null || c2 == null || c1.StoryID != c2.StoryID)
                ModelState.AddModelError("Character2ID", "Obie postacie muszą należeć do tego samego opowiadania.");

            if (ModelState.IsValid)
            {
                CharacterRelation c = db.CharacterRelations.Find(characterrelation.ID);
                
                c.Character1ID = characterrelation.Character1ID;
                c.Description = characterrelation.Description;
                c.Character2ID = characterrelation.Character2ID;
                c.UpdatedAt = DateTime.Now;

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Character1ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character1ID);
            ViewBag.Character2ID = new SelectList(db.Characters.Where(c => c.Story.LeaderId == WebSecurity.CurrentUserId), "ID", "Name", characterrelation.Character2ID);
            return View(characterrelation);
        }

        //
        // GET: /CharacterRelations/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            CharacterRelation characterrelation = db.CharacterRelations.Find(id);
            if (characterrelation == null)
            {
                return HttpNotFound();
            }
            Character character1 = db.Characters.Find(characterrelation.Character1ID);
            if (db.Stories.Find(character1.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                return View(characterrelation);
            }
            else return new HttpUnauthorizedResult();
        }

        //
        // POST: /CharacterRelations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            CharacterRelation characterrelation = db.CharacterRelations.Find(id);
            if (db.Stories.Find(characterrelation.Character1.StoryID).LeaderId == WebSecurity.CurrentUserId)
            {
                db.CharacterRelations.Remove(characterrelation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return new HttpUnauthorizedResult();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}