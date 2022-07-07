using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ClubPortalMS.ViewModel.Poster
{
    public class PosterViewModel
    {
        public PosterViewModel()
        {
            HinhAnh = "/Areas/Admin/Resource/HinhAnh/imguef.jfif";
        }
        public int ID { get; set; }
        [DisplayName("Tên Poster")]
        [Required(ErrorMessage = "Bạn chưa tiêu đề")]
        public string TenPoster { get; set; }
        [DisplayName("Hình Ảnh Tải Lên")]
        [Required(ErrorMessage = "Bạn chưa tải ảnh nào")]
        public string HinhAnh { get; set; }
        [DisplayName("Trạng Thái")]
        [Required(ErrorMessage = "Bạn chưa đặt trạng thái cho poster")]
        public bool Status { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
   
    }
}