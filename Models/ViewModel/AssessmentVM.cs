
using System;
using System.Linq;

namespace DGSappSem2.Models.ViewModel
{
    public class AssessmentVM
    {
        public int AssessmentID { get; set; }
        public string AssessmentName { get; set; }
        public int ContributionPercent { get; set; }
        public static string GetFeedback(int assessmentID)
        {
            var db=new ApplicationDbContext();
            var feedback = db.OnlineAssessments.Where(f => f.AssessmentID == assessmentID).Select(e => e.Feedback).FirstOrDefault();
            if (string.IsNullOrEmpty(feedback))
            {
                feedback = "No assessment submitted";
            }
            return feedback;
        }
        public byte[] UploadDocument { get; set; }
        public int TermID { get; set; }
        public int TotalMark { get; set; }
        public int ActualMark { get; set; }
        public int SubjectID { get; set; }
        public DateTime DueDate { get; set; }
    }
}