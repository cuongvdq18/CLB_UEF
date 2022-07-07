using System.ComponentModel;

namespace ClubPortalMS.ViewModel.LienHe
{
    public class LienHeViewModel
    {
        public int ID { get; set; }
        [DisplayName("Tiêu Đề")]
        public string TieuDe { get; set; }
        [DisplayName("Địa Chỉ")]
        public string DiaChi { get; set; }
        public string HotLine { get; set; }
        public string Email { get; set; }
        [DisplayName("Tên")]
        public string Ten { get; set; }
        [DisplayName("Nội Dung")]
        public string NoiDung { get; set; }
        [DisplayName("Đã xử lý")]
        public bool? HoanThanh { get; set; }
    }
}