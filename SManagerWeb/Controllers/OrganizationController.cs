using Common;
using Microsoft.AspNet.Identity;
using Models;
using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        return View(organization);
                    }
                    else return RedirectToAction("Index", "Payment",new {ID = organization.IdOrganization});
                }
            }
            return View("Error");
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //Tạo id mới
                var newID = GenerateIDHelper.OrganizationID();
                while (db.Organizations.Find(newID) != null)
                {
                    newID = GenerateIDHelper.OrganizationID();
                }
                // Thêm organization
                Organization organization = new Organization();
                organization.IdOrganization = newID;
                organization.CreateDate = DateTime.Now;
                organization.CreateBy = User.Identity.Name;
                organization.PhoneNumber = model.PhoneNumber;
                organization.Email = model.Email;
                organization.Name = model.Name;
                organization.FacebookLink = model.FacebookLink;
                organization.InstagramLink = model.InstagramLink;
                organization.LinkedinLink = model.LinkedinLink;
                organization.IsPaid = false;
                
                if(file != null)
                {
                   
                    var ext = Path.GetExtension(file.FileName);
                    string myfile = newID + ext;     
                    // đường dẫn lưu vào database
                    var path = "~/Source/Organization/" + myfile;
                    organization.LogoPath = path;
                    //đường dẫn để lưu tạo file trên ổ cứng
                    var path2 = Path.Combine(Server.MapPath("~/Source/Organization"), myfile);
                    file.SaveAs(path2);
                }
                db.Organizations.Add(organization);
                db.SaveChanges();
                CreateUserOwnOrganization(User.Identity.GetUserId(), newID);
                return RedirectToAction("Index", "Payment", new { ID = newID });
            }
            return View(model);
        }

        public void CreateUserOwnOrganization(string UserID, string OrganizationID)
        {
            UserOwnOrganization owner = new UserOwnOrganization();
            owner.IdOrganization = OrganizationID;
            owner.IdORegister = UserID;
            db.UserOwnOrganizations.Add(owner);
            db.SaveChanges();
        }
        //public async Task<ActionResult> Payment()
        //{

        //}

        //public async Task<Boolean> PaymentHandler()
        //{

        //}
    }
}