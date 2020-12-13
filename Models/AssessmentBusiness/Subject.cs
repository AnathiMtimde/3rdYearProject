using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentBusiness
{
   public class Subject
    {
        [Key]
        public int SubjectID { get; set; }
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }
        [Display(Name = "Required Percentage")]
        public int RequiredPercentage { get; set; }
        public int ClassRoomID { get; set; }
        public string Upload { get; set; }
        public virtual ClassRoom ClassRoom { get; set; }
        public virtual ICollection <Assessment> Assessments { get; set; }
    }
}
