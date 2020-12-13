using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using DGSappSem2.Models;
using DGSappSem2s.Models;


namespace DGSappSem2.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.BookCategory);
            return View(books.ToList());
        }
        public ActionResult BooksView()
        {
            var books = db.Books.Include(b => b.BookCategory);
            return View(books.ToList());
        }
        public ActionResult ReturnBook(int id)
        {
            Logic.UpdateBookStatus(id, "Available");
            return RedirectToAction("Index");
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.BookCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,CategoryId,Title,Description,Author,Numpages,Genre,Status,Picture,BookQuantity")] Book book, HttpPostedFileBase photoUpload)
        {
            if (ModelState.IsValid)
            {
                book.Status="Available";
            
                // ViewBag.Category_ID = new SelectList(category_Service.GetCategories(), "Category_ID", "Name");
                byte[] photo = null;
                photo = new byte[photoUpload.ContentLength];
                photoUpload.InputStream.Read(photo, 0, photoUpload.ContentLength);
                book.Picture = photo;
                db.Books.Add(book);
                db.SaveChanges();

                //QR Code generator
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(book.BookId.ToString(), QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5);
                book.QrCodeImage = ImageToByte(qrCodeImage);
                    
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.BookCategories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.BookCategories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,CategoryId,Title,Description,Author,Numpages,Genre,Status,BookQuantity")] Book book, HttpPostedFileBase img_upload)
        {
            byte[] Models = null;
            Models = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(Models, 0, img_upload.ContentLength);
            book.Picture = Models;
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.BookCategories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
