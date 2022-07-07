
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.ThongKe;
using CustomAuthorizationFilter.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Admin.Controllers
{

	[CustomAuthenticationFilter]
	[CustomAuthorize("Admin")]
	public class DashboardController : Controller
	{
		// GET: Admin/Dashboard
		private ApplicationDbContext db = new ApplicationDbContext();
		[HttpGet]
		public ActionResult Index()
		{
			List<HoatDong> hdctt = db.HoatDong.ToList();
			List<TinTuc> ttctt = db.TinTucs.ToList();
			var viewHoatDong = hdctt.Sum(s => s.LuotView);
			var viewTinTuc = ttctt.Sum(s => s.LuotView);
			var ViewAll = viewHoatDong + viewTinTuc;
			List<ThanhVien> tv = db.ThanhVien.ToList();
			List<SuKien> qlskhd = db.SuKien.ToList();
			List<Khoa> khoa = db.Khoa.ToList();
			var tv_khoa = from e in tv
						  join d in khoa on e.Khoa_ID equals d.ID
						  group e.ID by d.TenKhoa into g
						  select new 
						  {
							  soSV = g.Count(),
							  TenKhoa = g.Key
						  };

			var ids = db.ThanhVien.AsEnumerable().Select(r =>"Khóa "+r.MSSV.Substring(0, 2)).ToList();
			var khoahoc = (from d in tv
						select new
						{
						   ID = d.ID,
						   TenKhoaHoc = "Khóa "+d.MSSV.Substring(0,2)
						});
			
			var tv_khoahoc = from e in tv	
							 join d in khoahoc on e.ID equals d.ID
							 group e.ID by d.TenKhoaHoc into g
							 select new 
							 {
							   soSV = g.Count(),
							   TenKhoaHoc = g.Key
							 };
			List<CLB> clb = db.CLB.ToList();
			List<ThanhVien_CLB> tvclb = db.ThanhVien_CLB.ToList();
			var tv_clb = from e in tv
						 join d in tvclb on e.ID equals d.IDtvien
						 join f in clb on d.IDCLB equals f.ID
						 group e.ID by f.TenCLB into g
						 select new
						 {
							 soSV = g.Count(),
							 TenCLB = g.Key
						 };
			List<QLDSHoatDong> qldshd = db.QLDSHoatDong.ToList();
			var clb_hd = from e in qldshd
						 join d in clb on e.IdCLB equals d.ID
						 group e.ID by e.NgayBatDau.Year into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoNam =  g.Key
						 };
			var clb_sk = from e in qlskhd
						 join d in clb on e.IdCLB equals d.ID
						 group e.ID by e.NgayBatDau.Year into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoNam =  g.Key
						 };
			var DsCLB = from e in clb
						select new
						{
							IdCLBHD = e.ID,
							TenCLB = e.TenCLB
						};
			var DsCLBSK = from e in clb
						  select new
						  {
							  IdCLBSK = e.ID,
							  TenCLB = e.TenCLB
						  };
			var hd_loc = from e in qldshd
						 join d in clb on e.IdCLB equals d.ID
						 where d.ID == 1 && e.NgayBatDau.Year == 2021
						 group e.ID by e.NgayBatDau.Month  into g 
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoThang = g.Key
						 };
			var sk_loc = from e in qlskhd
						 join d in clb on e.IdCLB equals d.ID
						 where d.ID == 1 && e.NgayBatDau.Year == 2021
						 group e.ID by e.NgayBatDau.Month into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoThang = g.Key
						 };
			
			var getNameClubHD = (from e in clb
								where e.ID == 1
								select new { TenCLB = e.TenCLB }).FirstOrDefault();
			string nameClubHD = getNameClubHD.TenCLB;
			var getNameClubSK = (from e in clb
								where e.ID == 1
								select new { TenCLB = e.TenCLB }).FirstOrDefault();
			string nameClubSK = getNameClubSK.TenCLB;

			ViewBag.DsCLB = new SelectList(DsCLB, "IdCLBHD", "TenCLB");
			ViewBag.DsCLBSK = new SelectList(DsCLBSK, "IdCLBSK", "TenCLB");
			ViewBag.selectListYear = new SelectList(Enumerable.Range(2000, (DateTime.Now.Year - 2000) + 1), "YearHD");
			ViewBag.selectListYearSK = new SelectList(Enumerable.Range(2000, (DateTime.Now.Year - 2000) + 1), "YearSK");
			ViewBag.getNameClubHD = nameClubHD;
			ViewBag.getNameClubSK = nameClubSK;
			ViewBag.getYearClubHD = 2021;
			ViewBag.getYearClubSK = 2021;
			int sltv = db.ThanhVien.Count();
			int slclb = db.CLB.Count();
			int slsk = db.SuKien.Count();
			int slhd = db.QLDSHoatDong.Count();
			try
			{
				ViewBag.DataPoints = JsonConvert.SerializeObject(tv_khoa.ToList(), _jsonSetting);
				ViewBag.DataPoints1 = JsonConvert.SerializeObject(tv_khoahoc.ToList(), _jsonSetting);
				ViewBag.DataPoints2 = JsonConvert.SerializeObject(tv_clb.ToList(), _jsonSetting);
				ViewBag.DataPoints3 = JsonConvert.SerializeObject(clb_hd.ToList().OrderBy(d => d.HDtheoNam), _jsonSetting);
				ViewBag.DataPoints6 = JsonConvert.SerializeObject(clb_sk.ToList().OrderBy(d => d.HDtheoNam), _jsonSetting);
				ViewBag.DataPoints4 = JsonConvert.SerializeObject(hd_loc.ToList().OrderBy(d => d.HDtheoThang), _jsonSetting);
				ViewBag.DataPoints5 = JsonConvert.SerializeObject(sk_loc.ToList().OrderBy(d => d.HDtheoThang), _jsonSetting);
				ViewBag.sltv = sltv;
				ViewBag.slclb = slclb;
				ViewBag.slsk = slsk;
				ViewBag.slhd = slhd;
				ViewBag.LuotView = ViewAll;
				return View();
			}
			catch (System.Data.Entity.Core.EntityException)
			{
				return View("Error");
			}
			catch (System.Data.SqlClient.SqlException)
			{
				return View("Error");
			}
		}
		JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


		//HTTP POST
		[HttpPost]
		public ActionResult Index(ThongKeHDSKViewModel hdsk)
		{
			List<HoatDong> hdctt = db.HoatDong.ToList();
			List<TinTuc> ttctt = db.TinTucs.ToList();
			var viewHoatDong = hdctt.Sum(s => s.LuotView);
			var viewTinTuc = ttctt.Sum(s => s.LuotView);
			var ViewAll = viewHoatDong + viewTinTuc;
			if (hdsk.IdCLBHD == 0 || hdsk.YearHD == 0) 
			{
				hdsk.IdCLBHD = 1;
				hdsk.YearHD = 2021;
			}
			if (hdsk.IdCLBSK == 0 || hdsk.YearSK == 0)
			{
				hdsk.IdCLBSK = 1;
				hdsk.YearSK = 2021;
			}
			List<CLB> clb = db.CLB.ToList();
			List<QLDSHoatDong> qldshd = db.QLDSHoatDong.ToList();
			List<SuKien> qlskhd = db.SuKien.ToList();
			List<ThanhVien> tv = db.ThanhVien.ToList();
			List<Khoa> khoa = db.Khoa.ToList();
			var hd_loc = from e in qldshd
						 join d in clb on e.IdCLB equals d.ID
						 where d.ID == hdsk.IdCLBHD && e.NgayBatDau.Year == hdsk.YearHD
						 group e.ID by e.NgayBatDau.Month into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoThang = g.Key
						 };
			var sk_loc = from e in qlskhd
						 join d in clb on e.IdCLB equals d.ID
						 where d.ID == hdsk.IdCLBSK && e.NgayBatDau.Year == hdsk.YearSK
						 group e.ID by e.NgayBatDau.Month into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoThang = g.Key
						 };
			var DsCLB = from e in clb
						select new
						{
							IdCLBHD = e.ID,
							TenCLB = e.TenCLB
						};
			var getNameClubHD = (from e in clb
								where e.ID == hdsk.IdCLBHD
								select new { TenCLB = e.TenCLB }).FirstOrDefault();
			string nameClubHD = getNameClubHD.TenCLB;
			var getNameClubSK = (from e in clb
								where e.ID == hdsk.IdCLBSK
								select new { TenCLB = e.TenCLB }).FirstOrDefault();
			string nameClubSK = getNameClubSK.TenCLB;
			var ids = db.ThanhVien.AsEnumerable().Select(r => "Khóa " + r.MSSV.Substring(0, 2)).ToList();
			var DsCLBSK = from e in clb
						select new
						{
							IdCLBSK = e.ID,
							TenCLB = e.TenCLB
						};

			var tv_khoa = from e in tv
						  join d in khoa on e.Khoa_ID equals d.ID
						  group e.ID by d.TenKhoa into g
						  select new
						  {
							  soSV = g.Count(),
							  TenKhoa = g.Key
						  };

			var khoahoc = (from d in tv
						   select new
						   {
							   ID = d.ID,
							   TenKhoaHoc = "Khóa " + d.MSSV.Substring(0, 2)
						   });

			var tv_khoahoc = from e in tv
							 join d in khoahoc on e.ID equals d.ID
							 group e.ID by d.TenKhoaHoc into g
							 select new
							 {
								 soSV = g.Count(),
								 TenKhoaHoc = g.Key
							 };
			List<ThanhVien_CLB> tvclb = db.ThanhVien_CLB.ToList();
			var tv_clb = from e in tv
						 join d in tvclb on e.ID equals d.IDtvien
						 join f in clb on d.IDCLB equals f.ID
						 group e.ID by f.TenCLB into g
						 select new
						 {
							 soSV = g.Count(),
							 TenCLB = g.Key
						 };
			var clb_hd = from e in qldshd
						 join d in clb on e.IdCLB equals d.ID
						 group e.ID by e.NgayBatDau.Year into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoNam = g.Key
						 };
			var clb_sk = from e in qlskhd
						 join d in clb on e.IdCLB equals d.ID
						 group e.ID by e.NgayBatDau.Year into g
						 select new
						 {
							 soHD = g.Count(),
							 HDtheoNam = g.Key
						 };
			int sltv = db.ThanhVien.Count();
			int slclb = db.CLB.Count();
			int slsk = db.SuKien.Count();
			int slhd = db.QLDSHoatDong.Count();
			ViewBag.getYearClubHD = hdsk.YearHD;
			ViewBag.getYearClubSK = hdsk.YearSK;
			ViewBag.DsCLB = new SelectList(DsCLB, "IdCLBHD", "TenCLB");
			ViewBag.DsCLBSK = new SelectList(DsCLBSK, "IdCLBSK", "TenCLB");
			ViewBag.selectListYear = new SelectList(Enumerable.Range(2000, (DateTime.Now.Year - 2000) + 1), "YearHD");
			ViewBag.selectListYearSK = new SelectList(Enumerable.Range(2000, (DateTime.Now.Year - 2000) + 1), "YearSK");
			ViewBag.getNameClubHD = nameClubHD;
			ViewBag.getNameClubSK = nameClubSK;
			ViewBag.DataPoints = JsonConvert.SerializeObject(tv_khoa.ToList(), _jsonSetting);
			ViewBag.DataPoints1 = JsonConvert.SerializeObject(tv_khoahoc.ToList(), _jsonSetting);
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(tv_clb.ToList(), _jsonSetting);
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(clb_hd.ToList().OrderBy(d => d.HDtheoNam), _jsonSetting);
			ViewBag.DataPoints6 = JsonConvert.SerializeObject(clb_sk.ToList().OrderBy(d => d.HDtheoNam), _jsonSetting);
			ViewBag.DataPoints4 = JsonConvert.SerializeObject(hd_loc.ToList().OrderBy(d => d.HDtheoThang), _jsonSetting);
			ViewBag.DataPoints5 = JsonConvert.SerializeObject(sk_loc.ToList().OrderBy(d => d.HDtheoThang), _jsonSetting);
			ViewBag.sltv = sltv;
			ViewBag.slclb = slclb;
			ViewBag.slsk = slsk;
			ViewBag.slhd = slhd;
			ViewBag.LuotView = ViewAll;
			return View(hdsk);
		}
	}
}
