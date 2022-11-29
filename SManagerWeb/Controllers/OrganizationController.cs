using AutoMapper;
using Common;
using Common.Util;
using Microsoft.AspNet.Identity;
using Models;
using Models.Model;
using Models.ViewModel;
using SManagerWeb.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.XPath;

namespace SManagerWeb.Controllers
{
    [Authorize(Roles = "User")]
    public class OrganizationController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Organization

        public ActionResult Index(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        OrganizationViewModel viewModel = AutoMapper.Mapper.Map<OrganizationViewModel>(organization);
                        return View(viewModel);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
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
                organization.Information = model.Information;
                organization.FacebookLink = model.FacebookLink;
                organization.InstagramLink = model.InstagramLink;
                organization.LinkedinLink = model.LinkedinLink;
                organization.IsPaid = false;

                if (file != null)
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

        public ActionResult Information(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        OrganizationViewModel viewModel = AutoMapper.Mapper.Map<OrganizationViewModel>(organization);
                        return View(viewModel);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public PartialViewResult _NavigationBar(string ID)
        {
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                return PartialView(ID);
            }
            return null;
        }

        public ActionResult EditInformation(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        OrganizationViewModel viewModel = AutoMapper.Mapper.Map<OrganizationViewModel>(organization);
                        return View(viewModel);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditInformation(OrganizationViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Thêm organization
                var ID = model.IdOrganization;
                Organization organization = db.Organizations.Find(ID);
                if (organization != null)
                {

                    organization.Email = model.Email;
                    organization.Name = model.Name;
                    organization.FacebookLink = model.FacebookLink;
                    organization.InstagramLink = model.InstagramLink;
                    organization.LinkedinLink = model.LinkedinLink;
                    organization.Information = StringEncodeHelper.Base64Encode(model.Information);

                    if (file != null)
                    {
                        //------XOA ANH CU----------//
                        var delPath = Server.MapPath(organization.LogoPath);
                        FileInfo delFile = new FileInfo(delPath);
                        if (delFile.Exists)
                        {
                            delFile.Delete();
                        }


                        //------THEM ANH MOI----------//
                        var ext = Path.GetExtension(file.FileName);
                        string myfile = ID + ext;
                        // đường dẫn lưu vào database
                        var path = "~/Source/Organization/" + myfile;
                        organization.LogoPath = path;
                        //đường dẫn để lưu tạo file trên ổ cứng
                        var path2 = Path.Combine(Server.MapPath("~/Source/Organization"), myfile);
                        file.SaveAs(path2);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Information", "Organization", new { ID = ID });
                }
            }

            return View(model);
        }
        /// <summary>
        /// Regulation
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Regulation(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        RegulationViewModel model = new RegulationViewModel();
                        var shifts = db.OShifts.Where(x => x.IDOrganization == ID).ToList();
                        var periodLessons = db.OPeriodLessons.Where(x => x.IDOrganization == ID).OrderBy(a => a.PeriodStartTime).ToList();
                        model.ShiftList = AutoMapper.Mapper.Map<List<OShiftViewModel>>(shifts);
                        model.PeriodLessonList = AutoMapper.Mapper.Map<List<OPeriodLessonViewModel>>(periodLessons);
                        model.IdOrganization = ID;
                        if (model.ShiftList.Count == 0)
                        {
                            return RedirectToAction("CreateRegulation", "Organization", new { ID = organization.IdOrganization });
                        }
                        return View(model);

                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public ActionResult ChangeRegulation(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        RegulationViewModel model = new RegulationViewModel();
                        var shifts = db.OShifts.Where(x => x.IDOrganization == ID).ToList();
                        var periodLessons = db.OPeriodLessons.Where(x => x.IDOrganization == ID).OrderBy(a => a.PeriodStartTime).ToList();
                        model.ShiftList = AutoMapper.Mapper.Map<List<OShiftViewModel>>(shifts);
                        model.PeriodLessonList = AutoMapper.Mapper.Map<List<OPeriodLessonViewModel>>(periodLessons);
                        model.IdOrganization = ID;
                        return View(model);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public ActionResult CreateRegulation(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        ViewBag.ID = ID;
                        return View();
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRegulation(string ID, FormCollection form)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        var shifts = db.OShifts.Where(x => x.IDOrganization == ID);
                        var periodLessons = db.OPeriodLessons.Where(x => x.IDOrganization == ID).OrderBy(a => a.PeriodStartTime);
                        foreach (var period in periodLessons)
                        {
                            var periodID = period.ID;
                            period.PeriodName = form["period_name_" + periodID].ToString();
                            period.PeriodStartTime = TransferTime.TimeToValue(form["period_start_" + periodID].ToString());
                            period.PeriodEndTime = TransferTime.TimeToValue(form["period_end_" + periodID].ToString());
                        }
                        foreach (var shift in shifts)
                        {
                            var shiftID = shift.ID;
                            shift.ShiftName = form["shift_" + shiftID];
                        }
                        db.SaveChanges();
                        return RedirectToAction("Regulation", "Organization", new { ID = organization.IdOrganization });
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegulation(string ID, FormCollection form)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        int count = 1;
                        while (form["shift_" + count] != null)
                        {
                            OShift shift = new OShift();
                            shift.IDOrganization = ID;
                            shift.ShiftName = form["shift_" + count];
                            db.OShifts.Add(shift);
                            db.SaveChanges();

                            int p_count = 1;
                            while (form["name_" + count + "_" + p_count] != null)
                            {
                                OPeriodLesson period = new OPeriodLesson();
                                period.IDOrganization = ID;
                                period.IDShift = shift.ID;
                                period.PeriodName = form["name_" + count + "_" + p_count];
                                period.PeriodStartTime = TransferTime.TimeToValue(form["start_" + count + "_" + p_count]);
                                period.PeriodEndTime = TransferTime.TimeToValue(form["end_" + count + "_" + p_count]);
                                db.OPeriodLessons.Add(period);
                                db.SaveChanges();
                                p_count++;
                            }
                            count++;
                        }
                        return View("Regulation", new { ID = ID });
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }
        /// <summary>
        /// Semester
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Semester(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        var schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(a=>a.LastYear).ToList();
                        ViewBag.OrganizationID = ID;

                        List<SchoolYearViewModel> schoolYearVM = new List<SchoolYearViewModel>();
                        foreach (var item in schoolYears)
                        {
                            var semester = db.Semesters.Where(x => x.IDYear == item.ID).OrderBy(x => x.SemesterNum).ToList();
                            SchoolYearViewModel itemVM = new SchoolYearViewModel();
                            itemVM.SchoolYear = item;
                            itemVM.Semesters = Mapper.Map<List<SemesterViewModel>>(semester);
                            schoolYearVM.Add(itemVM);
                        }

                        return View(schoolYearVM);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public JsonResult SemesterFilter(string ID, string date)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        string[] dates = date.Split('-');
                        int preYear = int.Parse(dates[0]);
                        int nextYear = int.Parse(dates[1]);
                        
                        var schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID && x.LastYear == preYear
                                        && x.NextYear == nextYear).OrderByDescending(a => a.LastYear).ToList();
                        ViewBag.OrganizationID = ID;
                        List<SchoolYearViewModel> schoolYearVM = new List<SchoolYearViewModel>();
                        foreach (var item in schoolYears)
                        {
                            var semester = db.Semesters.Where(x => x.IDYear == item.ID).OrderBy(x=>x.SemesterNum).ToList();
                            SchoolYearViewModel itemVM = new SchoolYearViewModel();
                            itemVM.SchoolYear = item;
                            itemVM.Semesters = Mapper.Map<List<SemesterViewModel>>(semester);
                            schoolYearVM.Add(itemVM);
                        }
                        
                        return Json(schoolYearVM, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { ID = ID, status = "error" });
                }
            }
            return Json(new { ID = ID, status="error" });
        }
        ///---Phai sua lai-------/
        //[HttpPost]
        //public ActionResult DeleteSemester(string OrganizationID, int SemesterID)
        //{
        //    var currentUserId = User.Identity.GetUserId();
        //    var organization = db.Organizations.Find(OrganizationID);
        //    if (organization != null)
        //    {
        //        var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == OrganizationID && x.IdORegister == currentUserId).FirstOrDefault();
        //        if (checkLogin != null)
        //        {
        //            var isPaid = organization.IsPaid;
        //            if (isPaid)
        //            {
        //                var semester = db.Semesters.Find(SemesterID);
        //                if(semester != null)
        //                {
        //                    db.Semesters.Remove(semester);
        //                    db.SaveChanges();
        //                }

        //                return RedirectToAction("Semester", new {ID = OrganizationID});
        //            }
        //            else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
        //        }
        //    }
        //    return View("Error");
        //}

        public ActionResult CreateSemester(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        ViewBag.ID = ID;
                        var schoolYear = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(x=> x.NextYear).ToList();
                        ViewBag.SchoolYear = schoolYear;
                        return View();
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public ActionResult CreateSchoolYear(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        ViewBag.ID = ID;
                        return View();
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchoolYearHandler(string ID, SchoolYear schoolYear)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        ViewBag.ID = ID;
                        SchoolYear sy = new SchoolYear();
                        sy.IDOrganization = ID;
                        sy.NextYear = schoolYear.NextYear;
                        sy.LastYear = schoolYear.LastYear;

                        db.SchoolYears.Add(sy);
                        db.SaveChanges();

                        return RedirectToAction("Semester", new { ID = ID });
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSemesterHandler(string ID, Semester semester, int year)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        ViewBag.ID = ID;
                        Semester sm = new Semester();
                        sm.IDYear = year;
                        sm.SemesterNum = semester.SemesterNum;
                        sm.IsNow = semester.IsNow;

                        db.Semesters.Add(sm);
                        db.SaveChanges();

                        return RedirectToAction("Semester", new { ID = ID });
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }

        public ActionResult Class(string ID)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var checkLogin = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentUserId).FirstOrDefault();
                if (checkLogin != null)
                {
                    var isPaid = organization.IsPaid;
                    if (isPaid)
                    {
                        var classList = db.Classes.Where(x => x.IDOrganization == ID).ToList();
                        ViewBag.ID = ID;
                        return View(classList);
                    }
                    else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                }
            }
            return View("Error");
        }
    }
}