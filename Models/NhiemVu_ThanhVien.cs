using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class NhiemVu_ThanhVien
    {
        public int ID { get; set; }
        public int IdTVien { get; set; }
        public int IdNv { get; set; }
        public string TenFileNop { get; set; }
        public string ContentType { get; set; }
        public byte[] FileNop { get; set; }
        public string GhiChu { get; set; }

        [ForeignKey("IdTVien")]
        public virtual ThanhVien ThanhVien { get; set; }
        [ForeignKey("IdNv")]
        public virtual NhiemVu NhiemVu { get; set; }
    }
}