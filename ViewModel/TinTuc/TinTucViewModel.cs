using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.TinTuc
{
    public class TinTucViewModel
    {
        public TinTucViewModel()
        {
            
            HinhAnhBaiViet = "/Areas/Admin/Resource/HinhAnh/imguef.jfif";
            HinhAnhChiTiet = "/Areas/Admin/Resource/HinhAnh/imguef.jfif";
        }

        public int ID { get; set; }
        [DisplayName("Tiêu Đề")]
        [Required(ErrorMessage = "Bạn cần nhập tiêu đề")]
        public string TieuDe { get; set; }
        [DisplayName("Mô Tả")]
        [Required(ErrorMessage = "Bạn cần nhập mô tả")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }
        [DisplayName("Nội Dung")]
        [Required(ErrorMessage = "Bạn cần nhập nội dung")]
        public string NoiDung { get; set; }
        public string KeyWord { get; set; }
        public string URL { get; set; }
        [DisplayName("Hình ảnh bài viết")]
        public string HinhAnhBaiViet { get; set; }
        [DisplayName("Hình Ảnh Chi tiết")]
        public string HinhAnhChiTiet { get; set; }
        [DisplayName("Ngày Đăng")]
        [Required(ErrorMessage = "Bạn cần nhập ngày đăng")]
        public DateTime NgayDang { get; set; }
        [DisplayName("Người Đăng")]
        [Required(ErrorMessage = "Bạn cần nhập tên tác giả")]
        public string TenNguoiDang { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageDetailFile { get; set; }
    }
}