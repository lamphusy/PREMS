using AutoMapper;
using Common;
using Common.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using OfficeOpenXml.Style.XmlAccess;
using PagedList;
using SManagerWeb.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
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
            try
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

                            var students = db.Students.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.AllStudent = students.Count;

                            var teachers = db.Teachers.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.AllTeacher = teachers.Count;

                            var currentSemester = db.Semesters.Where(x => x.SchoolYear.IDOrganization == ID && x.IsNow == true).Include("SchoolYear").FirstOrDefault();
                            if (currentSemester != null)
                            {
                                var classes = db.Classes.Where(x => x.IDOrganization == ID && x.IDYear == currentSemester.SchoolYear.ID).ToList();
                                ViewBag.AllClass = classes.Count;
                            }
                            else
                            {
                                ViewBag.AllClass = 0;
                            }


                            var dateRegister = db.Organizations.Where(x => x.IdOrganization == ID).FirstOrDefault();
                            var now = Math.Floor((DateTime.Now - (DateTime)dateRegister.CreateDate).TotalDays);
                            ViewBag.Long = now;


                            return View(viewModel);
                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        public ActionResult Create()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(OrganizationViewModel model, HttpPostedFileBase file)
        {
            try
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
                    organization.Information = StringEncodeHelper.Base64Encode(model.Information);
                    organization.FacebookLink = model.FacebookLink;
                    organization.InstagramLink = model.InstagramLink;
                    organization.LinkedinLink = model.LinkedinLink;
                    organization.IsPaid = false;

                    var url = UploadImage.UploadOneImage(file, "~/Source/Organization/", newID);
                    organization.LogoPath = url;

                    db.Organizations.Add(organization);
                    db.SaveChanges();
                    CreateUserOwnOrganization(User.Identity.GetUserId(), newID);
                    return RedirectToAction("Index", "Payment", new { ID = newID });
                }
                return View(model);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public void CreateUserOwnOrganization(string UserID, string OrganizationID)
        {
            try
            {
                UserOwnOrganization owner = new UserOwnOrganization();
                owner.IdOrganization = OrganizationID;
                owner.IdORegister = UserID;
                db.UserOwnOrganizations.Add(owner);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return;
            }
        }

        public ActionResult Information(string ID)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
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
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditInformation(OrganizationViewModel model, HttpPostedFileBase file)
        {
            try
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
                            var url = UploadImage.UploadOneImage(file, "~/Source/Organization/", ID);
                            organization.LogoPath = url;
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index", "Organization", new { ID = ID });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }
        /// <summary>
        /// Regulation
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Regulation(string ID)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        public ActionResult ChangeRegulation(string ID)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult CreateRegulation(string ID)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRegulation(string ID, FormCollection form)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegulation(string ID, FormCollection form)
        {
            try
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
                            return RedirectToAction("Regulation", new { ID = ID });
                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }
        /// <summary>
        /// Semester
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Semester(string ID)
        {
            try
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
                            var schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(a => a.LastYear).ToList();
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult SemesterFilter(string ID, string date)
        {
            try
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
                            ViewBag.OrganizationID = ID;
                            List<SchoolYearViewModel> schoolYearVM = new List<SchoolYearViewModel>();
                            List<SchoolYear> schoolYears = new List<SchoolYear>();
                            if (String.Compare(date, "All", true) == 0)
                            {
                                schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(a => a.LastYear).ToList();
                            }
                            else
                            {
                                string[] dates = date.Split('-');
                                int preYear = int.Parse(dates[0]);
                                int nextYear = int.Parse(dates[1]);

                                schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID && x.LastYear == preYear
                                                && x.NextYear == nextYear).OrderByDescending(a => a.LastYear).ToList();
                            }


                            foreach (var item in schoolYears)
                            {
                                var semester = db.Semesters.Where(x => x.IDYear == item.ID).OrderBy(x => x.SemesterNum).ToList();
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
                return Json(new { ID = ID, status = "error" });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
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
            try
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
                            var schoolYear = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(x => x.NextYear).ToList();
                            ViewBag.SchoolYear = schoolYear;
                            return View();
                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult CreateSchoolYear(string ID)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchoolYearHandler(string ID, SchoolYear schoolYear)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSemesterHandler(string ID, Semester semester, int year)
        {
            try
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
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult Class(string ID)
        {
            try
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
                            var schoolYears = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(a => a.LastYear).ToList();
                            ViewBag.OrganizationID = ID;
                            List<ClassViewModel> classListVM = new List<ClassViewModel>();
                            foreach (var item in schoolYears)
                            {
                                var classListInYear = db.Classes.Where(x => x.IDYear == item.ID && x.IDOrganization == ID).OrderBy(x => x.Name).ToList();
                                ClassViewModel classVM = new ClassViewModel();
                                classVM.SchoolYear = item;
                                classVM.Classes = classListInYear;

                                classListVM.Add(classVM);
                            }

                            return View(classListVM);
                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        public JsonResult ClassFilter(string ID, string date)
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
                        List<ClassViewModel> classListVM = new List<ClassViewModel>();
                        foreach (var item in schoolYears)
                        {
                            var classListInYear = db.Classes.Where(x => x.IDYear == item.ID && x.IDOrganization == ID).OrderBy(x => x.Name).ToList();
                            ClassViewModel classVM = new ClassViewModel();
                            classVM.SchoolYear = item;
                            classVM.Classes = classListInYear;

                            classListVM.Add(classVM);
                        }
                        
                        return Json(JsonConvert.SerializeObject(classListVM), JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { ID = ID, status = "error" });
                }
            }
            return Json(new { ID = ID, status = "error" });
        }

        public ActionResult CreateClass(string ID, string date)
        {
            try
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
                            ViewBag.OrganizationID = ID;
                            var schoolYear = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(x => x.NextYear).ToList();
                            var teachers = db.Teachers.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.SchoolYear = schoolYear;
                            ViewBag.Teachers = teachers;
                            return View();

                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClassHandler(string ID, Class classObject, int year, string homeroomTeacher)
        {
            try
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
                            Class addClass = new Class();
                            addClass.IDYear = year;
                            addClass.IDHomeroomTeacher = homeroomTeacher;
                            addClass.Name = classObject.Name;
                            addClass.IDOrganization = ID;
                            addClass.IDClass = GenerateIDHelper.ClassID(ID);
                            addClass.Total = 0;

                            db.Classes.Add(addClass);
                            db.SaveChanges();

                            return RedirectToAction("Class", new { ID = ID });
                        }
                        else return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult DeleteClassHandler()
        {
            return null;
        }

        public ActionResult Teachers(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var teachers = db.Teachers.Where(x => x.IDOrganization == ID).ToList();
                            var teacherVM = Mapper.Map<List<Teacher>, List<TeacherViewModel>>(teachers);
                            return View(teacherVM);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult CreateTeacher(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            return View();
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        //create teacher
        [HttpPost]
        public async Task<ActionResult> CreateTeacherHandler(string ID, TeacherViewModel teacherVM, HttpPostedFileBase avatar)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        //handler
                        var userByEmail = await UserManager.FindByEmailAsync(teacherVM.Email);
                        if (userByEmail != null)
                        {
                            ModelState.AddModelError("email", "Email has been exist.");
                            return View("CreateTeacher", teacherVM);
                        }
                        var userByUserName = await UserManager.FindByNameAsync(teacherVM.Username);
                        if (userByUserName != null)
                        {
                            ModelState.AddModelError("username", "Username has been exist.");
                            return View("CreateTeacher", teacherVM);

                        }
                        var user = new ApplicationUser()
                        {
                            FullName = teacherVM.FullName,
                            UserName = teacherVM.Username,
                            Email = teacherVM.Email,
                            EmailConfirmed = true,
                            DayOfBirth = teacherVM.DayOfBirth,
                            PhoneNumber = teacherVM.PhoneNumber,
                            Address = teacherVM.Address

                        };
                        var result = await UserManager.CreateAsync(user, teacherVM.Password);
                        if (result.Succeeded)
                        {
                            var newUser = await UserManager.FindByIdAsync(user.Id);
                            var newTeacher = new Teacher()
                            {
                                IDUser = newUser.Id,
                                CreateBy = User.Identity.GetUserId(),
                                CreateDate = DateTime.Now,
                                IDCard = teacherVM.IDCard,
                                Degree = teacherVM.Degree,
                                StartJobDate = teacherVM.StartJobDate,
                                Specialization = teacherVM.Specialization,
                                CoefficientsSalary = 0,
                                Salary = 0,
                                AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Teacher/", newUser.Id),
                                IDOrganization = ID
                            };
                            db.Teachers.Add(newTeacher);
                            await db.SaveChangesAsync();

                            if (newUser != null)
                                await UserManager.AddToRolesAsync(newUser.Id, new string[] { "Teacher" });

                            return RedirectToAction("Teachers", new { ID = ID });
                        }
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult EditTeacher(string ID, string teacherID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var teacher = db.Teachers.Where(x => x.IDOrganization == ID
                            && x.ApplicationUser.UserName == teacherID).FirstOrDefault();
                            var teacherVM = Mapper.Map<TeacherViewModel>(teacher);
                            return View(teacherVM);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> EditTeacherHandler(string ID, TeacherViewModel teacherVM, HttpPostedFileBase avatar)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                            //handler
                            var userByEmail = await UserManager.FindByEmailAsync(teacherVM.Email);
                            if (userByEmail != null && userByEmail.UserName != teacherVM.Username)
                            {
                                ModelState.AddModelError("email", "Email has been exist.");
                                return View("EditTeacher", teacherVM);
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

                                var teacher = db.Teachers.Find(user.Id);
                                if (teacher != null)
                                {
                                    if (avatar != null)
                                    {
                                        teacher.AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Teacher/", user.Id);
                                    }
                                    teacher.Gender = teacherVM.Gender;
                                    teacher.Degree = teacherVM.Degree;
                                    teacher.StartJobDate = teacherVM.StartJobDate;
                                    teacher.IDCard = teacherVM.IDCard;
                                    teacher.Specialization = teacher.Specialization;
                                };

                                await db.SaveChangesAsync();


                                return RedirectToAction("Teachers", new { ID = ID });
                            }
                        }
                    }
                    else
                    {

                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> ImportExcelToDatabaseTeacher(string ID, HttpPostedFileBase file)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        //get list from excel
                        try
                        {
                            var teacherList = ExcelHelper.ImportTeacherExcel(currentId, file);

                            //insert to database
                            foreach (var teacher in teacherList)
                            {
                                var userByEmail = await UserManager.FindByEmailAsync(teacher.Email);
                                if (userByEmail != null)
                                {

                                    continue;
                                }
                                var userByUserName = await UserManager.FindByNameAsync(teacher.Username);
                                if (userByUserName != null)
                                {

                                    continue;
                                }
                                var user = new ApplicationUser()
                                {
                                    FullName = teacher.FullName,
                                    UserName = teacher.Username,
                                    Email = teacher.Email,
                                    EmailConfirmed = true,
                                    DayOfBirth = teacher.DayOfBirth,
                                    PhoneNumber = teacher.PhoneNumber,
                                    Address = teacher.Address

                                };
                                var result = await UserManager.CreateAsync(user, teacher.Password);
                                if (result.Succeeded)
                                {
                                    var newUser = await UserManager.FindByIdAsync(user.Id);
                                    var newTeacher = new Teacher()
                                    {
                                        IDUser = newUser.Id,
                                        CreateBy = User.Identity.GetUserId(),
                                        CreateDate = DateTime.Now,
                                        IDCard = teacher.IDCard,
                                        Degree = teacher.Degree,
                                        StartJobDate = teacher.StartJobDate,
                                        Specialization = teacher.Specialization,
                                        CoefficientsSalary = 0,
                                        Salary = 0,
                                        AvatarPath = null,
                                        IDOrganization = ID
                                    };
                                    db.Teachers.Add(newTeacher);
                                    await db.SaveChangesAsync();

                                    if (newUser != null)
                                        await UserManager.AddToRolesAsync(newUser.Id, new string[] { "Teacher" });
                                }
                            }
                            return RedirectToAction("Teachers", new { ID = ID });
                        }
                        catch (Exception ex)
                        {
                            return View("Error");
                        }
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult Subject(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.IDOrganization = ID;
                            var subjects = db.Subjects.Where(x => x.IDOrganization == ID).OrderBy(x => x.IDSubject).ToList();
                            return View(subjects);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        [HttpPost]
        public JsonResult CreateSubject(string ID, string subjectID, string subjectName, string Description)
        {
            string currentId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                if (check != null)
                {
                    try
                    {
                        ViewBag.IDOrganization = ID;
                        var subject = new Subject() { IDSubject = subjectID, Description = Description, IDOrganization = ID, SubjectName = subjectName };
                        db.Subjects.Add(subject);
                        db.SaveChanges();
                        return Json(new { result = "success", id = subjectID, name = subjectName, description = Description }
                        , JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { result = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Students(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var students = db.Students.Where(x => x.IDOrganization == ID).ToList();
                            var studentVM = Mapper.Map<List<Student>, List<StudentViewModel>>(students);
                            return View(studentVM);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

         
        }

        

        public ActionResult CreateStudent(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var semesterIsNow = db.Semesters.Where(x => x.IsNow && x.SchoolYear.IDOrganization == ID).FirstOrDefault();
                            var schoolYear = semesterIsNow.SchoolYear;
                            ViewBag.SchoolYear = schoolYear;
                            var classes = db.Classes.Where(x => x.SchoolYear == schoolYear).ToList();
                            ViewBag.Classes = classes;
                            return View();
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

           
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentHandler(string ID, StudentViewModel studentVM, HttpPostedFileBase avatar, string classNow)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        //handler
                        var userByEmail = await UserManager.FindByEmailAsync(studentVM.Email);
                        if (userByEmail != null)
                        {
                            ModelState.AddModelError("email", "Email has been exist.");
                            return View("CreateStudent", studentVM);
                        }
                        var userByUserName = await UserManager.FindByNameAsync(studentVM.Username);
                        if (userByUserName != null)
                        {
                            ModelState.AddModelError("username", "Username has been exist.");
                            return View("CreateStudent", studentVM);

                        }
                        var user = new ApplicationUser()
                        {
                            FullName = studentVM.FullName,
                            UserName = studentVM.Username,
                            Email = studentVM.Email,
                            EmailConfirmed = true,
                            DayOfBirth = studentVM.DayOfBirth,
                            PhoneNumber = studentVM.PhoneNumber,
                            Address = studentVM.Address

                        };

                        var result = await UserManager.CreateAsync(user, studentVM.Password);
                        if (result.Succeeded)
                        {
                            var newUser = await UserManager.FindByIdAsync(user.Id);
                            var newStudent = new Student()
                            {
                                IDStudent = newUser.Id,
                                CreateBy = User.Identity.GetUserId(),
                                CreateDate = DateTime.Now,
                                AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Student/", newUser.Id),
                                IDOrganization = ID,
                                Gender = studentVM.Gender
                            };
                            db.Students.Add(newStudent);
                            await db.SaveChangesAsync();

                            if (newUser != null)
                                await UserManager.AddToRolesAsync(newUser.Id, new string[] { "Student" });

                            Study study = new Study()
                            {
                                IDStudent = newStudent.IDStudent,
                                IDClass = classNow,
                            };

                            db.Studies.Add(study);
                            await db.SaveChangesAsync();

                            return RedirectToAction("Students", new { ID = ID });
                        }
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
       
        }

        public ActionResult EditStudent(string ID, string studentID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var student = db.Students.Where(x => x.IDOrganization == ID
                            && x.ApplicationUser.UserName == studentID).FirstOrDefault();
                            var studentVM = Mapper.Map<StudentViewModel>(student);
                            return View(studentVM);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
         
        }

        [HttpPost]
        public async Task<ActionResult> EditStudentHandler(string ID, StudentViewModel studentVM, HttpPostedFileBase avatar)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                            //handler
                            var userByEmail = await UserManager.FindByEmailAsync(studentVM.Email);
                            if (userByEmail != null && userByEmail.UserName != studentVM.Username)
                            {
                                ModelState.AddModelError("email", "Email has been exist.");
                                return View("EditStudent", studentVM);
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

                                var student = db.Students.Find(user.Id);
                                if (student != null)
                                {
                                    if (avatar != null)
                                    {
                                        student.AvatarPath = UploadImage.UploadOneImage(avatar, "~/Source/Student/", user.Id);
                                    }
                                    student.Gender = studentVM.Gender;
                                };

                                await db.SaveChangesAsync();


                                //{
                                //    FullName = studentVM.FullName,
                                //    UserName = studentVM.Username,
                                //    Email = studentVM.Email,
                                //    EmailConfirmed = true,
                                //    DayOfBirth = studentVM.DayOfBirth,
                                //    PhoneNumber = studentVM.PhoneNumber,
                                //    Address = studentVM.Address

                                //};
                                return RedirectToAction("Students", new { ID = ID });
                            }
                        }
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
         
        }

        [HttpPost]
        public async Task<ActionResult> ImportExcelToDatabaseStudent(string ID, HttpPostedFileBase file)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        //get list from excel
                        try
                        {
                            var studentList = ExcelHelper.ImportStudentHelper(currentId, file);

                            //insert to database
                            foreach (var student in studentList)
                            {
                                var userByEmail = await UserManager.FindByEmailAsync(student.Email);
                                if (userByEmail != null)
                                {
                                    //ModelState.AddModelError("email", "Email has been exist.");
                                    continue;
                                }
                                var userByUserName = await UserManager.FindByNameAsync(student.Username);
                                if (userByUserName != null)
                                {
                                    //ModelState.AddModelError("username", "Username has been exist.");
                                    continue;
                                }
                                var user = new ApplicationUser()
                                {
                                    FullName = student.FullName,
                                    UserName = student.Username,
                                    Email = student.Email,
                                    EmailConfirmed = true,
                                    DayOfBirth = student.DayOfBirth,
                                    PhoneNumber = student.PhoneNumber,
                                    Address = student.Address

                                };
                                var result = await UserManager.CreateAsync(user, student.Password);
                                if (result.Succeeded)
                                {
                                    var newUser = await UserManager.FindByIdAsync(user.Id);
                                    var newStudent = new Student()
                                    {
                                        IDStudent = newUser.Id,
                                        CreateBy = User.Identity.GetUserId(),
                                        CreateDate = DateTime.Now,
                                        Gender = student.Gender,
                                        AvatarPath = null,
                                        IDOrganization = ID
                                    };
                                    db.Students.Add(newStudent);
                                    await db.SaveChangesAsync();

                                    if (newUser != null)
                                        await UserManager.AddToRolesAsync(newUser.Id, new string[] { "Student" });
                                }
                            }
                            return RedirectToAction("Students", new { ID = ID });

                        }
                        catch (Exception ex)
                        {
                            return View("Error");
                        }
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
     
        }

        public ActionResult Study(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;

                            var schoolYear = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(x => x.NextYear).ToList();
                            var classList = db.Classes.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.SchoolYears = schoolYear;
                            ViewBag.Classes = classList;

                            List<StudyListViewModel> list = new List<StudyListViewModel>();
                            foreach (var @class in classList)
                            {
                                StudyListViewModel study = new StudyListViewModel();
                                study.IDClass = @class.IDClass;
                                study.ClassName = @class.Name;

                                var allStudyInClass = db.Studies.Where(x => x.IDClass == @class.IDClass).OrderBy(x => x.Student.ApplicationUser.FullName).ToList();
                                study.Students = Mapper.Map<List<Study>, List<StudyViewModel>>(allStudyInClass);
                                for (int i = 1; i <= study.Students.Count; i++)
                                {
                                    study.Students.ElementAt(i - 1).IndexInClass = i;
                                }

                                list.Add(study);
                            }

                            return View(list);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            

        }

        public ActionResult StudyCreate(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var currentSemester = db.Semesters.Where(x => x.IsNow == true && x.SchoolYear.IDOrganization == ID).FirstOrDefault();
                            if(currentSemester != null)
                            {
                                ViewBag.Classes = db.Classes.Where(x => x.IDOrganization == ID && x.IDYear == currentSemester.IDYear).ToList();
                                ViewBag.Students = db.Students.Where(x => x.IDOrganization == ID).ToList();
                            }

                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }


        }

        [HttpPost]
        public ActionResult StudyHandler(string ID, string IDStudent, string IDClass)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;

                            var idYear = db.Classes.Find(IDClass).IDYear;
                            Study study = db.Studies.Where(x => x.IDStudent == IDStudent && x.Class.IDYear == idYear).FirstOrDefault();
                            if(study == null)
                            {
                                Study newStudy = new Study()
                                {
                                    IDStudent = IDStudent,
                                    IDClass = IDClass,

                                };
                                db.Studies.Add(newStudy);
                                db.SaveChanges();
                            }
                            
                            return RedirectToAction("Study","Organization", new { ID = ID });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }


        }


        public void DownloadStudentList(string ID, string classID)
        {
            string currentId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                if (check != null)
                {
                    try
                    {
                        var findClass = db.Classes.Find(classID);


                        if (findClass != null)
                        {
                            var className = findClass.Name;
                            var year = findClass.SchoolYear.LastYear + "-" + findClass.SchoolYear.NextYear;
                            var students = findClass.Studies.ToList();
                            var studentVM = Mapper.Map<List<Study>, List<StudyViewModel>>(students);

                            string FilePath = WordHelper.ExportStudentListInClass(year, className, studentVM);
                            if (FilePath != null)
                            {
                                string filename = "List of student in " + year + " of " + className + ".docx";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                                Response.TransmitFile(FilePath);
                                Response.End();
                            }
                        }
                    }catch(Exception ex)
                    {

                    }
                  
                }
            }
        }

        public ActionResult Timetable(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var currentSemester = db.Semesters.Where(x => x.IsNow && x.SchoolYear.IDOrganization == ID).FirstOrDefault();
                            if (currentSemester != null)
                            {
                                var currentYear = db.SchoolYears.Find(currentSemester.IDYear);
                                var periodList = db.OPeriodLessons.Where(x => x.IDOrganization == ID).OrderBy(x => x.PeriodStartTime).ToList();
                                var classList = db.Classes.Where(x => x.IDYear == currentYear.ID).ToList();
                                var subjects = db.Subjects.Where(x => x.IDOrganization == ID).ToList();
                                var teachers = db.Teachers.Where(x => x.IDOrganization == ID).ToList();
                                var teachersVM = Mapper.Map<List<TeacherViewModel>>(teachers);

                                ViewBag.SchoolYear = currentYear;
                                ViewBag.Periods = periodList;
                                ViewBag.Classes = classList;
                                ViewBag.Subjects = subjects;
                                ViewBag.Teachers = teachersVM;
                            }
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        
        }

        
        public ActionResult GetTeachList(string ID, string classRoom)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            var teaches = db.Teaches.Where(x => x.IDClass == classRoom).OrderBy(x => x.Period.PeriodStartTime).ThenBy(x => x.WeekDay).ToList();
                            var teachListVM = Mapper.Map<List<TeachViewModel>>(teaches);
                            return Json(JsonConvert.SerializeObject(teachListVM), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        public ActionResult TeachHandler(string ID,string teacherId, int periodId, int weekday, string classID, string subjectId, int schoolYearID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            var checkTeach = db.Teaches.Where(x => x.IDClass == classID && x.IDPeriod == periodId && x.WeekDay == weekday).FirstOrDefault();
                            if (checkTeach != null)
                            {

                                checkTeach.IDTeacher = teacherId;

                                checkTeach.IDSubject = subjectId;
                            }
                            else
                            {
                                Teach teach = new Teach()
                                {
                                    IDClass = classID,
                                    IDTeacher = teacherId,
                                    IDPeriod = periodId,
                                    IDSchoolYear = schoolYearID,
                                    WeekDay = weekday,
                                    IDSubject = subjectId,
                                };
                                db.Teaches.Add(teach);
                            }
                            db.SaveChanges();
                            return new HttpStatusCodeResult(HttpStatusCode.OK);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
         
        }

        public ActionResult TransferClass( string ID )
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.IDOrganization = ID;

                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
           
        }

        public ActionResult _TransferClassPartial(int? page,string ID, int filter)
        {

            string currentId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                if (check != null)
                {
                    var checkPaid = organization.IsPaid;
                    if (checkPaid)
                    {
                        try
                        {
                            ViewBag.IDOrganization = ID;
                            ViewBag.ID = ID;
                            ViewBag.Filter = filter;
                            int pageSize = 10;
                            int pageIndex = 1;
                            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                            List<ClassTransferringForm> forms = new List<ClassTransferringForm>();
                            if (filter == 0)
                            {
                                forms = db.ClassTransferringForms.Where(x => x.IDOrganization == ID && x.Status == 0)
                                    .OrderByDescending(x => x.CreateDate).ToList();
                            }
                            else if (filter == 1)
                            {
                                forms = db.ClassTransferringForms.Where(x => x.IDOrganization == ID && x.Status == 1)
                                    .OrderByDescending(x => x.CreateDate).ToList();
                            }
                            else if (filter == -1)
                            {
                                forms = db.ClassTransferringForms.Where(x => x.IDOrganization == ID && x.Status == -1)
                                    .OrderByDescending(x => x.CreateDate).ToList();
                            }
                            return PartialView(forms.ToPagedList(pageIndex, pageSize));
                        }catch(Exception ex)
                        {

                        }
                       
                    }
                    else
                    {
                        return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                    }
                }
            }
            return View("Error");
            
        }

        public ActionResult TransferClassFormDetail(string ID, int IDForm)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var form = db.ClassTransferringForms.Find(IDForm);
                            if (form != null)
                            {
                                return View(form);
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
         
        }

        public ActionResult TransferClassHandler(string ID, int IDForm, bool accept)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var form = db.ClassTransferringForms.Find(IDForm);
                            if (form != null)
                            {
                                if (accept)
                                {
                                    form.Status = 1;

                                    var study = db.Studies.Where(x => x.IDClass == form.IDOldClass && x.IDStudent == form.IDStudent).FirstOrDefault();
                                    study.IDClass = form.IDNewClass;

                                    db.SaveChanges();
                                }
                                else
                                {
                                    form.Status = -1;

                                    db.SaveChanges();
                                }
                                return RedirectToAction("TransferClass", new { ID = ID });
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Payment", new { ID = organization.IdOrganization });
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
          
           
        }

        public ActionResult Score(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;

                            var semesters = db.Semesters
                                .Where(x => x.SchoolYear.IDOrganization == ID)
                                .OrderByDescending(x => x.SchoolYear.NextYear)
                                .ThenBy(x => x.SemesterNum)
                                .Include("SchoolYear")
                                .ToList();
                            ViewBag.Semesters = semesters;

                            var classList = db.Classes.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.Classes = classList;

                            var subjects = db.Subjects.Where(x => x.IDOrganization == ID).OrderBy(x => x.IDSubject).ToList();
                            ViewBag.Subjects = subjects;

                            var typeScore = db.TypeScores.Where(x => x.IDOrganization == ID).OrderBy(x => x.PercentScore).ToList();
                            ViewBag.TypeScore = typeScore;

                            return View();
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        public ActionResult RulesOfScore(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var types = db.TypeScores.Where(x => x.IDOrganization == ID).ToList();
                            ViewBag.Count = types.Count;
                            return View(types);
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        public ActionResult ChangeRulesOfScore(string ID, FormCollection form)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;
                            var types = db.TypeScores.Where(x => x.IDOrganization == ID).ToList();
                            //update---
                            foreach(var type in types)
                            {
                                var id = type.IDScoreType;
                                type.NameScore = form["name_" + id];
                                type.PercentScore = float.Parse(form["percent_" + id]);
                            }
                            //add----
                            int count = 0;
                            while (form["name_new_"+count] != null && form["percent_new_"+count]!= null)
                            {
                                TypeScore typeScore = new TypeScore();
                                typeScore.NameScore = form["name_new_" + count];
                                typeScore.PercentScore = float.Parse(form["percent_new_" + count]);
                                typeScore.IDOrganization = ID;
                                

                                db.TypeScores.Add(typeScore);
                                db.SaveChanges();

                                count++;

                            }
                                
                            return RedirectToAction("RulesOfScore", new {ID = ID});
                        }
                    }
                }
                return View("Error");
            }catch(Exception ex)
            {
                return RedirectToAction("RulesOfScore", new { ID = ID });
            }
           
        }

        public ActionResult GetScoreList(string ID, string IDClass, int IDSemester )
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {

                            var @class = db.Classes.Find(IDClass);
                            var students = @class.Studies.Select(x => x.IDStudent).ToList();

                            List<ScoreViewModel> listAll = new List<ScoreViewModel>();

                            foreach (string student in students)
                            {
                                ScoreViewModel scoreVM = new ScoreViewModel();
                                scoreVM.StudentID = student;
                                scoreVM.StudentName = db.Students.Find(student).ApplicationUser.FullName;
                                var temp = db.ScoreDetails
                                    .Where(x => x.IDStudent == student && x.IDSemester == IDSemester)
                                    .OrderBy(x => x.IDSubject)
                                    .Include("TypeScore")
                                    .ToList();
                                scoreVM.Subjects = temp.Select(x => x.IDSubject).Distinct().ToList();
                                foreach (var subject in scoreVM.Subjects)
                                {
                                    var scoreSubjects = temp.Where(x => x.IDSubject == subject).ToList();
                                    float score = 0;
                                    foreach (var i in scoreSubjects)
                                    {
                                        score += i.Score * i.TypeScore.PercentScore / 100;
                                    }
                                    scoreVM.Scores.Add(score);
                                }
                                listAll.Add(scoreVM);
                            }
                            return Json(JsonConvert.SerializeObject(listAll), JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

   
        public ActionResult GetScoreListStudent(string ID, string IDStudent, int IDSemester, string IDSubject)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            var scores = db.ScoreDetails
                                .Where(x => x.IDSubject == IDSubject && x.IDSemester == IDSemester && x.IDStudent == IDStudent)
                                .OrderBy(x => x.TypeScore.PercentScore)
                                .ToList();

                            return Json(JsonConvert.SerializeObject(scores), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }

        [HttpPost]
        public void ChangeScoreList(string ID, string IDStudent, int IDSemester, string IDSubject, FormCollection form)
        {
            string currentId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                if (check != null)
                {
                    var checkPaid = organization.IsPaid;
                    if (checkPaid)
                    {
                        try
                        {
                            var scoreTypes = db.TypeScores.Where(x => x.IDOrganization == ID).ToList();
                            var scoreList = db.ScoreDetails.Where(x => x.IDSubject == IDSubject && x.IDSemester == IDSemester && x.IDStudent == IDStudent).ToList();
                            foreach (var type in scoreTypes)
                            {
                                if (form["type_" + type.IDScoreType] != null && form["type_" + type.IDScoreType].Length > 0)
                                {
                                    var score = scoreList.Where(x => x.IDScoreType == type.IDScoreType).FirstOrDefault();
                                    if (score != null)
                                    {
                                        score.Score = float.Parse(form["type_" + type.IDScoreType]);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        var scoreNew = new ScoreDetail();
                                        scoreNew.IDStudent = IDStudent;
                                        scoreNew.IDSubject = IDSubject;
                                        scoreNew.IDSemester = IDSemester;
                                        scoreNew.IDScoreType = type.IDScoreType;
                                        scoreNew.Score = float.Parse(form["type_" + type.IDScoreType]);
                                        db.ScoreDetails.Add(scoreNew);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            ViewBag.Error = 0;
                            return;
                        }
                        catch(Exception ex)
                        {
                            ViewBag.Error = 0;
                            return;
                        }
                        
                    }
                }
            }
            ViewBag.Error = 1;
            return;
        }

        public ActionResult Absentee(string ID)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;

                            var absentees = db.AbsenteeForms
                                .Where(x => x.IDOrganization == ID)
                                .OrderByDescending(x => x.CreateDate).ToList();

                            return View(absentees);
                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
           
        }


        public ActionResult AbsenteeDetail(string ID, int IDForm)
        {
            try
            {
                string currentId = User.Identity.GetUserId();
                var organization = db.Organizations.Find(ID);
                if (organization != null)
                {
                    var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPaid = organization.IsPaid;
                        if (checkPaid)
                        {
                            ViewBag.OrganizationID = ID;

                            var absentee = db.AbsenteeForms
                                .Find(IDForm);
                            if (absentee != null)
                            {
                                absentee.Status = 1;
                                db.SaveChanges();
                                return View(absentee);
                            }


                        }
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
            
        }
    }
}