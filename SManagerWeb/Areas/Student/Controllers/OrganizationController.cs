using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class OrganizationController : Controller
    {
        private SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Organization
        public ActionResult Index()
        {
            try
            {
                var id = User.Identity.GetUserId();
                var student = db.Students.Find(id);
                if (student != null)
                {
                    var studentVM = Mapper.Map<StudentViewModel>(student);
                    ViewBag.Student = studentVM;
                    var organization = db.Organizations.Find(student.IDOrganization);
                    if (organization != null)
                    {
                        var organizationVM = Mapper.Map<OrganizationViewModel>(organization);
                        return View(organizationVM);
                    }
                }

                return View("Error");
            }catch(Exception ex)
            {
                return View("Error");

            }

        }

    }
}