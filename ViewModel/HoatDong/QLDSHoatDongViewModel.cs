using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.HoatDong
{
    public class QLDSHoatDongViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa tiêu đề")]
        public string ChuDe { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mô tả")]
        public string Mota { get; set; }
        [Required(ErrorMessage = "Thời gian bắt đầu đang thiếu")]
        public DateTime NgayBatDau { get; set; }
        [Required(ErrorMessage = "Thời gian kết thúc đang thiếu")]
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung")]
        public string NoiDung { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa điểm")]
        public string DiaDiem { get; set; }
        public string TenFile { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
        public string TenCLB { get; set; }
        public string LoaiHD { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn loại hoạt động")]
        public int IdLoaiHD { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn câu lạc bộ")]
        public int IdCLB { get; set; }
    }
}