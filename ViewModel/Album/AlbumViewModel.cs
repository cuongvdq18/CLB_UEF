using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.Album
{
    public class AlbumViewModel
    {
        public AlbumViewModel()
        {
            HinhAnh = "/Areas/Admin/Resource/HinhAnh/imguef.jfif";
            Video = "/Areas/Admin/Resource/HinhAnh/imguef.jfif";
        }
        public int ID { get; set; }
        /*[DisplayName("Tiêu Đề")]*/
        [Required(ErrorMessage = "Bạn cần nhập tiêu đề")]
        public string TieuDe { get; set; }
        [DisplayName("Tải hình ảnh lên")]
        [Required(ErrorMessage = "Bạn cần tải ảnh lên")]
        public string HinhAnh { get; set; }
        [DisplayName("Tải video lên")]
        public string Video { get; set; }
        [DisplayName("Mô Tả")]
        public string MoTa { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}