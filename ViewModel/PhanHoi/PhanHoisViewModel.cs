using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.PhanHoi
{
    public class PhanHoisViewModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string NoiDung { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string TenNguoiPH { get; set; }
        public DateTime TGphanhoi { get; set; }
        public string TenCLB { get; set; }
    }
}