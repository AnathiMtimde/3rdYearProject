﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using DGSappSem2.Models;
using DGSappSem2.Models.Students;
using Microsoft.AspNet.Identity.Owin;

namespace DGSappSem2.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public StudentsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public StudentsController()
        {
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Students
        public ActionResult Index()
        {
            return View(db.students.ToList());
        }
        public ActionResult ConfirmRegistration(int? id)
        {
            Session["bookID"] = id;

            Student studentApplication = db.students.Where(p => p.StID == id).FirstOrDefault();
            var username = User.Identity.GetUserName();
            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(GeneratePDF(id)), "Proof of registration", "application/pdf"));
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(username, studentApplication.StudentName));
            var body = $"Hello {studentApplication.StudentName}, \n\n {studentApplication.StudentName} has been successfully registered at our school. Please Find the attached registration information(Proof of Registration)<br/> Regards,<br/><br/> New Erra <br/> .";

            DGSappSem2.Models.EmailService emailService = new DGSappSem2.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + studentApplication.StID,
                mailBody = body,
                mailFooter = "<br/> <br/> <b>New Erra</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments

            });
            TempData["AlertMessage"] = $"{studentApplication.StudentName} has been successfully Registered";

            return RedirectToAction("Index");
        }
        public ActionResult Successful()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(Student student, HttpPostedFileBase StudentBirthCertURL, HttpPostedFileBase StudentReportURL, HttpPostedFileBase StudentProofResURL, HttpPostedFileBase StudentPermitURL)
        {
            StudentBirthCertURL.SaveAs(Server.MapPath("/") + "/Content/" + StudentBirthCertURL.FileName);
            student.StudentBirthCertURL = StudentBirthCertURL.FileName;


            StudentReportURL.SaveAs(Server.MapPath("/") + "/Content/" + StudentReportURL.FileName);
            student.StudentReportURL = StudentReportURL.FileName;

            StudentProofResURL.SaveAs(Server.MapPath("/") + "/Content/" + StudentProofResURL.FileName);
            student.StudentProofResURL = StudentProofResURL.FileName;

            StudentPermitURL.SaveAs(Server.MapPath("/") + "/Content/" + StudentPermitURL.FileName);
            student.StudentPermitURL = StudentPermitURL.FileName;

            student.StudentAllowReg = false;
            student.Status = "Pending";
            var user = new ApplicationUser { UserName = student.StudentEmail, Email = student.StudentEmail };
            string pwd = "@User001";
            var result = await UserManager.CreateAsync(user, pwd);
          
            Logic.CreateAccount(student.StudentEmail);
            db.students.Add(student);
            db.SaveChanges();
            EmailSender1.SendApplicationEmail(student);
            UserManager.AddToRole(user.Id, "Student");
            TempData["AlertMessage"] = "All your details have been successfully captured and the form has been submitted.\n" +
                "Please wait for your details to be verified, this may take up to a week and if you are accepted then you will receive a email indicating that you may proceed to registration.\n" +
                " Thank You!!  ";
            return RedirectToAction("Index","Home");
        }


        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StID,StudentName,StudentSurname,StudentGender,StudentAddress,StudentTown,StudentContact,StudentGrade,StudentEmail,StudentBirthCertURL,StudentReportURL,StudentProofResURL,StudentPermitURL,StudentAllowReg")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Status = "Accepted";
                student.StudentAllowReg = true;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                EmailSender1.SendAcceptanceEmail(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.students.Find(id);
            db.students.Remove(student);
            db.SaveChanges();
            EmailSender1.SendDeclineEmail(student);
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
        public byte[] GeneratePDF(int? ReservationID)
        {
            MemoryStream memoryStream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            int ID = int.Parse(Session["bookID"].ToString());
            Student roomBooking = new Student();
            roomBooking = db.students.Find(ID);

            //var tenant1 = db.Tenants.Find(roomBooking.TenantId);


            //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
            //var user = _iUserService.Get(reservation.UserID);

            iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.RED);
            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Color.BLUE);

            // Create the heading paragraph with the headig font
            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(5);
            PdfPTable table3 = new PdfPTable(1);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            seperator.Offset = -6f;
            // Remove table cell
            table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table1.WidthPercentage = 80;
            table1.SetWidths(new float[] { 100 });
            table2.WidthPercentage = 80;
            table3.SetWidths(new float[] { 100 });
            table3.WidthPercentage = 80;

            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            table1.AddCell("\n");
            table1.AddCell(cell);
            table1.AddCell("\n\n");
            table1.AddCell(
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
                "New Erra \n" +
                "Email :chantellekap98@gmail.com" + "\n" +
                "\n" + "\n");
            table1.AddCell("            Student Details                        ");
            table1.AddCell("\nClass Name : \t" + roomBooking.StudentGrade);
            table1.AddCell("Full Name : \t" + roomBooking.StudentName);
            table1.AddCell("Last Name : \t" + roomBooking.StudentSurname);
            table1.AddCell("Gender : \t" + roomBooking.StudentGender);
            table1.AddCell("Address : \t" + roomBooking.StudentAddress);
            table1.AddCell("Student Town : \t" + roomBooking.StudentTown);
            table1.AddCell("Student Contact : \t" + roomBooking.StudentContact);
            table1.AddCell("Student Email : \t" + roomBooking.StudentEmail);

            table1.AddCell("\n           School details  \n");

            table1.AddCell("\nStudentGrade : \t" + roomBooking.StudentGrade);
     


            table1.AddCell("\n");

            //table3.AddCell("------------Looking forward to hear from you soon-");

            //////Intergrate information into 1 document
            //var qrCode = iTextSharp.text.Image.GetInstance(reservation.QrCodeImage);
            //qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            //document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }

    }
}
