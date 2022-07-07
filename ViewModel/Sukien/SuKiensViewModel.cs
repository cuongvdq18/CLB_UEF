using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.Sukien
{
    public class SuKiensViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa tiêu đề")]
        public string TieuDeSK { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mô tả")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Thời gian bắt đầu đang thiếu")]
        public DateTime NgayBatDau { get; set; }
        [Required(ErrorMessage = "Thời gian kết thúc đang thiếu")]
        public DateTime NgayKetThuc { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung")]
        public string NoiDung { get; set; }
        public string KetQua { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa điểm")]
        public string DiaDiem { get; set; }
        public string TenFile { get; set; }
        public string ContentType { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn loại sự kiện")]
        public int IdLoaiSK { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn câu lạc bộ")]
        public int IdCLB { get; set; }
        public byte[] File { get; set; }
        public string LoaiSK { get; set; }
        public string CLB { get; set; }
    }
}