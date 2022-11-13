using Microsoft.AspNet.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Controllers
{
    [Authorize(Roles ="User")]
    public class OrganizationController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Organization
        public ActionResult Index(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if(organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    return View(organization);
                }
            }
            return View("Error");
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}