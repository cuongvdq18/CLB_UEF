using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.ThanhVien;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class ThanhViensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ThanhViens
        public ActionResult CLB_TV()
        {
            List<CLB> clb = db.CLB.ToList();
            var ds = from i in clb
                     select new CLBDaThamGiaViewModel
                     {
                         TenCLB = i.TenCLB,
                         IDCLB = i.ID,
                         Mota = i.Mota,
                         HinhCLB = i.HinhCLB,
                         NgayThanhLap = i.NgayThanhLap
                     };
            return View(ds);
        }
        public ActionResult Index(int? id)
        {
            List<ThanhVien> tv = db.ThanhVien.ToList();
            List<ThanhVien_CLB> tvclb = db.ThanhVien_CLB.ToList();
            var dstv = from e in tv
                       join d in tvclb on e.ID equals d.IDtvien
                       where d.IDCLB == id
                       select new ThanhViensViewModel
                       {
                           ID=e.ID,
                           Ten = e.Ten,
                           Ho = e.Ho,
                           NgaySinh = e.NgaySinh,
                           MSSV = e.MSSV,
                           Lop = e.Lop,
                           SDT = e.SDT,
                           Mail = e.Mail,
                           HinhDaiDien = e.HinhDaiDien,
                           Khoa = e.Khoa.TenKhoa,
                           TenTK = e.DBUser.Username
                       };
            ViewBag.IdCLB = id;
            return View(dstv); 
        }
            // GET: Admin/ThanhViens/Details/5
         public ActionResult Details(int? id,int? id2)
        {
         if (id == null)
         {
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var e = db.ThanhVien.SingleOrDefault(n => n.ID == id);
         if (e == null)
         {
             return HttpNotFound();
         }
         var viewModel = new ThanhViensViewModel
         {
             ID = e.ID,
             Ten = e.Ten,
             Ho = e.Ho,
             NgaySinh = e.NgaySinh,
             MSSV = e.MSSV,
             Lop = e.Lop,
             SDT = e.SDT,
             Mail = e.Mail,
             HinhDaiDien = e.HinhDaiDien,
             Khoa = e.Khoa.TenKhoa,
             TenTK = e.DBUser.Username
         };
            ViewBag.IdCLB = id2;
         return View(viewModel);
        }

        public ActionResult DeleteConfirmed(ThanhViensViewModel tvvm)
        {
            var thanhVien = db.ThanhVien_CLB.SingleOrDefault(n => n.IDtvien == tvvm.ID);
            db.ThanhVien_CLB.Remove(thanhVien);
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
