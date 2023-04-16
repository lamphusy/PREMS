using AutoMapper;
using Microsoft.AspNet.Identity;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Teacher.Controllers
{
    [Authorize(Roles ="Teacher")]
    public class TimetableController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Teacher/Timetable
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
                    //period//
                    var periods = db.OPeriodLessons.Where(x => x.IDOrganization == teacher.IDOrganization).OrderBy(x => x.PeriodStartTime).ToList();
                    ViewBag.Periods = periods;

                    ///--find current year--//
                    var currentSemester = db.Semesters.Where(x => x.IsNow && x.SchoolYear.IDOrganization == teacher.IDOrganization).FirstOrDefault();
                    var currentYear = db.SchoolYears.Find(currentSemester.IDYear);
                    ///---get class--//

                    if (currentYear != null)
                    {
                        var teaches = db.Teaches.Where(x => x.IDSchoolYear == currentYear.ID && x.IDTeacher == teacher.IDUser)
                            .OrderBy(x => x.Period.PeriodStartTime).ThenBy(x => x.WeekDay).ToList();
                        var teachListVM = Mapper.Map<List<TeachViewModel>>(teaches);
                        return View(teachListVM);
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");

            }
           
        }
    }
}