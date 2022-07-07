using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.PhanHoi;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class PhanHoisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/PhanHois
        public ActionResult Index()
        {
            List<PhanHoi> phanHoi = db.PhanHoi.ToList();
            List<ThanhVien> tv = db.ThanhVien.ToList();
            var dsPH = from e in phanHoi
                       join d in tv on e.Idtv equals d.ID
                       select new PhanHoisViewModel
                       {
                           ID = e.ID,
                           Ten = e.Ten,
                           DiaChi = e.DiaChi,
                           SDT = e.SDT,
                           Email = e.Email,
                           NoiDung = e.NoiDung,
                           TGphanhoi = e.TGphanhoi,
                           TenCLB = e.CLB.TenCLB
                       };

            return View(dsPH);
        }

        // GET: Admin/PhanHois/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.PhanHoi.SingleOrDefault(n => n.ID == id);
            var d = db.ThanhVien.SingleOrDefault(c => c.ID == e.Idtv);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PhanHoisViewModel
            {
                ID = e.ID,
                Ten = e.Ten,
                DiaChi = e.DiaChi,
                SDT = e.SDT,
                Email = e.Email,
                NoiDung = e.NoiDung,    
                TGphanhoi = e.TGphanhoi,
                TenCLB = e.CLB.TenCLB

            };
            return View(viewModel);
        }

        public ActionResult DeleteConfirmed(PhanHoisViewModel phvm)
        {
            PhanHoi phanHoi = db.PhanHoi.Find(phvm.ID);
            db.PhanHoi.Remove(phanHoi);
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
