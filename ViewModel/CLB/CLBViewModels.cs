using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.CLB
{
    public class CLBViewModels
    {
        public int ID { get; set; }
        [DisplayName("Loại Câu Lạc Bộ")]
        public string LoaiCLB { get; set; }
        [DisplayName("Tên Câu Lạc Bộ")]
        public string TenCLB { get; set; }
        [DisplayName("Trạng Thái")]
        public bool? TrangThai { get; set; }
        [DisplayName("Ngày Thành Lập")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        public DateTime? NgayThanhLap { get; set; }
        [DisplayName("Liên Hệ")]
        public string LienHe { get; set; }
        [DisplayName("Mô Tả")]
        public string Mota { get; set; }
        [DisplayName("Hình đại diện CLB")]
        public string HinhCLB { get; set; }
        public string FanPage { get; set; }
        public string Email { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}