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
    public class SchoolDepositsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SchoolDeposits
        public ActionResult Index()
        {
            return View(db.SchoolDeposits.ToList());
        }

        // GET: SchoolDeposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolDeposit schoolDeposit = db.SchoolDeposits.Find(id);
            if (schoolDeposit == null)
            {
                return HttpNotFound();
            }
            return View(schoolDeposit);
        }

        // GET: SchoolDeposits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolDeposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchoolDepositId,Amount")] SchoolDeposit schoolDeposit)
        {
            if (ModelState.IsValid)
            {
                db.SchoolDeposits.Add(schoolDeposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(schoolDeposit);
        }

        // GET: SchoolDeposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolDeposit schoolDeposit = db.SchoolDeposits.Find(id);
            if (schoolDeposit == null)
            {
                return HttpNotFound();
            }
            return View(schoolDeposit);
        }

        // POST: SchoolDeposits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolDepositId,Amount")] SchoolDeposit schoolDeposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schoolDeposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schoolDeposit);
        }

        // GET: SchoolDeposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolDeposit schoolDeposit = db.SchoolDeposits.Find(id);
            if (schoolDeposit == null)
            {
                return HttpNotFound();
            }
            return View(schoolDeposit);
        }

        // POST: SchoolDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SchoolDeposit schoolDeposit = db.SchoolDeposits.Find(id);
            db.SchoolDeposits.Remove(schoolDeposit);
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
