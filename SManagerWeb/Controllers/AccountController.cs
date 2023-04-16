using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SManagerWeb.Controllers
{
   
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        SchoolManagementDbContext db = new SchoolManagementDbContext();

        public AccountController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AccountController()
        {

        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, string role)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                
                if(user != null)
                {
                    if(UserManager.IsInRole(user.Id,role))
                    {
                        IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationProperties props = new AuthenticationProperties();
                        props.IsPersistent = false;
                        authenticationManager.SignIn(props, identity);

                        if (user.EmailConfirmed)
                        {
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                if (role == "User")
                                {
                                    return RedirectToAction("Index", "Dashboard");
                                }
                                else if (role == "Student")
                                {
                                    return RedirectToAction("Index", "Organization", new { area = "Student" });

                                }
                                else if (role == "Teacher")
                                {
                                    return RedirectToAction("Index", "Organization", new { area = "Teacher" });

                                }
                                else
                                {
                                    return RedirectToAction("Index", "Dashboard", new {area="Admin"});

                                }
                            }
                        }
                        else
                        {
                            return View("LinkToConfirmEmail", user);
                        }
                       
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong role.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
          
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await UserManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email has been exist.");
                    return View(model);
                }
                var userByUserName = await UserManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("username", "Username has been exist.");
                    return View(model);
                }
                var user = new ApplicationUser()
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = false,
                    DayOfBirth = model.DayOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address

                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var newUser = await UserManager.FindByIdAsync(user.Id);
                    
                    //add ORegister from ApplicationUser
                    var oRegisterObject = new ORegister();
                    oRegisterObject.RegisterDate = DateTime.Now;
                    oRegisterObject.IdCard = model.IDCard;
                    oRegisterObject.IdApplicationUser = newUser.Id;

                    db.ORegister.Add(oRegisterObject);
                    db.SaveChanges();
                    //----

                    if (newUser != null)
                        await UserManager.AddToRolesAsync(newUser.Id, new string[] { "User" });

                    return RedirectToAction("HandleSendConfirmEmail", "Account", new { id = newUser.Id });
                }
                else
                {
                    ModelState.AddModelError("", "???");
                }
            }
          
            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async Task<ActionResult> HandleSendConfirmEmail(string id)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            return View("DisplayEmail");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public async Task<ActionResult> ChangePassword(string UserId,string OldPassword, string NewPassword)
        {
            var result = await UserManager.ChangePasswordAsync(UserId, OldPassword,NewPassword);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard",new {message = "Change password success" });
            }
            else
            {
                return RedirectToAction("Index", "Dashboard", new { message = "Change password failed" });

            }
            
        }

        public async Task<ActionResult> ChangeProfile(UserViewModel profile)
        {
            var userId = profile.IdApplicationUser;
            var user = await db.ORegister.Include("ApplicationUser")
                .FirstAsync(x => x.IdApplicationUser == userId);

            if (user != null)
            {
                if (profile.EmailAddress != null && profile.EmailAddress != user.ApplicationUser.Email)
                {
                    var userByEmail = await UserManager.FindByEmailAsync(profile.EmailAddress);
                    if (userByEmail != null)
                    {
                        ModelState.AddModelError("email", "Email has been exist.");
                        return RedirectToAction("Index", "Dashboard", new { message = "Email has been exist." });
                    }
                    user.ApplicationUser.Email = profile.EmailAddress;
                    user.ApplicationUser.EmailConfirmed = false;
                }

                if (profile.PhoneNumber != null && profile.PhoneNumber != user.ApplicationUser.PhoneNumber)
                {
                    user.ApplicationUser.PhoneNumber = profile.PhoneNumber;
                    user.ApplicationUser.PhoneNumberConfirmed = false;
                }

                user.IdCard = profile.IdCard;
                user.Nation = profile.Nation;
                user.ApplicationUser.Address = profile.Address;
                
                user.ApplicationUser.DayOfBirth = profile.DayOfBirth;
                user.ApplicationUser.FullName = profile.FullName;

                await db.SaveChangesAsync();

                if(profile.Password != null
                    && profile.NewPassword != null 
                    && profile.ConfirmPassword != null)
                {
                    return RedirectToAction("ChangePassword", new { UserId = userId, OldPassword = profile.Password, NewPassword = profile.NewPassword });
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}