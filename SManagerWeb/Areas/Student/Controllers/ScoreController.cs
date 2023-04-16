using AutoMapper;
using Microsoft.AspNet.Identity;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class ScoreController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();

        // GET: Student/Score
        public ActionResult Index()
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);

                if (student != null)
                {
                    var studentVM = Mapper.Map<StudentViewModel>(student);
                    ViewBag.Student = studentVM;

                    var semesters = db.Semesters
                        .Where(x => x.SchoolYear.IDOrganization == student.IDOrganization)
                        .OrderByDescending(x => x.SchoolYear.NextYear)
                        .ThenBy(x => x.SemesterNum)
                        .Include("SchoolYear")
                        .ToList();
                    ViewBag.Semesters = semesters;

                    var subjects = db.Subjects
                        .Where(x => x.IDOrganization == student.IDOrganization)
                        .OrderBy(x => x.IDSubject).ToList();
                    ViewBag.Subjects = subjects;

                    var typeScore = db.TypeScores
                        .Where(x => x.IDOrganization == student.IDOrganization)
                        .OrderBy(x => x.PercentScore).ToList();
                    ViewBag.TypeScore = typeScore;

                    return View();
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");

            }
      
        }

        public ActionResult GetScoreList(int IDSemester)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);

                if (student != null)
                {
                    var allSubject = db.Subjects.Where(x => x.IDOrganization == student.IDOrganization).ToList();
                    List<ScoreStudentViewModel> result = new List<ScoreStudentViewModel>();
                    foreach (var subject in allSubject)
                    {
                        ScoreStudentViewModel scoreVM = new ScoreStudentViewModel();
                        scoreVM.IDSubject = subject.IDSubject;
                        scoreVM.SubjectName = subject.SubjectName;
                        scoreVM.ScoreDetails = db.ScoreDetails
                            .Where(x => x.IDStudent == student.IDStudent && x.IDSubject == subject.IDSubject && x.IDSemester == IDSemester)
                            .OrderBy(x => x.TypeScore.PercentScore)
                            .ToList();
                        result.Add(scoreVM);
                    }
                    return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return View("Error");

            }

          
        }
    }
}