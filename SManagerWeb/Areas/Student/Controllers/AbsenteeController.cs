using AutoMapper;
using Microsoft.AspNet.Identity;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class AbsenteeController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
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
                    var shifts = db.OShifts.Where(x => x.IDOrganization == student.IDOrganization).OrderBy(x => x.ShiftName).ToList();
                    ViewBag.Shift = shifts;
                    var list = db.AbsenteeForms.Where(x => x.IDStudent == student.IDStudent)
                        .OrderByDescending(x => x.CreateDate).ToList();

                    return View(list);
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");

            }

           
        }

        [HttpPost]
        public ActionResult SendForm(string title, DateTime date, int shift, string description)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);
                if (student != null)
                {
                    var studentVM = Mapper.Map<StudentViewModel>(student);
                    ViewBag.Student = studentVM;
                    var currentSemester = db.Semesters.Where(x => x.SchoolYear.IDOrganization == student.IDOrganization &&
                    x.IsNow == true).FirstOrDefault();

                    AbsenteeForm absentee = new AbsenteeForm();
                    absentee.IDStudent = student.IDStudent;
                    absentee.IDShift = shift;
                    absentee.Description = description;
                    absentee.Date = date;
                    absentee.Title = title;
                    absentee.Status = 0;
                    absentee.CreateDate = DateTime.Now;
                    absentee.IDSemester = currentSemester.IdSemester;
                    absentee.IDOrganization = student.IDOrganization;

                    db.AbsenteeForms.Add(absentee);

                    db.SaveChanges();

                    return new HttpStatusCodeResult(HttpStatusCode.OK);


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