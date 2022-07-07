using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.ThongKe
{
    public class ThongKeHDSKViewModel
    {
        [Required(ErrorMessage = "Bạn cần chọn năm")]
        public int YearSK { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn năm")]
        public int YearHD { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn câu lạc bộ")]
        public int IdCLBHD { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn câu lạc bộ")]
        public int IdCLBSK { get; set; }
    }
}