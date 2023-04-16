using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Controllers
{
    public class HomeController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        
        public ActionResult Index()
        {
            
            return View();

        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View("Error");
        }
    }
}