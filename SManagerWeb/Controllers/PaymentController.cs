using Common;
using Common.Util;
using Microsoft.AspNet.Identity;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SManagerWeb.Controllers
{
    [Authorize(Roles ="User")]
    public class PaymentController : Controller
    {
        SchoolManagementDbContext db = new SchoolManagementDbContext();
        // GET: Payment
        public ActionResult Index(string ID)
        {
            var organization = db.Organizations.Find(ID);
            if(organization != null)
            {
                return View(organization);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Payment(string ID)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            VnPayLibrary pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", "59900000"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "en"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", ID); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                VnPayLibrary pay = new VnPayLibrary();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string OrganizationID = Request.QueryString["vnp_OrderInfo"];
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
                string vnp_BankCode = Request.QueryString["vnp_BankCode"];
                double vnp_Amount = double.Parse(Request.QueryString["vnp_Amount"]) / 100;
                DateTime vnp_PayDate = DateTime.ParseExact(Request.QueryString["vnp_PayDate"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Status = "1";

                        var orga = db.Organizations.Find(OrganizationID);
                        if (orga != null)
                        {
                            orga.IsPaid = true;
                        }
                        db.SaveChanges();

                        Receipt receipt = CreateReceipt(User.Identity.GetUserId(), OrganizationID, vnp_BankCode, vnp_Amount, vnp_PayDate);
                        ReceiptViewModel viewModel = AutoMapper.Mapper.Map<ReceiptViewModel>(receipt);
                        return View(viewModel);
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Status = "0";
                    }
                }
                else
                {
                    ViewBag.Status = "-1";
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

        public Receipt CreateReceipt(string UserID, string OrganizationID, string BankCode, double Amount, DateTime PayDate)
        {

            Receipt receipt = new Receipt();
            var id = GenerateIDHelper.ReceiptID();
            while (db.Receipts.Find(id) != null)
            {
                id = GenerateIDHelper.ReceiptID();
            }
            receipt.IDReceipt = id;
            receipt.IDAccount = UserID;
            receipt.IDOrganization = OrganizationID;
            receipt.PaymentDate = PayDate;
            receipt.BankCode = BankCode;
            receipt.Price = Amount;
            db.Receipts.Add(receipt);
            db.SaveChanges();
            if(receipt.ORegister == null)
            {
                receipt.ORegister = db.ORegister.Find(UserID);
            }
            return receipt;
        }
    }
}