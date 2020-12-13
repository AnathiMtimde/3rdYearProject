using AssessmentBusiness;
using DGSappSem2.Models.Staffs;
using DGSappSem2.Models.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models.AssessmentBusiness
{
    public class OnlineAssessment
    {
        [Key]
        public int OnlineAssessmentID { get; set; }
        public int StID { get; set; }
        public virtual Student Students { get; set; }

        public int AssessmentID { get; set; }
        public virtual Assessment Assessments { get; set; }
        public int StaffId { get; set; }
        //public virtual Staff Staffs { get; set; }
        public byte[] Document { get; set; }
        public string Feedback { get; set; }
    }
}