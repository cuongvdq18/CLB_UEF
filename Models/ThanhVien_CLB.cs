using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace ClubPortalMS.Models
{
    public class ThanhVien_CLB
    {
        public int ID { get; set; }
        public int IDtvien { get; set; } 
        public int IDCLB { get; set; }
        public int? IDRoles { get; set; }
        [ForeignKey("IDtvien")]
        public virtual ThanhVien ThanhVien { get; set; }
        [ForeignKey("IDCLB")]
        public virtual CLB CLB { get; set; }

    }
}