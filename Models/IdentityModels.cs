using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using DGSappSem2.Models.Staffs;
using DGSappSem2.Models.Students;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DGSappSem2.Models.Events;
using AssessmentBusiness;
using DGSappSem2.Models.AssessmentBusiness;

using DGSappSem2s.Models;

namespace DGSappSem2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public System.Data.Entity.DbSet<DGSappSem2.Models.FileUpload.FileUploadModel> fileUploadModel { get; set; }

        public DbSet<Student> students { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StudentTimetable> StudentTimetables { get; set; }
        public DbSet<StaffAttendance> StaffAttendances { get; set; }
        public DbSet<StaffTimetable> StaffTimetables { get; set; }
        //___________________Assessmentbusiness___________________
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<OnlineAssessment> OnlineAssessments { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<StudentClassRoom> StudentClassRooms { get; set; }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Term> Terms { get; set; }

        public DbSet<StudentAssessment> StudentAssessments { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Admin> Admins { get; set; }
       public DbSet<Guardian> Guardians { get; set; }
       public DbSet<Shipping_Address> shipping_Addresses { get; set; }

        public System.Data.Entity.DbSet<BookReservation> BookReservations { get; set; }

        public System.Data.Entity.DbSet<RegistrationPayment> RegistrationPayments { get; set; }

        public System.Data.Entity.DbSet<PenaltyCost> PenaltyCosts { get; set; }
        public System.Data.Entity.DbSet<ReservationPenalty> ReservationPenalties { get; set; }
        public System.Data.Entity.DbSet<StudentDebt> StudentDebts { get; set; }
        //__________________AssessmentBusiness____________________


        //Event Database
        public DbSet<Event> Events { get; set; }
        public DbSet<BookEvent> BookEvents { get; set; }
        public DbSet<Venue> Venues { get; set; }

     
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<DGSappSem2.Models.Blackboard> Blackboards { get; set; }

        public System.Data.Entity.DbSet<DGSappSem2.Models.Timeslot> Timeslots { get; set; }

        public System.Data.Entity.DbSet<DGSappSem2.Models.GroupRoom> GroupRooms { get; set; }

        public System.Data.Entity.DbSet<DGSappSem2.Models.GrpRoomReservation> GrpRoomReservations { get; set; }

        public System.Data.Entity.DbSet<DGSappSem2.Models.LibraryTimes> LibraryTimes { get; set; }

        public System.Data.Entity.DbSet<DGSappSem2.Models.SchoolDeposit> SchoolDeposits { get; set; }
    }
}