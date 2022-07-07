using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.HoatDong;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class QLDSHoatDongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {

            List<QLDSHoatDong> hd = db.QLDSHoatDong.ToList();
            var ds = from e in hd
                     select new QLDSHoatDongViewModel
                       {
                           ID = e.ID,
                           ChuDe = e.ChuDe,
                           NgayBatDau = e.NgayBatDau,
                           NgayKetThuc = e.NgayKetThuc,
                           Mota = e.Mota,
                           TenFile = e.TenFile,
                           ContentType = e.ContentType,
                           File = e.File,
                           NoiDung = e.NoiDung,
                           DiaDiem = e.DiaDiem,
                           TenCLB = e.CLB.TenCLB,
                           LoaiHD = e.LoaiHD.TenLoaiHD
                     };

            return View(ds.ToList().OrderByDescending(x => x.ID));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.QLDSHoatDong.SingleOrDefault(n => n.ID == id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new QLDSHoatDongViewModel
            {
                ID = e.ID,
                ChuDe = e.ChuDe,
                NgayBatDau = e.NgayBatDau,
                NgayKetThuc = e.NgayKetThuc,
                Mota = e.Mota,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung,
                DiaDiem = e.DiaDiem,
                TenCLB = e.CLB.TenCLB,
                LoaiHD = e.LoaiHD.TenLoaiHD
            };
            return View(viewModel);           
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QLDSHoatDongViewModel hoatDong, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    int filelength = upload.ContentLength;
                    string fileName = upload.FileName;
                    string contentType = upload.ContentType;
                    byte[] Myfile = new byte[filelength];
                    upload.InputStream.Read(Myfile, 0, filelength);
                    QLDSHoatDong hoatDongs = new QLDSHoatDong();
                    hoatDongs.File = Myfile;
                    hoatDongs.TenFile = fileName;
                    hoatDongs.ContentType = contentType;
                    hoatDongs.IdCLB = hoatDong.IdCLB;
                    hoatDongs.ChuDe = hoatDong.ChuDe;
                    hoatDongs.Mota = hoatDong.Mota;
                    hoatDongs.NgayBatDau = hoatDong.NgayBatDau;
                    hoatDongs.NgayKetThuc = hoatDong.NgayKetThuc;
                    hoatDongs.NoiDung = hoatDong.NoiDung;
                    hoatDongs.DiaDiem = hoatDong.DiaDiem;
                    hoatDongs.IdLoaiHD = hoatDong.IdLoaiHD;
                    db.QLDSHoatDong.Add(hoatDongs);
                    db.SaveChanges();

                }
                else
                {
                    QLDSHoatDong hoatDongs = new QLDSHoatDong();
                    hoatDongs.IdCLB = hoatDong.IdCLB;
                    hoatDongs.ChuDe = hoatDong.ChuDe;
                    hoatDongs.Mota = hoatDong.Mota;
                    hoatDongs.NgayBatDau = hoatDong.NgayBatDau;
                    hoatDongs.NgayKetThuc = hoatDong.NgayKetThuc;
                    hoatDongs.NoiDung = hoatDong.NoiDung;
                    hoatDongs.DiaDiem = hoatDong.DiaDiem;
                    hoatDongs.IdLoaiHD = hoatDong.IdLoaiHD;
                    db.QLDSHoatDong.Add(hoatDongs);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD");
            return View(hoatDong);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QLDSHoatDong e = db.QLDSHoatDong.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new QLDSHoatDongViewModel
            {
                ID = e.ID,
                ChuDe = e.ChuDe,
                NgayBatDau = e.NgayBatDau,
                NgayKetThuc = e.NgayKetThuc,
                Mota = e.Mota,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung,
                DiaDiem = e.DiaDiem,
                TenCLB = e.CLB.TenCLB,
                LoaiHD = e.LoaiHD.TenLoaiHD,
                IdCLB = e.IdCLB,
                IdLoaiHD = e.IdLoaiHD
            };
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QLDSHoatDongViewModel hoatDong, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    int filelength = upload.ContentLength;
                    string fileName = upload.FileName;
                    string contentType = upload.ContentType;
                    byte[] Myfile = new byte[filelength];
                    upload.InputStream.Read(Myfile, 0, filelength);
                    QLDSHoatDong hoatDongs = db.QLDSHoatDong.Find(hoatDong.ID);
                    hoatDongs.File = Myfile;
                    hoatDongs.TenFile = fileName;
                    hoatDongs.ContentType = contentType;
                    hoatDongs.IdCLB = hoatDong.IdCLB;
                    hoatDongs.ChuDe = hoatDong.ChuDe;
                    hoatDongs.Mota = hoatDong.Mota;
                    hoatDongs.NgayBatDau = hoatDong.NgayBatDau;
                    hoatDongs.NgayKetThuc = hoatDong.NgayKetThuc;
                    hoatDongs.NoiDung = hoatDong.NoiDung;
                    hoatDongs.DiaDiem = hoatDong.DiaDiem;
                    hoatDongs.IdLoaiHD = hoatDong.IdLoaiHD;
                    db.SaveChanges();

                }
                else
                {
                    QLDSHoatDong hoatDongs = db.QLDSHoatDong.Find(hoatDong.ID);
                    hoatDongs.IdCLB = hoatDong.IdCLB;
                    hoatDongs.ChuDe = hoatDong.ChuDe;
                    hoatDongs.Mota = hoatDong.Mota;
                    hoatDongs.NgayBatDau = hoatDong.NgayBatDau;
                    hoatDongs.NgayKetThuc = hoatDong.NgayKetThuc;
                    hoatDongs.NoiDung = hoatDong.NoiDung;
                    hoatDongs.DiaDiem = hoatDong.DiaDiem;
                    hoatDongs.IdLoaiHD = hoatDong.IdLoaiHD;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = hoatDong.ID });
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", hoatDong.IdCLB);
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD", hoatDong.IdLoaiHD);
            return View(hoatDong);
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var hoatDong = db.QLDSHoatDong.Where(u => u.ID == id).FirstOrDefault();
            return File(hoatDong.File, hoatDong.ContentType, hoatDong.TenFile);
        }
        public ActionResult DeleteConfirmed(QLDSHoatDongViewModel hdvm)
        {
            QLDSHoatDong qLDSHoatDong = db.QLDSHoatDong.Find(hdvm.ID);
            db.QLDSHoatDong.Remove(qLDSHoatDong);
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
