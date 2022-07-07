using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubPortalMS.ViewModel.ThongKe
{
    public class ThongKeSKCLBViewModel
    {
        [Required]
        public int Year { get; set; }

        [Required]
        public int IdCLB { get; set; }
    }
}