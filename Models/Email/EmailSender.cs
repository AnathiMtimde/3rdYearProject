using DGSappSem2.Models;
using DGSappSem2.Models;
using DGSappSem2.Models.Students;
using DGSappSem2s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DGSappSem2.Models
{
    public class EmailSender1
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static void SendBookingEmail(BookReservation bookReservation)
        {
            var book = db.Books.Find(bookReservation.BookId);
            var bookCategory = db.BookCategories.Find(book.CategoryId);
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(bookReservation.StudentEmail,bookReservation.StudentEmail));
            var body = $"Good Day {bookReservation.StudentEmail}," +
                $" Your book reservation has been successfully made /n" +
                $" -----------------------------------Reservation Details---------------------------------" +
                $"Book Title : {book.Title} /n" +
                $"Book Title : {book.Genre} /n" +
                $"Book Title : {bookCategory.CategoryName} /n" +
                $"CollectionDate : {bookReservation.CollectionDate} /n" +
                $"Return Date : {bookReservation.ReturnDate} /n" +
                $"<br/> This email confrims your reservation,please ensure that you return the book on time to prevent penalties. Should you have anny further enquiries feel free to contact us.";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Book Reservation Confirmation!!  | Ref No.:{bookReservation.StudentEmail}{book.BookId}",
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Your Friendly Librarian </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
           public static void PenaltyEmail(BookReservation bookReservation,ReservationPenalty reservationPenalty)
        {
            var book = db.Books.Find(bookReservation.BookId);
            var bookCategory = db.BookCategories.Find(book.CategoryId);
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(bookReservation.StudentEmail,bookReservation.StudentEmail));
            var body = $"Good Day {bookReservation.StudentEmail}," +
                $" THe following reservation is overdue by {reservationPenalty.NumberOfDays} days /n" +
                $" -----------------------------------Reservation Details---------------------------------" +
                $"Book Title : {book.Title} /n" +
                $"Book Title : {book.Genre} /n" +
                $"Book Title : {bookCategory.CategoryName} /n" +
                $"CollectionDate : {bookReservation.CollectionDate} /n" +
                $"Return Date : {bookReservation.ReturnDate} /n" +
                $"<br/> This email is to notify you that you've fined {reservationPenalty.PenaltyCost.ToString("c")} for not returning the book on time .";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Book Reservation Overdue!!  | Ref No.:{bookReservation.StudentEmail}{book.BookId}",
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Your Friendly Librarian </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
        public static void SendAcceptanceEmail(Student studentApplication)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.StudentEmail, studentApplication.StudentName));
            var body = $"Good Day {studentApplication.StudentName}, \n\n Your application was successful. Please proceed to registration.<br/><br/> Thank you";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Acceptance into Durban Girls Secondary School",
                mailBody = body,
                mailFooter = "<br/> <br/> <b>Durban Girls Secondary School</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
        public static void SendDeclineEmail(Student studentApplication)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.StudentEmail, studentApplication.StudentName));
            var body = $"Good Day {studentApplication.StudentName}, \n\n Your application was unsuccessful. Please try again next year." +
         $"<br/> <br/>Thank you";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Acceptance into Durban Girls Secondary School",
                mailBody = body,
                mailFooter = "<br/> <br/> <b>Durban Girls Secondary School</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
        public static void SendApplicationEmail(Student studentApplication)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.StudentEmail, studentApplication.StudentName));
            var body = $"Good Day {studentApplication.StudentName}, \n\n We have received your application, please wait for approval." +
         $"<br/><br/> Thank you";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Feedback",
                mailBody = body,
                mailFooter = "<br/> <br/> <b>Durban Girls Secondary School</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
        //public static string GetCustomerName(string customerEmail)
        //{
        //    var name = (from customer in db.Customers
        //                where customer.Email == customerEmail
        //                select customer.FirstName).FirstOrDefault();
        //    return name;
        //}
    }
}