using DGSappSem2.Models;
using DGSappSem2.Models.Staffs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentBusiness
{
   public class ClassRoom
    {
        [Key]
        public int ClassRoomID { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }
        public virtual ICollection<StudentClassRoom> StudentClassRooms { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual ICollection<Blackboard> Blackboard { get; set; }
    }
}
