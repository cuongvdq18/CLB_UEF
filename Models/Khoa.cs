using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
    public class Khoa
    {
        public int ID { get; set; }
        public String TenKhoa { get; set; }
        public virtual ICollection<ThanhVien> ThanhVien { get; set; }
    }
}