using ClubPortalMS.ViewModel.CLB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.ThongBao
{
    public class ThongBaosViewModels
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa tiêu đề")]
        [DisplayName("Tiêu Đề")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mô tả")]
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }
        [DisplayName("CLB")]
        public string CLB { get; set; }
        [DisplayName("Ngày Thông Báo")]
        [Required(ErrorMessage = "Bạn cần nhập ngày thông báo")]
        public DateTime NgayThongBao { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung")]
        [DisplayName("Nội Dung")]
        public string NoiDung { get; set; }
        [DisplayName("Tên File")]
        public string TenFile { get; set; }
        public string ContentType { get; set; }
        [DisplayName("Tệp đính kèm")]
        public byte[] File { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn câu lạc bộ")]
        public int IdCLB { get; set; }
    }
}