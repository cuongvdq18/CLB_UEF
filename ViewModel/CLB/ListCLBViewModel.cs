using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.CLB
{
    public class ListCLBViewModel
    {
        public int ID { get; set; }
        public Nullable<int> IdLoaiCLB { get; set; }
        public string TenCLB { get; set; }
        public bool? TrangThai { get; set; }
        public DateTime? NgayThanhLap { get; set; }
        public string LienHe { get; set; }
        public string Mota { get; set; }
        public string FanPage { get; set; }
        public string Email { get; set; }

        public LoaiCLBViewModel LoaiCLB { get; set; }
    }
}