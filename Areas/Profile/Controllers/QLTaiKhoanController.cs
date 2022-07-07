using ClubPortalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    public class QLTaiKhoanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        #region trang xem tài khoản của user
        //Trang chỉnh sửa profile của user
        public ActionResult Index(int? id)
        {
            int UserID = Convert.ToInt32(Session["UserId"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBUser dBUser = db.DBUser.Find(id);
            if (dBUser == null)
            {
                return HttpNotFound();
            }
            return View(dBUser);
        }
        #endregion
        #region đổi password
        //Chỉnh sửa password
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(int? id,ChangePasswordModel changePasswordModel)
        {
            int UserID = Convert.ToInt32(Session["UserId"]);
            id = UserID;
            if (ModelState.IsValid)
            {
              
                var userAccount = db.DBUser.Where(u => u.ID.ToString().Equals(id.ToString())).FirstOrDefault();
                if (userAccount != null)
                {
                    string passWord = changePasswordModel.OldPassword + "A48BF46E-1V4F-58B4-2208-CQH7-U19JC5K2K3NV";
                    string pw = Processing.EncodePasswordToBase64(passWord);
                    string salt = pw.Substring(1, 10);
                    string H_Password = pw.Replace(salt, "");
                    if (H_Password.Equals(userAccount.HashedPassword))
                    {
                        string newpassWord = changePasswordModel.NewPassword + "A48BF46E-1V4F-58B4-2208-CQH7-U19JC5K2K3NV";
                        string pws = Processing.EncodePasswordToBase64(newpassWord);
                        string salts = pws.Substring(1, 10);
                        string H_Passwords = pws.Replace(salts, "");
                        userAccount.HashedPassword = H_Passwords;
                        userAccount.Salt = salt;
                        db.SaveChanges();
                       ViewBag.Message= "Mật khẩu của tài khoản đổi thành công";
                    }
                    else
                    {
                       ViewBag.Messages = "Mật khẩu của tài khoản hiện tại không chính xác";
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
           
            return View(changePasswordModel);
        }
        #endregion
        #region Sửa Email
        //chỉnh sửa Email
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeEmail(int? id,ChangeEmailModel changeEmailModel)
        {
            string userName = Membership.GetUserNameByEmail(changeEmailModel.NewEmail);
            if (!string.IsNullOrEmpty(userName))
            {

                ModelState.AddModelError("Warning Email", "Xin lỗi Email bạn nhập đã tồn tại");
                return View(changeEmailModel);
            }
            int UserID = Convert.ToInt32(Session["UserId"]);
            id = UserID;
            if (ModelState.IsValid)
            {
                var userAccount = db.DBUser.Where(u => u.ID.ToString().Equals(id.ToString())).FirstOrDefault();
                if (userAccount != null)
                {
                    string passWord = changeEmailModel.OldPassword + "A48BF46E-1V4F-58B4-2208-CQH7-U19JC5K2K3NV";
                    string pw = Processing.EncodePasswordToBase64(passWord);
                    string salt = pw.Substring(1, 10);
                    string H_Password = pw.Replace(salt, "");
                    if (H_Password.Equals(userAccount.HashedPassword))
                    {
                        userAccount.Email = changeEmailModel.NewEmail;
                        userAccount.EmailConfirmation = false;
                        db.SaveChanges();
                        VerificationEmail(changeEmailModel.NewEmail, userAccount.ActivationCode.ToString());
                        ViewBag.Message = "Đổi email thành công!! Một email đã được gửi đến email của bạn để xác thực, vui lòng kiểm tra và xác thực email";
                    }
                    else
                    {
                       ViewBag.Messages= "Mật khẩu bạn nhập không chính xác!!! Vui lòng kiểm tra lại";
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return View(changeEmailModel);
        }
        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var userAccount = dbContext.DBUser.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

                if (userAccount != null)
                {
                    userAccount.EmailConfirmation = true;
                    dbContext.SaveChanges();
                    statusAccount = true;
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                }

            }
            ViewBag.Status = statusAccount;
            return View();
        }
        [NonAction]
        public void VerificationEmail(string email, string activationCode)
        {

            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("tuanbui2091@gmail.com", "Xác nhận Email");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "25251325Cc";
            string subject = "Xác nhận Email";

            string body = "<br/> Nhân vào link để xác nhận rằng email bạn vừa sửa đổi là email của bạn" + "<br/><a href='" + link + "'> Xác nhận Email </a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

                smtp.Send(message);

        }
        #endregion
        #region Xác thực email
  
        public ActionResult ConfirmButton(int? id)
        {
            int UserID = Convert.ToInt32(Session["UserId"]);
            id = UserID;
            if (ModelState.IsValid)
            {
                var userAccount = db.DBUser.Where(u => u.ID.ToString().Equals(id.ToString())).FirstOrDefault();
                if (userAccount != null)
                {
                    if (userAccount.EmailConfirmation!=true)
                    {
                        VerificationEmail(userAccount.Email, userAccount.ActivationCode.ToString());
                        ModelState.AddModelError("", "Đã gửi email xác nhận đến tài khoản Email của bạn, Vui lòng kiếm tra và xác nhận");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email của bạn đã được xác nhận");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View();
        }
        #endregion
    }
}