
namespace ClubPortalMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class GioiThieu
    {
        [Key]
        public int ID { get; set; }
        public int IdCLB { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string LichSuHinhThanh { get; set; }
        [ForeignKey("IdCLB")]
        public virtual CLB CLB { get; set; }
    }
}
