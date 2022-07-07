using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class LichTap_ThanhVien
    {
        public int ID { get; set; }
        public int IDTvien { get; set; }
        public int IdLT { get; set; }
        [ForeignKey("IDTvien")]
        public virtual ThanhVien ThanhVien { get; set; } 
        [ForeignKey("IdLT")]
        public virtual LichTap LichTap { get; set; }
    }
}