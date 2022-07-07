using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.Khoa
{
    public class KhoaViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa tên khoa mới")]
        [DisplayName("Tên Khoa")]
        public String TenKhoa { get; set; }
    }
}