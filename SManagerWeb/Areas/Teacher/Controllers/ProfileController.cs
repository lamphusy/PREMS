using Common.Util;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Models.ViewModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ProfileController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Profile
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(userId);
                if (teacher != null)
                {
                    var teacherVM = AutoMapper.Mapper.Map<TeacherViewModel>(teacher);
                    if (teacherVM != null)
                    {
                        return View(teacherVM);
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                return View("Error");

            }
           
        }

        [HttpPost]
        public async Task<ActionResult> EditTeacher(string ID, TeacherViewModel teacherVM, HttpPostedFileBase avatar)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(currentId);
                if (teacher != null)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    //handler
                    var userByEmail = await UserManager.FindByEmailAsync(teacherVM.Email);
                    if (userByEmail != null && userByEmail.UserName != teacherVM.Username)
                    {
                        ModelState.AddModelError("email", "Email has been exist.");
                        return View("Index", teacherVM);
                    }

                    var user = UserManager.FindByName(teacherVM.Username);
                    if (user != null)
                    {
                        if (teacherVM.FullName != null)
                            user.FullName = teacherVM.FullName;
                        if (teacherVM.Username != null)
                            user.UserName = teacherVM.Username;
                        if (teacherVM.Email != null)
                        {
                            user.Email = teacherVM.Email;
                            user.EmailConfirmed = false;
                        }
                        if (teacherVM.DayOfBirth != null)
                            user.DayOfBirth = teacherVM.DayOfBirth;
                        if (teacherVM.PhoneNumber != null)
                            user.PhoneNumber = teacherVM.PhoneNumber;
                        if (teacherVM.Address != null)
                            user.Address = teacherVM.Address;


                        await UserManager.UpdateAsync(user);

                        if (teacherVM.Password != null)
                        {
                            string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                            var result = await UserManager.ResetPasswordAsync(user.Id, token, teacherVM.Password);

                        }

                        if (avatar != null)
                        {
                            teacherVM.AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Student/", user.Id);
                        }
                        teacher.Gender = teacherVM.Gender;
                        teacher.Specialization = teacher.Specialization;
                        teacher.Degree = teacherVM.Degree;
                        teacher.IDCard = teacherVM.IDCard;
                        teacher.StartJobDate = teacherVM.StartJobDate;

                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
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