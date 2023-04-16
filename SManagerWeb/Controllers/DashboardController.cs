using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Controllers
{
    [Authorize(Roles ="User")]
    public class DashboardController : Controller

    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        
        // GET: Dashboard
        public ActionResult Index(string message)
        {
            try
            {
                var idUser = User.Identity.GetUserId();
                var currentUser = db.ORegister.Include("ApplicationUser").
                    FirstOrDefault(x => x.IdApplicationUser == idUser);
                var currentUserViewModel = AutoMapper.Mapper.Map<UserViewModel>(currentUser);

                ViewBag.Result = message;

                return View(currentUserViewModel);
            }catch(Exception ex)
            {
                return View("Error");
            }
         
        }

        public ActionResult Organizations()
        {
            try
            {
                var idUser = User.Identity.GetUserId();
                var ownsId = db.UserOwnOrganizations.Where(x => x.IdORegister == idUser).Select(x => x.IdOrganization).ToList();
                if (ownsId.Any())
                {
                    var organizations = db.Organizations.Where(x => ownsId.Contains(x.IdOrganization)).ToList();
                    return View(organizations);
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

    }
}