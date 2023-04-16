using AutoMapper;
using Microsoft.AspNet.Identity;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Student.Controllers
{
    [Authorize(Roles ="Student")]
    public class TimetableController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Timetable
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
                    //period//
                    var periods = db.OPeriodLessons.Where(x => x.IDOrganization == student.IDOrganization).OrderBy(x => x.PeriodStartTime).ToList();
                    ViewBag.Periods = periods;

                    ///--find current year--//
                    var currentSemester = db.Semesters.Where(x => x.IsNow && x.SchoolYear.IDOrganization == student.IDOrganization).FirstOrDefault();
                    var currentYear = db.SchoolYears.Find(currentSemester.IDYear);
                    ///---get class--//
                    var classList = db.Classes.Where(x => x.IDYear == currentYear.ID).ToList();
                    Class @class = null;
                    foreach (var item in classList)
                    {
                        if (item.Studies.Where(x => x.IDStudent == student.IDStudent).FirstOrDefault() != null)
                        {
                            @class = item;
                            break;
                        }
                    }
                    if (@class != null)
                    {
                        var teaches = db.Teaches.Where(x => x.IDClass == @class.IDClass).OrderBy(x => x.Period.PeriodStartTime).ThenBy(x => x.WeekDay).ToList();
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