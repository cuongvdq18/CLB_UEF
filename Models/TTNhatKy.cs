
namespace ClubPortalMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TTNhatKy
    {
        public int ID { get; set; }
        public int IdThanhVien { get; set; }
        public DateTime? TGThamGia { get; set; }
        public string SKDaThamGia { get; set; }
        public int? IDSuKien { get; set; }
        public int? IDHoatDong { get; set; }
        public bool DiemDanh { get; set; }
        [ForeignKey("IdThanhVien")]
        public virtual ThanhVien ThanhVien { get; set; }
    }
}
