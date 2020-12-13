using DGSappSem2s.Models;
//using Microsoft.VisualBasic.ApplicationServices;
using DGSappSem2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DGSappSem2s.Models;


namespace DGSappSem2.Models
{
    public class Logic
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public  static void UpdateBookStatus(int bookId,string status)
        {
            var bkk = db.BookReservations.Where(x=>x.BookId==bookId).FirstOrDefault();
            bkk.Status = status;
            db.Entry(bkk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
         
        } 
        public  static void ReturnBook(int bookId,string status)
        {
            var bkk = db.BookReservations.Where(x=>x.BookId==bookId).FirstOrDefault();
            bkk.Status = status;
            db.Entry(bkk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
         
        }
        public static void UpdateBookStatus1(int bookiId, string status)
        {
            var bk = db.Books.Find(bookiId);
            bk.BookQuantity -= 1;
            if (bk.BookQuantity == 0)
            {
                bk.Status = status;
            }
            db.Entry(bk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        } 
        public static void ReturnBookStatus1(int bookiId, string status)
        {
            var bk = db.Books.Find(bookiId);
            bk.Status = status;
            bk.BookQuantity += 1;
            db.Entry(bk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }


        public static void ChangeReservationStatus(int id, string status)
        {
            var bkk = db.BookReservations.Find(id);
            bkk.Status = status;
            db.Entry(bkk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public static bool CheckDate(DateTime dateTime)
        {
            return dateTime >= DateTime.Now.Date;
        }


        public static void CheckOverDue(int id)
        {
            var reservation = db.BookReservations.Find(id);
            if (DateTime.Now.Date > reservation.ReturnDate.Date)
            {
                var numberOfDays = CalcNoOfDays(reservation.ReturnDate);
                var penaltySettings = db.PenaltyCosts.Where(x=>x.NumberOfDays==numberOfDays).FirstOrDefault();
                var penaltyRecord = new ReservationPenalty();
                penaltyRecord.NumberOfDays = numberOfDays;
                penaltyRecord.ReservationId =(int) id;
                penaltyRecord.PenaltyCost = penaltySettings.CostPrice;
                db.ReservationPenalties.Add(penaltyRecord);
                db.SaveChanges();
                AdAmount(penaltySettings.CostPrice, reservation.StudentEmail);
                ChangeReservationStatus ( id,"Over Due");
                EmailSender1.PenaltyEmail(reservation, penaltyRecord);
            }
        }

        public static int CalcNoOfDays(DateTime ReturndateTime)
        {
            return (DateTime.Now.Subtract(ReturndateTime).Days);
        }

        public static bool CheckBooking(BookReservation booking)
        {
            bool result = false;
            var dbRecords = db.BookReservations.Where(x => x.CollectionDate == booking.CollectionDate).ToList();
            foreach (var item in dbRecords)
            {
                if (booking.BookId == booking.BookId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static void CreateAccount(string studentEmail)
        {
            var studentAccount = new StudentDebt();
            studentAccount.StudentEmail = studentEmail;
            studentAccount.Amount = 0;
            studentAccount.Status = "Allowed";
            db.StudentDebts.Add(studentAccount);
            db.SaveChanges();
        }

        public static void AdAmount(decimal amount,string studentEmail)
        {
            var studentAccount = db.StudentDebts.Where(x => x.StudentEmail == studentEmail).FirstOrDefault();
            studentAccount.Amount += amount;
            studentAccount.Status = "Blocked";
            db.Entry(studentAccount).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }  public static void Paid(decimal amount,string studentEmail)
        {
            var studentAccount = db.StudentDebts.Where(x => x.StudentEmail == studentEmail).FirstOrDefault();
            studentAccount.Amount -= amount;
            studentAccount.Status = "Allowed";
            db.Entry(studentAccount).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public static decimal GetPenaltyCost(string studentEmail)
        {
            try
            {
                var amount = db.StudentDebts.Where(x => x.StudentEmail == studentEmail).FirstOrDefault();
                if (amount != null)
                {
                    return amount.Amount;

                }
                return 0M;
            }
            catch (Exception ex)
            {

                throw ex;
            }
    
        }

        public static bool CheckRoomNumber(string roomNumber)
        {
            bool result = false;
            var rooms = db.GroupRooms.ToList();
            foreach (var item in rooms)
            {
                if (item.RoomNumber == roomNumber)
                {
                    result = true;
                }
            }
            return result;
        }
        public static string getRoomNumber(int id)
        {
            var roomNumber = (from g in db.GroupRooms
                          where g.GroupRoomId == id
                          select g.RoomNumber).FirstOrDefault();
            return roomNumber;
        }
        public static bool checkCapacity(int roomID, int capacity)
        {
            bool result = false;
            var roomCap = db.GroupRooms.Find(roomID);
            if(capacity > roomCap.capacity)
            {
                result = true;
            }
            return result;
        }
        public static bool checkTimeDifference(GrpRoomReservation grpRoomReservation)
        {
            bool result = false;
            int timeDiffrerence = grpRoomReservation.TimeOut.Hour - grpRoomReservation.TimeIn.Hour;
            if(timeDiffrerence > 2)
            {
                result = true;
            }
            return result;
        }

        public static bool checkTime(GrpRoomReservation grpRoomReservation)
        {
            bool result = false;
            var libraryTimes = db.LibraryTimes.ToList().FirstOrDefault();
            if(grpRoomReservation.TimeIn < libraryTimes.openingTime || grpRoomReservation.TimeIn > libraryTimes.closingTime)
            {
                result = true;               
            }
            else if (grpRoomReservation.TimeOut > libraryTimes.closingTime || grpRoomReservation.TimeOut < libraryTimes.openingTime)
            {
                result = true;
            }
            return result;
        }
        public static bool checkAvailability(GrpRoomReservation grpRoomReservation, int roomId)
        {
            bool result = false;
            var checkAvail = db.GrpRoomReservations.Where(x => x.TimeOut > grpRoomReservation.TimeIn && x.GroupRoomId == roomId && x.DateFor == grpRoomReservation.DateFor).Count();
            if(checkAvail > 0)
            {
                result = true;
            }
            return result;
        }
    }
}