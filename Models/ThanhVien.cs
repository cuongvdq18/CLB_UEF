namespace ClubPortalMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class ThanhVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string MSSV { get; set; }
        public string Lop { get; set; }
        public string SDT { get; set; }
        public string Mail { get; set; }
        public string HinhDaiDien { get; set; }
        public int? User_ID { get; set; }
        public int? Khoa_ID { get; set; }
        [ForeignKey("Khoa_ID ")]
        public virtual Khoa Khoa { get; set; }     
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [ForeignKey("User_ID")]
        public virtual DBUser DBUser { get; set; }
        public virtual ICollection<DkyCLB> DkyCLB { get; set; }
        public virtual ICollection<ThanhVien_CLB> ThanhVien_CLB { get; set; }
        public virtual ICollection<NhiemVu_ThanhVien> NhiemVu_ThanhVien { get; set; }
        public virtual ICollection<TTNhatKy> TTNhatKy { get; set; }
        public virtual ICollection<DangKy> DangKy { get; set; }
        public virtual ICollection<LichTap_ThanhVien> LichTap_ThanhViens { get; set; }


    }
}
