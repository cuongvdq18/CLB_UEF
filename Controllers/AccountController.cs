using ClubPortalMS.CustomAuthentication;
using ClubPortalMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ClubPortalMS.Controllers
{
    
    public class AccountController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        #region Đăng nhập
        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            ThanhVien thanhViens = db.ThanhVien.Find(user.ID);
                            Session["AnhDaiDien"] = thanhViens.HinhDaiDien;
                            var userrole = from e in db.DBUserRoles
                                           join d in db.DBRoles on e.RoleID equals d.ID
                                           where e.UserID == user.ID
                                           select d;
                            Session["Role"] = userrole.ToList();
                        }
                        Session["Ten"] = user.FirstName;
                        Session["Ho"] = user.LastName;
                        Session["UserId"] = user.ID;
                        Session["UserName"] = user.UserName;
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            ID = user.ID,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.DBUserRoles.Select(r => r.RoleID.ToString()).ToList()
                        };
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            userModel.RoleName = (from u in db.DBRoles 
                                                  where userModel.RoleName.Contains(u.ID.ToString()) 
                                                  select u.Name).ToList();

                        }
                     

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                        
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index","Dashboard", new { area = "Profile" });
                    }
                }
                else {ViewBag.Message= "Tài khoản hoặc mật khẩu không đúng"; }
            }
            return View(loginView);
        }
        #endregion
        #region Đăng ký
        [HttpGet]
        public ActionResult Registration()
        {        
            ViewBag.IdKhoa = new SelectList(dbContext.Khoa, "ID", "TenKhoa");  
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserRegisterView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;
            ViewBag.IdKhoa = new SelectList(dbContext.Khoa, "ID", "TenKhoa", registrationView.IdKhoa);
            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationView.Email);
                if (!string.IsNullOrEmpty(userName))
                {

                    ModelState.AddModelError("Warning Email", "Email bạn nhập đã tồn tại");
                    return View(registrationView);
                }

                var Vuser = (CustomMembershipUser)Membership.GetUser(registrationView.UserName, false);
                if (Vuser != null)
                {
                    ModelState.AddModelError("Warning UserName", "Tên tài khoản bạn nhập đã tồn tại");
                    return View(registrationView);
                }
                else
                {
                   
                        string passWord = registrationView.Password + "A48BF46E-1V4F-58B4-2208-CQH7-U19JC5K2K3NV";
                        string pw = Processing.EncodePasswordToBase64(passWord);
                        string salt = pw.Substring(1, 10);
                        string H_Password = pw.Replace(salt, "");
                        registrationView.ActivationCode = Guid.NewGuid();
                        var user = new DBUser()
                        {
                            Username = registrationView.UserName,
                            FirstName = registrationView.FirstName,
                            LastName = registrationView.LastName,
                            Email = registrationView.Email,
                            HashedPassword = H_Password,
                            Salt = salt,
                            IsLocked = false,
                            EmailConfirmation = false,
                            DateCreated = DateTime.Now,
                            ActivationCode = registrationView.ActivationCode,
                        };
                        dbContext.DBUser.Add(user);
                        dbContext.SaveChanges();
                        var getIDUser = new ThanhVien()
                        {
                            User_ID = user.ID,
                            Khoa_ID = registrationView.IdKhoa,
                            MSSV = registrationView.MSSV,
                            Ten = registrationView.FirstName,
                            Ho = registrationView.LastName,
                            Mail = registrationView.Email,
                            HinhDaiDien = "/Hinh/HinhDaiDienNguoiDung/avatar_default.jpg"
                        };
                        dbContext.ThanhVien.Add(getIDUser);
                        dbContext.SaveChanges();
                    
                } 
                //Verification Email
                VerificationEmail(registrationView.Email, registrationView.ActivationCode.ToString());
                messageRegistration = "Tài khoản của bạn được tạo thành công. Vui lòng kiểm tra email của bạn để xác thực email";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Có lỗi đang xảy ra!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;
            
         
            return View(registrationView);
        }
        #endregion
        #region Gửi xác thực email sau khi đăng ký
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
                    ViewBag.Message = "Có lỗi gì đó !!";
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

            var fromEmail = new MailAddress("tuanbui2091@gmail.com", "Xác thực Email");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "25251325Cc";
            string subject = "Activation Account !";

            string body = "<br/> Truy cập link này để xác thực tài khoản của bạn" + "<br/><a href='" + link + "'> Xác thực tài khoản! </a>";

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
        #region đăng xuất
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("Login", "Account", null);
        }
        #endregion
        #region quên mật khẩu
        [HttpGet]
        public ActionResult ForgotPassWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassWord(ForgotPasswordView forgotPasswordViewModel)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;
            if (ModelState.IsValid)
            {
                var userName = Membership.GetUserNameByEmail(forgotPasswordViewModel.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    using (ApplicationDbContext dbContext = new ApplicationDbContext())
                    {
                        var userAccount = dbContext.DBUser.Where(u => u.Email.ToString().Equals(forgotPasswordViewModel.Email)).FirstOrDefault();
                        if (userAccount != null)
                        {
                            ResetPasswordEmail(userAccount.Email, userAccount.ActivationCode.ToString());
                            ViewBag.Message = "Thành công !! Vui lòng kiểm tra Email để đặt lại mật khẩu!";
                            statusRegistration = true;
                        } 
                    }
                }
                else
                {
                    ViewBag.Message = "Email bạn nhập không tồn tại !! Vui lòng kiểm tra Email của bạn!";
                    return View(forgotPasswordViewModel);
                 }
            }
            else
            {
                ModelState.AddModelError("", "Lỗi");
            }
            ViewBag.Status = statusRegistration;
            return View(forgotPasswordViewModel);
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string id,ResetPasswordView resetPasswordView)
        {
          
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    var userAccount = dbContext.DBUser.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

                    if (userAccount != null)
                    {
                        userAccount.HashedPassword = resetPasswordView.Password;
                        dbContext.SaveChanges();
                        ViewBag.Message = "Mật khẩu của bạn đã được lưu lại";
                        RedirectToAction("ForgotPasswordConfirmation");
                    }
                    else
                    {
                        ViewBag.Message = "Xảy ra lỗi !!";
                    }

                }
            }
         
            return View(resetPasswordView);
        }
        public void ResetPasswordEmail(string email, string activationCode)
        {

            var url = string.Format("/Account/ResetPassword/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("tuanbui2091@gmail.com", "Đặt lại mật khẩu");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "25251325Cc";
            string subject = "Đặt lại mật khẩu!";

            string body = "<br/> Truy cập liên kết để đặt lại mật khẩu của bạn" + "<br/><a href='" + link + "'> Đặt lại mật khẩu ! </a>";

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
        #region đăng nhập bằng google
        public void SignIn(string ReturnUrl = "/", string type = "")
        {
            if (!Request.IsAuthenticated)
            {
                if (type == "Google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Google");
                }
            }
        }
        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;

            var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);
            if (loginInfo == null)
            {
                return RedirectToAction("Index");
            }

            var user = dbContext.DBUser.FirstOrDefault(x => x.Email == loginInfo.emailaddress);
            if (user == null)
            {
                user = new DBUser
                {
                    Email = loginInfo.emailaddress,
                    FirstName = loginInfo.givenname,
                    Identifier = loginInfo.nameidentifier,
                    Username = loginInfo.name,
                    LastName = loginInfo.surname,
                    IsLocked = false,
                    EmailConfirmation = true,
                };
                dbContext.DBUser.Add(user);
                dbContext.SaveChanges();
                var getIDUser = new ThanhVien()
                {
                    User_ID = user.ID,
                    Ten = loginInfo.givenname,
                    Ho = loginInfo.surname,
                    Mail = loginInfo.emailaddress,
                    HinhDaiDien = "/Hinh/HinhDaiDienNguoiDung/avatar_default.jpg"
                };
                dbContext.ThanhVien.Add(getIDUser);
                dbContext.SaveChanges();
                return RedirectToAction("GetinfoMember", new { id = getIDUser.ID});
            }

            var ident = new ClaimsIdentity(
                    new[] { 
						// adding following 2 claim just for supporting default antiforgery provider
						new Claim(ClaimTypes.NameIdentifier, user.Email),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),						
						//new Claim(ClaimTypes.Role, user.DB)
                    },
                    CookieAuthenticationDefaults.AuthenticationType);
            ThanhVien thanhViens = dbContext.ThanhVien.Find(user.ID);
            Session["AnhDaiDien"] = thanhViens.HinhDaiDien;
            var userrole = from e in dbContext.DBUserRoles
                           join d in dbContext.DBRoles on e.RoleID equals d.ID
                           where e.UserID == user.ID
                           select d;
            Session["Role"] = userrole.ToList();
            Session["Ten"] = user.FirstName;
            Session["Ho"] = user.LastName;
            Session["UserId"] = user.ID;
            Session["UserName"] = user.Username;

            HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
            return RedirectToAction("Index", "Dashboard", new { area = "Profile" });
        }
        [HttpGet]
        public ActionResult GetinfoMember(int? id)
        {
            ViewBag.ID = id;
            ViewBag.IdKhoa = new SelectList(dbContext.Khoa, "ID", "TenKhoa");
            return View();
        }
        [HttpPost]
        public ActionResult GetinfoMember(GoogleLoginPlusViewModel tv, int? id)
        {
            ViewBag.IdKhoa = new SelectList(dbContext.Khoa, "ID", "TenKhoa", tv.IdKhoa);
            ThanhVien tttv = dbContext.ThanhVien.Find(tv.ID);
            tttv.Khoa_ID = tv.IdKhoa;
            tttv.MSSV = tv.MSSV;
            dbContext.SaveChanges();
            return RedirectToAction("GoogleLoginCallback");
        }
            #endregion
    }
}