using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.Sukien;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class SuKiensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SuKiens
        public ActionResult Index()
        {

            List<SuKien> hd = db.SuKien.ToList();
            var ds = from e in hd
                     select new SuKiensViewModel
                     {
                         ID = e.ID,
                         TieuDeSK = e.TieuDeSK,
                         NgayBatDau = e.NgayBatDau,
                         NgayKetThuc = e.NgayKetThuc,
                         MoTa = e.MoTa,
                         TenFile = e.TenFile,
                         ContentType = e.ContentType,
                         File = e.File,
                         NoiDung = e.NoiDung,
                         DiaDiem = e.DiaDiem,
                         CLB = e.CLB.TenCLB,
                         LoaiSK = e.LoaiSuKien.TenLoaiSK
                     };

            return View(ds.ToList().OrderByDescending(x => x.ID));
        }

        // GET: Admin/SuKiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.SuKien.SingleOrDefault(n => n.ID == id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new SuKiensViewModel
            {
                ID = e.ID,
                TieuDeSK = e.TieuDeSK,
                NgayBatDau = e.NgayBatDau,
                NgayKetThuc = e.NgayKetThuc,
                MoTa = e.MoTa,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung,
                DiaDiem = e.DiaDiem,
                CLB = e.CLB.TenCLB,
                LoaiSK = e.LoaiSuKien.TenLoaiSK
            };
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SuKiensViewModel suKien, HttpPostedFileBase upload)
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
                    SuKien suKiens = new SuKien();
                    suKiens.File = Myfile;
                    suKiens.TenFile = fileName;
                    suKiens.ContentType = contentType;
                    suKiens.IdCLB = suKien.IdCLB;
                    suKiens.TieuDeSK = suKien.TieuDeSK;
                    suKiens.MoTa = suKien.MoTa;
                    suKiens.NgayBatDau = suKien.NgayBatDau;
                    suKiens.NgayKetThuc = suKien.NgayKetThuc;
                    suKiens.NoiDung = suKien.NoiDung;
                    suKiens.DiaDiem = suKien.DiaDiem;
                    suKiens.IdLoaiSK = suKien.IdLoaiSK;
                    db.SuKien.Add(suKiens);
                    db.SaveChanges();

                }
                else
                {
                    SuKien suKiens = new SuKien();
                    suKiens.IdCLB = suKien.IdCLB;
                    suKiens.TieuDeSK = suKien.TieuDeSK;
                    suKiens.MoTa = suKien.MoTa;
                    suKiens.NgayBatDau = suKien.NgayBatDau;
                    suKiens.NgayKetThuc = suKien.NgayKetThuc;
                    suKiens.NoiDung = suKien.NoiDung;
                    suKiens.DiaDiem = suKien.DiaDiem;
                    suKiens.IdLoaiSK = suKien.IdLoaiSK;
                    db.SuKien.Add(suKiens);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", suKien.IdCLB);
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK", suKien.LoaiSK);
            return View(suKien);
        }
            [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuKien e = db.SuKien.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new SuKiensViewModel
            {
                ID = e.ID,
                TieuDeSK = e.TieuDeSK,
                NgayBatDau = e.NgayBatDau,
                NgayKetThuc = e.NgayKetThuc,
                MoTa = e.MoTa,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung,
                DiaDiem = e.DiaDiem,
                CLB = e.CLB.TenCLB,
                LoaiSK = e.LoaiSuKien.TenLoaiSK,
                IdCLB = e.IdCLB,
                IdLoaiSK = e.IdLoaiSK
            };
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SuKiensViewModel suKien, HttpPostedFileBase upload)
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
                    SuKien suKiens = db.SuKien.Find(suKien.ID);
                    suKiens.File = Myfile;
                    suKiens.TenFile = fileName;
                    suKiens.ContentType = contentType;
                    suKiens.IdCLB = suKien.IdCLB;
                    suKiens.TieuDeSK = suKien.TieuDeSK;
                    suKiens.MoTa = suKien.MoTa;
                    suKiens.NgayBatDau = suKien.NgayBatDau;
                    suKiens.NgayKetThuc = suKien.NgayKetThuc;
                    suKiens.NoiDung = suKien.NoiDung;
                    suKiens.DiaDiem = suKien.DiaDiem;
                    suKiens.IdLoaiSK = suKien.IdLoaiSK;
                    db.SaveChanges();

                }
                else
                {
                    SuKien suKiens = db.SuKien.Find(suKien.ID);
                        suKiens.IdCLB = suKien.IdCLB;
                        suKiens.TieuDeSK = suKien.TieuDeSK;
                        suKiens.MoTa = suKien.MoTa;
                        suKiens.NgayBatDau = suKien.NgayBatDau;
                        suKiens.NgayKetThuc = suKien.NgayKetThuc;
                        suKiens.NoiDung = suKien.NoiDung;
                        suKiens.DiaDiem = suKien.DiaDiem;
                        suKiens.IdLoaiSK = suKien.IdLoaiSK;
                        db.SaveChanges(); 
                }
                return RedirectToAction("Details", new { id = suKien.ID });
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB",suKien.IdCLB);
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK", suKien.LoaiSK);
            return View(suKien);
        }
        public ActionResult DeleteConfirmed(SuKiensViewModel skvm)
        {
            SuKien suKien = db.SuKien.Find(skvm.ID);
            db.SuKien.Remove(suKien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var suKien = db.SuKien.Where(u => u.ID == id).FirstOrDefault();
            return File(suKien.File, suKien.ContentType, suKien.TenFile);
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
