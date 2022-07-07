using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.LichHoatDong
{
    public class LichHoatDongViewModel
    {
        public int ID { get; set; }
        public string TieuDe { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public string CauLacBo { get; set; }
    }
}