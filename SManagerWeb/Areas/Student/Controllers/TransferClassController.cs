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
    [Authorize(Roles = "Student")]
    public class TransferClassController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/TransferClass
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
                    var list = db.ClassTransferringForms.Where(x => x.IDStudent == student.IDStudent)
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

        public ActionResult CreateForm()
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);
                if (student != null)
                {
                    var studentVM = Mapper.Map<StudentViewModel>(student);
                    ViewBag.Student = studentVM;

                    var currentSemester = db.Semesters.Where(x => x.IsNow && x.SchoolYear.IDOrganization == student.IDOrganization).FirstOrDefault();
                    var currentYear = db.SchoolYears.Find(currentSemester.IDYear);
                    ///---get class--//
                    var classList = db.Classes.Where(x => x.IDYear == currentYear.ID).ToList();
                    ViewBag.Classes = classList;
                    return View();
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");

            }
         
        }

        [HttpPost]
        public ActionResult CreateFormHandler(string NewClass, ClassTransferringForm form)
        {
            try
            {
                var currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);
                if (student != null)
                {
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
                        ClassTransferringForm newForm = new ClassTransferringForm();
                        newForm.Status = 0;
                        newForm.IDStudent = currentId;
                        newForm.Title = form.Title;
                        newForm.IDOldClass = @class.IDClass;
                        newForm.IDNewClass = NewClass;
                        newForm.CreateDate = DateTime.Now;
                        newForm.Description = form.Description;
                        newForm.IDSemester = currentSemester.IdSemester;
                        newForm.IDOrganization = student.IDOrganization;

                        db.ClassTransferringForms.Add(newForm);

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error! You isn't a student in this school. Contact us to get help!");
                        return View("CreateForm", form);
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