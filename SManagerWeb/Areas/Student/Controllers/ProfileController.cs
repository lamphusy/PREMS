using Common.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProfileController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Student/Profile
        public ActionResult Index()
        {
            try
            {

                var userId = User.Identity.GetUserId();
                var student = db.Students.Find(userId);
                if (student != null)
                {
                    var studentVM = AutoMapper.Mapper.Map<StudentViewModel>(student);
                    if (studentVM != null)
                    {
                        return View(studentVM);
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
        public async Task<ActionResult> EditStudent(string ID, StudentViewModel studentVM, HttpPostedFileBase avatar)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var student = db.Students.Find(currentId);
                if (student != null)
                {
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    //handler
                    var userByEmail = await UserManager.FindByEmailAsync(studentVM.Email);
                    if (userByEmail != null && userByEmail.UserName != studentVM.Username)
                    {
                        ModelState.AddModelError("email", "Email has been exist.");
                        return View("Index", studentVM);
                    }

                    var user = UserManager.FindByName(studentVM.Username);
                    if (user != null)
                    {
                        if (studentVM.FullName != null)
                            user.FullName = studentVM.FullName;
                        if (studentVM.Username != null)
                            user.UserName = studentVM.Username;
                        if (studentVM.Email != null)
                        {
                            user.Email = studentVM.Email;
                            user.EmailConfirmed = false;
                        }
                        if (studentVM.DayOfBirth != null)
                            user.DayOfBirth = studentVM.DayOfBirth;
                        if (studentVM.PhoneNumber != null)
                            user.PhoneNumber = studentVM.PhoneNumber;
                        if (studentVM.Address != null)
                            user.Address = studentVM.Address;

                        await UserManager.UpdateAsync(user);

                        if (studentVM.Password != null)
                        {
                            string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                            var result = await UserManager.ResetPasswordAsync(user.Id, token, studentVM.Password);

                        }

                        if (avatar != null)
                        {
                            student.AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Student/", user.Id);
                        }
                        student.Gender = studentVM.Gender;

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