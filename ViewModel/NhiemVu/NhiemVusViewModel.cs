using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.NhiemVu
{
    public class NhiemVusViewModel
    {
        public int ID { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public string TenFile { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string CauLacBo { get; set; }
    }
}