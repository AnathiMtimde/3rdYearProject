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
    public class GroupRoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GroupRooms
        public ActionResult Index()
        {
            return View(db.GroupRooms.ToList());
        }
        public ActionResult GroupRoomView()
        {
            return View(db.GroupRooms.ToList());
        }
        // GET: GroupRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRoom groupRoom = db.GroupRooms.Find(id);
            if (groupRoom == null)
            {
                return HttpNotFound();
            }
            return View(groupRoom);
        }

        // GET: GroupRooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroupRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupRoomId,RoomNumber,Status,capacity")] GroupRoom groupRoom)
        {
            if (ModelState.IsValid)
            {
                if (Logic.CheckRoomNumber(groupRoom.RoomNumber) == false)
                {
                    groupRoom.Status = "Available";
                    db.GroupRooms.Add(groupRoom);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Room Number Already exists , please use another combination");
                    return View(groupRoom);
                }
            }
            return View(groupRoom);
        }

        // GET: GroupRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRoom groupRoom = db.GroupRooms.Find(id);
            if (groupRoom == null)
            {
                return HttpNotFound();
            }
            return View(groupRoom);
        }

        // POST: GroupRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupRoomId,RoomNumber,Status,capacity")] GroupRoom groupRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupRoom);
        }

        // GET: GroupRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRoom groupRoom = db.GroupRooms.Find(id);
            if (groupRoom == null)
            {
                return HttpNotFound();
            }
            return View(groupRoom);
        }

        // POST: GroupRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupRoom groupRoom = db.GroupRooms.Find(id);
            db.GroupRooms.Remove(groupRoom);
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
