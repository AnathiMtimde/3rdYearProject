using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DGSappSem2.Models;
using DGSappSem2s.Models;

namespace reservationbooks1.Controllers
{
    public class BookReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookReservations
        public ActionResult Index()
        {
            ViewBag.Email = User.Identity.GetUserName();
            if (!User.IsInRole("Admin"))
            {
                return View(db.BookReservations.ToList().Where(x=>x.StudentEmail== User.Identity.GetUserName()).ToList());
            }
            else
            {
                return View(db.BookReservations.ToList());
            }
           
        }  
        public ActionResult Penalties(int? id)
        {
            return View(db.ReservationPenalties.ToList().Where(x=>x.ReservationId==id));
        }

        // GET: BookReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReservation bookReservation = db.BookReservations.Find(id);
            if (bookReservation == null)
            {
                return HttpNotFound();
            }
            return View(bookReservation);
        }
        public ActionResult PlaceOrder(string id)
        {
            var userName = User.Identity.GetUserName();
            var bookingID = db.BookReservations.Where(x => x.StudentEmail == userName).Select(x=>x.ReservationId).Max();

            BookReservation bookReservation = db.BookReservations.Find(bookingID);
            bookReservation.Status = "Collect";
            db.Entry(bookReservation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HowToGetMyOrder(int? id)
        {
            Session["reservId"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult HowToGetMyOrder(string street_number, string street_name, string City, string State, string ZipCode, string Country)
        {
            Session["street_number"] = street_number;
            Session["street_name"] = street_name;
            Session["City"] = City;
            Session["State"] = State;
            Session["ZipCode"] = ZipCode;
            Session["Country"] = Country;
            var bookingId = Convert.ToInt32(Session["reservId"].ToString());
            var userName = User.Identity.GetUserName();
            Shipping_Address shipping_Address = new Shipping_Address();
            shipping_Address.street_number = street_number;
            shipping_Address.street_name = street_name;
            shipping_Address.City = City;
            shipping_Address.State = State;
            shipping_Address.ZipCode = ZipCode;
            shipping_Address.Country = Country;
            shipping_Address.ReservationId = bookingId;
            shipping_Address.StudentEmail = userName;
            db.shipping_Addresses.Add(shipping_Address);
            db.SaveChanges();
            BookReservation bookReservation = db.BookReservations.Find(bookingId);
            bookReservation.Status = "Delivery";
            db.Entry(bookReservation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: BookReservations/Create
        public ActionResult Create(int? id)
        {
            Session["BookId"] = id;
            return View();
        }

        // POST: BookReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,BookId,ReservationDate,StudentEmail,CollectionDate,ReturnDate,Status")] BookReservation bookReservation)
        {
            var userName = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                if (Logic.CheckBooking(bookReservation) == false)
                {
                    if (Logic.CheckDate(bookReservation.CollectionDate))
                    {
                        bookReservation.BookId = int.Parse(Session["BookId"].ToString());
                        bookReservation.StudentEmail = userName;
                        bookReservation.ReservationDate = DateTime.Now.Date;
                        bookReservation.ReturnDate = bookReservation.CollectionDate.AddDays(3);
                        bookReservation.Status = "Reserved";
                        //Logic.UpdateBookStatus(bookReservation.BookId, bookReservation.Status);
                        Logic.UpdateBookStatus1(bookReservation.BookId, bookReservation.Status);
                        db.BookReservations.Add(bookReservation);
                        db.SaveChanges();
                        EmailSender1.SendBookingEmail(bookReservation);
                        //Logic.UpdateBookStatus(bookReservation.BookId, "Reserved");

                        return RedirectToAction("HowToGetMyOrder", new { id = bookReservation.ReservationId });

                    }
                    else
                    {
                        ModelState.AddModelError("", "You can not select a date that has already passed");
                        return View(bookReservation);
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"Book is currently reserved., You can book after {bookReservation.CollectionDate.AddDays(3).ToShortDateString()}");
                    return View(bookReservation);
                }

            }

            return View(bookReservation);
        }

        // GET: BookReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReservation bookReservation = db.BookReservations.Find(id);
            if (bookReservation == null)
            {
                return HttpNotFound();
            }
            return View(bookReservation);
        }

        // POST: BookReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,BookId,ReservationDate,StudentEmail,CollectionDate,ReturnDate,Status")] BookReservation bookReservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookReservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookReservation);
        }

        // GET: BookReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReservation bookReservation = db.BookReservations.Find(id);
            if (bookReservation == null)
            {
                return HttpNotFound();
            }
            return View(bookReservation);
        }

        // POST: BookReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookReservation bookReservation = db.BookReservations.Find(id);
            db.BookReservations.Remove(bookReservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ReturnBook(int id)
        {
            var findRecord = db.BookReservations.Find(id);
            Logic.CheckOverDue(id);
            var book = db.Books.Find(findRecord.BookId);
            book.Status = "Available";
            book.BookQuantity += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            Logic.ChangeReservationStatus(id, "Returned");
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
