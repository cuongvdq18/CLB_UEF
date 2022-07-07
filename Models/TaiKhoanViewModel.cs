using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace ClubPortalMS.Models
{
    public class ForgotPasswordView
    {
        [Required(ErrorMessage = "Bạn chưa nhập Email")]
        [EmailAddress]
        [Display(Name = "Nhập Email của bạn:")]
        public string Email { get; set; }
        public Guid ActivationCode { get; set; }
    }
    public class ResetPasswordView
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác thực bạn nhập không giống với mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    }
    public class GoogleLoginPlusViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn khoa")]
        public int IdKhoa { get; set; }
        [Required(ErrorMessage = "Bạn chưa mã số sinh viên")]
        public string MSSV { get; set; }
    }
    public class GoogleLoginViewModel
    {
        public string emailaddress { get; set; }
        public string name { get; set; }
        public string givenname { get; set; }
        public string surname { get; set; }
        public string nameidentifier { get; set; }

        internal static GoogleLoginViewModel GetLoginInfo(ClaimsIdentity identity)
        {
            if (identity.Claims.Count() == 0 || identity.Claims.FirstOrDefault
            (x => x.Type == ClaimTypes.Email) == null)
            {
                return null;
            }
            return new GoogleLoginViewModel
            {
                emailaddress = identity.Claims.FirstOrDefault
              (x => x.Type == ClaimTypes.Email).Value,
                name = identity.Claims.FirstOrDefault
              (x => x.Type == ClaimTypes.Email).Value,
                givenname = identity.Claims.FirstOrDefault
              (x => x.Type == ClaimTypes.GivenName).Value,
                surname = identity.Claims.FirstOrDefault
              (x => x.Type == ClaimTypes.Surname).Value,
                nameidentifier = identity.Claims.FirstOrDefault
              (x => x.Type == ClaimTypes.NameIdentifier).Value,
            };
        }
    }
    public class ManagePofileModelView
    {
        public bool HasPassword { get; set; }
        //public IList<UserLoginInfo> Logins { get; set; }
        public string Email { get; set; }
        // public bool BrowserRemembered { get; set; }
    }
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangeEmailModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress]
        [Display(Name = "Nhập Email mới:")]
        public string NewEmail { get; set; }
        public Guid ActivationCode { get; set; }
    }
    public class UserLoginView
    {
        [Required(ErrorMessage = "Bạn cần nhập tên đăng nhập")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class UserRegisterView
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập")]
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập họ")]
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn khoa")]
        public int IdKhoa { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã số sinh viên")]
        public string MSSV { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
        public Guid ActivationCode { get; set; }
    }
}