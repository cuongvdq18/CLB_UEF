using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.NhiemVu;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class NhiemVusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/NhiemVus
        public ActionResult Index()
        {
            List<NhiemVu> nv = db.NhiemVu.ToList();
            List<CLB> clb = db.CLB.ToList();
            var dsAlbum = from e in nv
                          join d in clb on e.IdCLB equals d.ID
                          select new NhiemVusViewModel
                          {
                              ID = e.ID,
                              TieuDe = e.TieuDe,
                              CauLacBo = d.TenCLB,
                              TenFile = e.TenFile,
                              MoTa = e.MoTa,
                              ContentType = e.ContentType,
                              File = e.File,
                              ThoiGianKetThuc=e.ThoiGianKetThuc
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
            var e = db.NhiemVu.SingleOrDefault(n => n.ID == id);
            var clb = db.CLB.SingleOrDefault(n => n.ID == e.IdCLB);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new NhiemVusViewModel
                            {
                                ID = e.ID,
                                TieuDe = e.TieuDe,
                                CauLacBo = clb.TenCLB,
                                TenFile = e.TenFile,
                                MoTa = e.MoTa,
                                ContentType = e.ContentType,
                                File = e.File,
                                ThoiGianKetThuc = e.ThoiGianKetThuc

                            };
            return View(viewModel);
        }
        public ActionResult DeleteConfirmed(NhiemVusViewModel nvvm)
        {
            NhiemVu data = db.NhiemVu.Find(nvvm.ID);
            var nvtv = db.NhiemVu_ThanhVien.Where(u => u.IdNv.ToString().Equals(nvvm.ID.ToString())).FirstOrDefault();
            db.NhiemVu_ThanhVien.Remove(nvtv);
            db.NhiemVu.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public FileResult DocumentDownloadFileDinhKem(int? id)
        {
            var nhiemVUs = db.NhiemVu.Where(u => u.ID == id).FirstOrDefault();
            return File(nhiemVUs.File, nhiemVUs.ContentType, nhiemVUs.TenFile);
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
