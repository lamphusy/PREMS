using AutoMapper;
using Microsoft.Ajax.Utilities;
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

namespace SManagerWeb.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ScoreController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Teacher/Score
        public ActionResult Index()
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var teacherVM = Mapper.Map<TeacherViewModel>(teacher);
                    ViewBag.Teacher = teacherVM;

                    var semesters = db.Semesters
                               .Where(x => x.SchoolYear.IDOrganization == teacher.IDOrganization)
                               .OrderByDescending(x => x.SchoolYear.NextYear)
                               .ThenBy(x => x.SemesterNum)
                               .Include("SchoolYear")
                               .ToList();
                    ViewBag.Semesters = semesters;

                    var classList = db.Classes.Where(x => x.IDOrganization == teacher.IDOrganization)
                        .ToList();
                    ViewBag.Classes = classList;

                    var subjects = db.Subjects.Where(x => x.IDOrganization == teacher.IDOrganization)
                        .OrderBy(x => x.IDSubject).ToList();
                    ViewBag.Subjects = subjects;

                    var typeScore = db.TypeScores.Where(x => x.IDOrganization == teacher.IDOrganization)
                        .OrderBy(x => x.PercentScore).ToList();
                    ViewBag.TypeScore = typeScore;
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");

            }
           
        }

        public ActionResult GetSubjectTeach(string IDClass, int IDSemester)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var schoolYear = db.Semesters.Find(IDSemester).IDYear;
                    var subjects = db.Teaches
                        .Where(x => x.IDSchoolYear == schoolYear && x.IDClass == IDClass && x.IDTeacher == teacher.IDUser)
                        .Select(x => x.Subject)
                        .Distinct()
                        .ToList();
                    return Json(JsonConvert.SerializeObject(subjects), JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return View("Error");

            }
            
        }

        public ActionResult GetScoreList(string IDClass, int IDSemester, string IDSubject)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var @class = db.Classes.Find(IDClass);
                    var students = @class.Studies.Select(x => x.IDStudent).ToList();

                    List<ScoreTeacherViewModel> listAll = new List<ScoreTeacherViewModel>();

                    foreach (string student in students)
                    {
                        ScoreTeacherViewModel scoreVM = new ScoreTeacherViewModel();
                        scoreVM.IDStudent = student;
                        scoreVM.StudentName = db.Students.Find(student).ApplicationUser.FullName;
                        var temp = db.ScoreDetails
                            .Where(x => x.IDStudent == student && x.IDSemester == IDSemester && x.IDSubject == IDSubject)
                            .OrderBy(x => x.TypeScore.PercentScore)
                            .Include("TypeScore")
                            .ToList();
                        scoreVM.ScoreDetails = temp;

                        listAll.Add(scoreVM);
                    }
                    return Json(JsonConvert.SerializeObject(listAll), JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return View("Error");

            }
            
        }

        [HttpPost]
        public void ChangeScoreList(string IDStudent, int IDSemester, string IDClass , string IDSubject, FormCollection form)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var check = db.Teaches.Where(x => x.IDTeacher == teacher.IDUser && x.IDSubject == IDSubject && x.IDClass == IDClass).FirstOrDefault();
                    if (check != null)
                    {
                        var scoreTypes = db.TypeScores.Where(x => x.IDOrganization == teacher.IDOrganization).ToList();
                        var scoreList = db.ScoreDetails.Where(x => x.IDSubject == IDSubject && x.IDSemester == IDSemester && x.IDStudent == IDStudent).ToList();
                        foreach (var type in scoreTypes)
                        {
                            if (form["type_" + type.IDScoreType] != null && form["type_" + type.IDScoreType].Length > 0)
                            {
                                var score = scoreList.Where(x => x.IDScoreType == type.IDScoreType).FirstOrDefault();
                                if (score != null)
                                {
                                    score.Score = float.Parse(form["type_" + type.IDScoreType]);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    var scoreNew = new ScoreDetail();
                                    scoreNew.IDStudent = IDStudent;
                                    scoreNew.IDSubject = IDSubject;
                                    scoreNew.IDSemester = IDSemester;
                                    scoreNew.IDScoreType = type.IDScoreType;
                                    scoreNew.Score = float.Parse(form["type_" + type.IDScoreType]);
                                    db.ScoreDetails.Add(scoreNew);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    ViewBag.Error = 0;
                    return;
                }

                return;
            }
            catch (Exception ex)
            {
                return;

            }
            
        }

        public ActionResult GetScoreListStudent(string IDStudent, int IDSemester, string IDSubject)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var scores = db.ScoreDetails
                        .Where(x => x.IDSubject == IDSubject && x.IDSemester == IDSemester && x.IDStudent == IDStudent)
                        .OrderBy(x => x.TypeScore.PercentScore)
                        .ToList();

                    return Json(JsonConvert.SerializeObject(scores), JsonRequestBehavior.AllowGet);
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