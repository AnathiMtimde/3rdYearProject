using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssessmentBusiness;
using DGSappSem2.Models;
using DGSappSem2.Models.AssessmentBusiness;
using DGSappSem2.Models.ViewModel;

namespace DGSappSem2.Controllers
{
    public class OnlineAssessmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

   

        // GET: OnlineAssessments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineAssessment onlineAssessment = db.OnlineAssessments.Find(id);
            if (onlineAssessment == null)
            {
                return HttpNotFound();
            }
            return View(onlineAssessment);
        }

        // GET: OnlineAssessments/Create
        public ActionResult Create()
        {
            ViewBag.AssessmentID = new SelectList(db.Assessments, "AssessmentID", "AssessmentName");
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name");
            ViewBag.StID = new SelectList(db.students, "StID", "StudentName");
            return View();
        }
        public ActionResult Upload(int assessmentID)
        {
            ViewBag.AssessmentID = assessmentID;
            return View();
        }

        public ActionResult StudentSubjectList()
        {
            var studentUser = User.Identity.Name;
            var studentId = db.students.Where(p => p.StudentEmail == studentUser).Select(o=>o.StID).FirstOrDefault();
            var studentClassroom = db.StudentClassRooms.Where(l => l.StID == studentId).Select(p => new ClassRoomList { GradeName = p.ClassRoom.GradeName, ClassRoomID = p.ClassRoomID }).FirstOrDefault(); 
            ViewBag.gradeName = studentClassroom.GradeName;
            ViewBag.classRoomId = studentClassroom.ClassRoomID;
            var subjects = db.Subjects.Include(s => s.ClassRoom).Where(c => c.ClassRoomID == studentClassroom.ClassRoomID);
            var subjectList = new List<SubjectVM>();
            var termList = db.Terms.ToList();
            foreach (var subject in subjects)
            {
                var subjectVM = new SubjectVM();
                subjectVM.SubjectID = subject.SubjectID;
                subjectVM.SubjectName = subject.SubjectName;
                subjectVM.RequirementPercentage = subject.RequiredPercentage;
                subjectVM.termLists = termList;
                subjectList.Add(subjectVM);
            }
            return View(subjectList);
        }

        public ActionResult AssessmentList(int subjectId, int termId, string gradeName, string termName, string subjectName)
        {
            ViewBag.gradeName = gradeName;
            ViewBag.termName = termName;
            ViewBag.subjectName = subjectName;
            ViewBag.subjectId = subjectId;
            ViewBag.termId = termId;
            var assessments = db.Assessments.Include(a => a.Subject).Include(a => a.Term).Where(h => h.SubjectID == subjectId && h.TermID == termId);
            return View(assessments.ToList());
        }

        public JsonResult getHighest()
        {
            return new JsonResult { Data = new {highest="Maths" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public FileResult OpenPDFMedDoc(int id)
        {
            var fl = db.Assessments.Where(l=>l.AssessmentID==id).Select(u=>u.AssessmentDocument).FirstOrDefault();
            byte[] pdfByte = fl;
            return File(pdfByte, "application/pdf");
        }

        // POST: OnlineAssessments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OnlineAssessment onlineAssessment)
        {
            if (ModelState.IsValid)
            {
                db.OnlineAssessments.Add(onlineAssessment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssessmentID = new SelectList(db.Assessments, "AssessmentID", "AssessmentName", onlineAssessment.AssessmentID);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", onlineAssessment.StaffId);
            ViewBag.StID = new SelectList(db.students, "StID", "StudentName", onlineAssessment.StID);
            return View(onlineAssessment);
        }
        [HttpPost]
        public ActionResult Upload([Bind(Exclude = "AssessmentDocument")] OnlineAssessment onlineAssessment)
        {
            if (ModelState.IsValid)
            {
                var studentUser = User.Identity.Name;
                var studentId = db.students.Where(p => p.StudentEmail == studentUser).Select(o => o.StID).FirstOrDefault();
                byte[] assessmentData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase httpPostedFile = Request.Files["AssessmentDocument"];
                    using (var binary = new BinaryReader(httpPostedFile.InputStream))
                    {
                        assessmentData = binary.ReadBytes(httpPostedFile.ContentLength);
                    }
                }
                var assessment = db.OnlineAssessments.Where(s => s.AssessmentID == onlineAssessment.AssessmentID && s.StID == studentId).FirstOrDefault();
                if (assessment!=null)
                {
                    onlineAssessment.Document = assessmentData;
                    onlineAssessment.StID = studentId;
                    onlineAssessment.Feedback = "Pending";
                    onlineAssessment.StaffId = 0;
                    db.OnlineAssessments.Add(onlineAssessment);
                }
                else
                {
                    assessment.Document = onlineAssessment.Document;
                    db.Entry(assessment).State = EntityState.Modified;
                }
                db.SaveChanges();

            }
            return RedirectToAction("StudentSubjectList");
        }
                // GET: OnlineAssessments/Edit/5
                public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineAssessment onlineAssessment = db.OnlineAssessments.Find(id);
            if (onlineAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssessmentID = new SelectList(db.Assessments, "AssessmentID", "AssessmentName", onlineAssessment.AssessmentID);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", onlineAssessment.StaffId);
            ViewBag.StID = new SelectList(db.students, "StID", "StudentName", onlineAssessment.StID);
            return View(onlineAssessment);
        }

        // POST: OnlineAssessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnlineAssessmentID,StID,AssessmentID,StaffId,Document,Feedback")] OnlineAssessment onlineAssessment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineAssessment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssessmentID = new SelectList(db.Assessments, "AssessmentID", "AssessmentName", onlineAssessment.AssessmentID);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", onlineAssessment.StaffId);
            ViewBag.StID = new SelectList(db.students, "StID", "StudentName", onlineAssessment.StID);
            return View(onlineAssessment);
        }

        // GET: OnlineAssessments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineAssessment onlineAssessment = db.OnlineAssessments.Find(id);
            if (onlineAssessment == null)
            {
                return HttpNotFound();
            }
            return View(onlineAssessment);
        }

        // POST: OnlineAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnlineAssessment onlineAssessment = db.OnlineAssessments.Find(id);
            db.OnlineAssessments.Remove(onlineAssessment);
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
