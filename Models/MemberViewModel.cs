using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class ProfileViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Tên")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Họ")]
        public string Ho { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Mã số sinh viên")]
        public string MSSV { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Số lớp")]
        public string Lop { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Số điện thoại")]
        public string SDT { get; set; }
        public string HinhDaiDien { get; set; }
        public int? User_ID { get; set; }
        public int? Khoa_ID { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn tên khoa")]
        public string TenKhoa { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}