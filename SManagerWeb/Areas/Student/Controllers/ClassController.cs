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
    public class ClassController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Class
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
                        StudyListViewModel study = new StudyListViewModel();
                        study.IDClass = @class.IDClass;
                        study.ClassName = @class.Name;

                        var allStudyInClass = db.Studies.Where(x => x.IDClass == @class.IDClass).OrderBy(x => x.Student.ApplicationUser.FullName).ToList();
                        study.Students = Mapper.Map<List<Study>, List<StudyViewModel>>(allStudyInClass);
                        for (int i = 1; i <= study.Students.Count; i++)
                        {
                            study.Students.ElementAt(i - 1).IndexInClass = i;
                        }

                        return View(study);
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