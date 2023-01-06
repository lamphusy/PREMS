using AutoMapper;
using Common;
using Common.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.ViewModel;
using SManagerWeb.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.XPath;

namespace SManagerWeb.Controllers
{
    //[Authorize(Roles = "User")]
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
        [ValidateInput(false)]
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
                        var url = UploadImage.UploadOneImage(file, "~/Source/Organization/", ID);
                        organization.LogoPath = url;
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
                        var schoolYear = db.SchoolYears.Where(x => x.IDOrganization == ID).OrderByDescending(x => x.NextYear).ToList();
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

                        return Json(classListVM, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { ID = ID, status = "error" });
                }
            }
            return Json(new { ID = ID, status = "error" });
        }

        public ActionResult CreateClass(string ID, string date)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClassHandler(string ID, Class classObject, int year, string homeroomTeacher)
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

        public ActionResult DeleteClassHandler()
        {
            return null;
        }

        public ActionResult Teachers(string ID)
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

        public ActionResult CreateTeacher(string ID)
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

        //create teacher
        [HttpPost]
        public async Task<ActionResult> CreateTeacherHandler(string ID, TeacherViewModel teacherVM, HttpPostedFileBase avatar)
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

        [HttpPost]
        public async Task<ActionResult> ImportExcelToDatabaseTeacher(string ID, HttpPostedFileBase file)
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

        public ActionResult Students(string ID)
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

        public ActionResult CreateStudent(string ID)
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

        [HttpPost]
        public async Task<ActionResult> CreateStudentHandler(string ID, StudentViewModel studentVM, HttpPostedFileBase avatar)
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

        [HttpPost]
        public async Task<ActionResult> ImportExcelToDatabaseStudent(string ID, HttpPostedFileBase file)
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

        public ActionResult Study(string ID)
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
                        foreach(var @class in classList)
                        {
                            StudyListViewModel study = new StudyListViewModel();
                            study.IDClass = @class.IDClass;
                            study.ClassName = @class.Name;

                            var allStudyInClass = db.Studies.Where(x => x.IDClass == @class.IDClass).ToList();
                            study.Students = Mapper.Map<List<Study>, List<StudyViewModel>>(allStudyInClass);

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

        
        public void DownloadStudentList(string ID, string classID)
        {
            string currentId = User.Identity.GetUserId();
            var organization = db.Organizations.Find(ID);
            if (organization != null)
            {
                var check = db.UserOwnOrganizations.Where(x => x.IdOrganization == ID && x.IdORegister == currentId).FirstOrDefault();
                if (check != null)
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
                            string filename = "List of student in " + year + " of " + className +".docx";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            Response.TransmitFile(FilePath);
                            Response.End();
                        }
                    }
                }
            }
        }
    }
}