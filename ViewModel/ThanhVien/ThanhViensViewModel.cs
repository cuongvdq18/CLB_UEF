using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.ThanhVien
{
    public class ThanhViensViewModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string MSSV { get; set; }
        public string Lop { get; set; }
        public string SDT { get; set; }
        public string Mail { get; set; }
        public string HinhDaiDien { get; set; }
        public string Khoa { get; set; }
        public string TenTK { get; set; }

    }
}