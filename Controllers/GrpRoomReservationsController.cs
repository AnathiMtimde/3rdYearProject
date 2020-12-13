using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGSappSem2.Models;
using Microsoft.AspNet.Identity;

namespace DGSappSem2.Controllers
{
    public class GrpRoomReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GrpRoomReservations
        public ActionResult Index()
        {
           var grpRoomReservations = db.GrpRoomReservations;
            var userName = User.Identity.GetUserName();
            if (!User.IsInRole("Customer"))
            {
                return View(grpRoomReservations.ToList());
            }
            else
            {
                return View(grpRoomReservations.ToList().Where(x => x.StudentEmail == userName));
            }
            //   return View(db.GrpRoomReservations.ToList());
        }

        // GET: GrpRoomReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrpRoomReservation grpRoomReservation = db.GrpRoomReservations.Find(id);
            if (grpRoomReservation == null)
            {
                return HttpNotFound();
            }
            return View(grpRoomReservation);
        }
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrpRoomReservation grpRoomReservation = db.GrpRoomReservations.Find(id);
            if (grpRoomReservation == null)
            {
                return HttpNotFound();
            }
            return View(grpRoomReservation);
        }

        // GET: GrpRoomReservations/Create
        public ActionResult Create(int? id)
        {
            Session["id"] = id;
            return View();
        }

        // POST: GrpRoomReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrpRoomReservationId,StudentEmail,RoomNumber,DateReserved,DateFor,TimeIn,TimeOut,Status,capacity")] GrpRoomReservation grpRoomReservation)
        {
            var grpRoomId =Convert.ToInt32(Session["id"].ToString());
            var userName = User.Identity.GetUserName(); 
            if (ModelState.IsValid)
            {
                if(Logic.checkAvailability(grpRoomReservation, grpRoomId)==false)
                {
                    if (Logic.checkTime(grpRoomReservation) == false)
                    {
                        if (Logic.checkTimeDifference(grpRoomReservation) == false)
                        {
                            if (Logic.checkCapacity(grpRoomId, grpRoomReservation.capacity) == false)
                            {
                                grpRoomReservation.RoomNumber = Logic.getRoomNumber(grpRoomId);
                                grpRoomReservation.GroupRoomId = grpRoomId;
                                grpRoomReservation.StudentEmail = userName;
                                grpRoomReservation.Status = "Booked";
                                grpRoomReservation.DateReserved = DateTime.Now.Date;
                                db.GrpRoomReservations.Add(grpRoomReservation);
                                db.SaveChanges();
                                return RedirectToAction("Confirm", new { id = grpRoomReservation.GrpRoomReservationId });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Limit reached");
                                return View(grpRoomReservation);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Time should be 2 hours or less");
                            return View(grpRoomReservation);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The library is not operational at those times");
                        return View(grpRoomReservation);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The Room is already booked");
                    return View(grpRoomReservation);
                }
            }
            return View(grpRoomReservation);
        }

        // GET: GrpRoomReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrpRoomReservation grpRoomReservation = db.GrpRoomReservations.Find(id);
            if (grpRoomReservation == null)
            {
                return HttpNotFound();
            }
            return View(grpRoomReservation);
        }

        // POST: GrpRoomReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrpRoomReservationId,StudentEmail,RoomNumber,DateReserved,DateFor,TimeIn,TimeOut,Status")] GrpRoomReservation grpRoomReservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grpRoomReservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grpRoomReservation);
        }

        // GET: GrpRoomReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrpRoomReservation grpRoomReservation = db.GrpRoomReservations.Find(id);
            if (grpRoomReservation == null)
            {
                return HttpNotFound();
            }
            return View(grpRoomReservation);
        }

        // POST: GrpRoomReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrpRoomReservation grpRoomReservation = db.GrpRoomReservations.Find(id);
            db.GrpRoomReservations.Remove(grpRoomReservation);
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
