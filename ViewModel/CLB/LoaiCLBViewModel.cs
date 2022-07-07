using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.CLB
{
    public class LoaiCLBViewModel
    {
        public int IDLoaiCLB { get; set; }

        [DisplayName("Loại Câu Lạc Bộ")]
        [Required(ErrorMessage = "Bạn chưa nhập loại câu lạc bộ mới")]
        public string TenLoaiCLB { get; set; }
    }
}