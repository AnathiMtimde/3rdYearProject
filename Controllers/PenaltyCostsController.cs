using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGSappSem2.Models;

namespace reservationbooks1.Controllers
{
    public class PenaltyCostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PenaltyCosts
        public ActionResult Index()
        {
            return View(db.PenaltyCosts.ToList());
        }

        // GET: PenaltyCosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyCost penaltyCost = db.PenaltyCosts.Find(id);
            if (penaltyCost == null)
            {
                return HttpNotFound();
            }
            return View(penaltyCost);
        }

        // GET: PenaltyCosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenaltyCosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PenaltyCostId,NumberOfDays,CostPrice")] PenaltyCost penaltyCost)
        {
            if (ModelState.IsValid)
            {
                db.PenaltyCosts.Add(penaltyCost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(penaltyCost);
        }

        // GET: PenaltyCosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyCost penaltyCost = db.PenaltyCosts.Find(id);
            if (penaltyCost == null)
            {
                return HttpNotFound();
            }
            return View(penaltyCost);
        }

        // POST: PenaltyCosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PenaltyCostId,NumberOfDays,CostPrice")] PenaltyCost penaltyCost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(penaltyCost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(penaltyCost);
        }

        // GET: PenaltyCosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyCost penaltyCost = db.PenaltyCosts.Find(id);
            if (penaltyCost == null)
            {
                return HttpNotFound();
            }
            return View(penaltyCost);
        }

        // POST: PenaltyCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PenaltyCost penaltyCost = db.PenaltyCosts.Find(id);
            db.PenaltyCosts.Remove(penaltyCost);
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
