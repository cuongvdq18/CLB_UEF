using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.User
{
    public class RoleViewModel
    {
        public int ID { get; set; }
        [DisplayName("Tên Vai Trò")]
        [Required(ErrorMessage = "Bạn cần nhập tên vai trò")]
        public string Name { get; set; }
        [DisplayName("Mô Tả")]
        public string MoTa { get; set; }
    }
}