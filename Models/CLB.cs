
namespace ClubPortalMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class CLB
    {
     
        public int ID { get; set; }
        public Nullable<int> IdLoaiCLB { get; set; }
        public string TenCLB { get; set; }
        public DateTime? NgayThanhLap { get; set; }
        public string LienHe { get; set; }
        public string Mota { get; set; }
        public string FanPage { get; set; }
        public string HinhCLB { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public string Email { get; set; }
        [ForeignKey("IdLoaiCLB")]
        public virtual LoaiCLB LoaiCLB { get; set; }
        public virtual ICollection<GioiThieu> GioiThieu { get; set; }
        public virtual ICollection<PhanHoi> PhanHoi { get; set; }
        public virtual ICollection<QLDSHoatDong> QLDSHoatDong { get; set; }
        public virtual ICollection<SuKien> SuKien { get; set; }
        public virtual ICollection<ThanhVien_CLB> ThanhVien_CLB { get; set; }
        public virtual ICollection<ThongBao> ThongBao { get; set; }
    }
}
