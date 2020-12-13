using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGSappSem2.Models;

namespace DGSappSem2.Controllers
{
    public class LibraryTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LibraryTimes
        public ActionResult Index()
        {
            return View(db.LibraryTimes.ToList());
        }

        // GET: LibraryTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryTimes libraryTimes = db.LibraryTimes.Find(id);
            if (libraryTimes == null)
            {
                return HttpNotFound();
            }
            return View(libraryTimes);
        }

        // GET: LibraryTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LibraryTimesId,openingTime,closingTime")] LibraryTimes libraryTimes)
        {
            if (ModelState.IsValid)
            {
                db.LibraryTimes.Add(libraryTimes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(libraryTimes);
        }

        // GET: LibraryTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryTimes libraryTimes = db.LibraryTimes.Find(id);
            if (libraryTimes == null)
            {
                return HttpNotFound();
            }
            return View(libraryTimes);
        }

        // POST: LibraryTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LibraryTimesId,openingTime,closingTime")] LibraryTimes libraryTimes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libraryTimes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(libraryTimes);
        }

        // GET: LibraryTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryTimes libraryTimes = db.LibraryTimes.Find(id);
            if (libraryTimes == null)
            {
                return HttpNotFound();
            }
            return View(libraryTimes);
        }

        // POST: LibraryTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LibraryTimes libraryTimes = db.LibraryTimes.Find(id);
            db.LibraryTimes.Remove(libraryTimes);
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
