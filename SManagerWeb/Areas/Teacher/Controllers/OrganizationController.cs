using AutoMapper;
using Microsoft.AspNet.Identity;
using Models.ViewModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class OrganizationController : Controller
    {
        private SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Organization
        public ActionResult Index()
        {
            try
            {
                var id = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(id);
                if (teacher != null)
                {
                    var teacherVM = Mapper.Map<TeacherViewModel>(teacher);
                    ViewBag.Teacher = teacherVM;
                    var organization = db.Organizations.Find(teacher.IDOrganization);
                    if (organization != null)
                    {
                        var organizationVM = Mapper.Map<OrganizationViewModel>(organization);
                        return View(organizationVM);
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