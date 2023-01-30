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
    public class AbsenteeController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        
        //public ActionResult Index()
        //{
        //    var currentId = User.Identity.GetUserId();
        //    var teacher = db.Teachers.Find(currentId);
        //    if (teacher != null)
        //    {
        //        var teacherVM = Mapper.Map<StudentViewModel>(teacher);
        //        ViewBag.Teacher = teacherVM;
        //        var list = db.AbsenteeForms.Where(x => x.ID == teacher.IDUser)
        //            .OrderByDescending(x => x.CreateDate).ToList();

        //        return View(list);
        //    }
        //    return View("Error");
        //}
    }
}