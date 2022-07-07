using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class TinTuc
    {
        [Key]
        public int ID { get; set; }
        public string TieuDe{ get; set; }
        public string MoTa { get; set; }
        public string NoiDung { get; set; }
        public string KeyWord { get; set; }
        public string URL { get; set; }
        public string HinhAnhBaiViet { get; set; }
        public string HinhAnhChiTiet { get; set; }
        public DateTime NgayDang { get; set; }
        public string TenNguoiDang { get; set; }
        public int? LuotView { get; set; }


    }
}