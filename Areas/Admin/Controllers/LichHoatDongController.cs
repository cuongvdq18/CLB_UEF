using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.LichHoatDong;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class LichHoatDongController : Controller
    {
        // GET: Admin/LichHoatDong
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/NhiemVus
        public ActionResult Index()
        {
            List<LichTap> nv = db.LichTap.ToList();
            List<CLB> clb = db.CLB.ToList();
            var dsAlbum = from e in nv
                          join d in clb on e.IdCLB equals d.ID
                          select new LichHoatDongViewModel
                          {
                              ID = e.ID,
                              TieuDe = e.TieuDe,
                              CauLacBo = d.TenCLB,
                              DiaDiem = e.DiaDiem,
                              NgayBatDau = e.NgayBatDau,
                              NgayKetThuc = e.NgayKetThuc
                          };

            return View(dsAlbum);
        }

        // GET: Admin/NhiemVus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.LichTap.SingleOrDefault(n => n.ID == id);
            var clb = db.CLB.SingleOrDefault(n => n.ID == e.IdCLB);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new LichHoatDongViewModel
            {
                ID = e.ID,
                TieuDe = e.TieuDe,
                CauLacBo = clb.TenCLB,
                DiaDiem = e.DiaDiem,
                NgayBatDau = e.NgayBatDau,
                NgayKetThuc = e.NgayKetThuc

            };
            return View(viewModel);
        }
        public ActionResult DeleteConfirmed(LichHoatDongViewModel lhd)
        {
            LichTap data = db.LichTap.Find(lhd.ID);
            var lttv = db.LichTap_ThanhVien.Where(u => u.IdLT.ToString().Equals(lhd.ID.ToString())).FirstOrDefault();
            db.LichTap_ThanhVien.Remove(lttv);
            db.LichTap.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}