
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.Sukien
{
    public class LoaiSuKienViewModel
    {
       

        public int ID { get; set; }
        [DisplayName("Loại sự Kiện")]
        [Required(ErrorMessage = "Bạn chưa nhập loại sự kiện mới")]
        public string TenLoaiSK { get; set; }
        [DisplayName("Trạng Thái")]
        public string TrangThai { get; set; }
    }
}