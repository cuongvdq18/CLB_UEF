using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.User
{
    public class UserRoleViewModel
    {
        public int ID { get; set; }
        [DisplayName("Tài Khoản")]
        public int UserID { get; set; }
        [DisplayName("Vai Trò")]
        public int RoleID { get; set; }
    }
}