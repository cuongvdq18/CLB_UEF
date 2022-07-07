using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.User
{
    public class UserViewModel
    {
        public int ID { get; set; }
        [DisplayName("Họ và Tên Đệm")]
        public string FirstName { get; set; }
        [DisplayName("Tên")]
        public string LastName { get; set; }
        [DisplayName("Tài khoản")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Identifier { get; set; }
        public bool? EmailConfirmation { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        [DisplayName("Trạng Thái Hoạt Động")]
        public bool? IsLocked { get; set; }
        [DisplayName("Ngày Tạo")]
        public DateTime? DateCreated { get; set; }
        
        public bool? IsDeleted { get; set; }
        public Guid ActivationCode { get; set; }
        [DisplayName("Ngày Xóa")]
        public DateTime? NgayXoa { get; set; }
        [DisplayName("Người Xóa")]
        public int? UserDeleted { get; set; }
    }
}