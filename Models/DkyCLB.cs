using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClubPortalMS.Models
{
	public class DkyCLB
	{
		public int ID { get; set; }
		public string TenCLB { get; set; }
		public int IDLoaiCLB { get; set; }
		public string LyDoThanhLap { get; set; }
		public int IdTvien { get; set; }
		[ForeignKey("IdTvien")]
		public virtual ThanhVien ThanhVien { get; set; }
	}
}