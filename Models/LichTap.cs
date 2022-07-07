using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class LichTap
    {
        public int ID { get; set; }
        public string TieuDe { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public int IdCLB { get; set; }
        public virtual ICollection<LichTap_ThanhVien> LichTap_ThanhViens { get; set; }
    }
}