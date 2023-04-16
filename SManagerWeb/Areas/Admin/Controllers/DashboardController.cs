using Microsoft.Ajax.Utilities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            Statistics statistics = db.Statistics
                .Where(x => x.Month == month && x.Year == year).FirstOrDefault();
            var numberOfRegister = db.ORegister
                    .Where(x => x.RegisterDate.Month == month && x.RegisterDate.Year == year)
                    .Count();
            var numberOfTeacher = db.Teachers
                    .Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year)
                    .Count();
            var numberOfStudent = db.Teachers
                    .Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year)
                    .Count();
            var numberOfOrganization = db.Organizations
                   .Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year)
                   .Count();
            var profit = db.Organizations
                    .Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year && x.IsPaid == true)
                    .Count() * 599000;

            var listProfit = db.Statistics.Where(x => x.Year == year).OrderBy(x=>x.Month).ToList();
            ViewBag.ListProfit = listProfit;

            if (statistics != null)
            {
                statistics.NumOfRegister = numberOfRegister;
                statistics.NumOfOrganization = numberOfRegister;
                statistics.NumOfTeacher = numberOfTeacher;
                statistics.NumOfStudent = numberOfStudent;
                statistics.Profit = profit;
                db.SaveChanges();
                return View(statistics);
            }
            else
            {
                Statistics newStatistic = new Statistics();
                newStatistic.ID = "month" + month + "year" + year;
                newStatistic.Year = year;
                newStatistic.Month = month;
                newStatistic.Profit = profit;
                newStatistic.NumOfRegister = numberOfRegister;
                newStatistic.NumOfOrganization = numberOfOrganization;
                newStatistic.NumOfTeacher = numberOfTeacher;
                newStatistic.NumOfStudent = numberOfStudent;
                db.Statistics.Add(newStatistic);
                db.SaveChanges();
                return View(newStatistic);
            }
        }
    }
}