using DGSappSem2.Models;
using DGSappSem2.Models.AssessmentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssessmentBusiness
{
    public class Assessment
    {
        [Key]
        public int AssessmentID { get; set; }
        [Display(Name = "Assessment Name")]
        public string AssessmentName { get; set; }
        [Display(Name = "Contribution Percentage")]
        public int ContributionPercent { get; set; }
        public int TermID { get; set; }
        public virtual Term Term { get; set; }
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        [Display(Name = "Total Mark")]
        public int TotalMark { get; set; }
        public byte[] AssessmentDocument { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public virtual ICollection<StudentAssessment> StudentAssessments { get; set; }
        public virtual ICollection<OnlineAssessment> OnlineAssessments { get; set; }
    }
}