using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.ThongBao;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class ThongBaosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ThongBaos
        public ActionResult Index()
        {
            List<ThongBao> tb = db.ThongBao.ToList();
            var dsTB = from e in tb
                          select new ThongBaosViewModels
                          {
                              ID = e.ID,
                              TieuDe = e.TieuDe,
                              CLB = e.CLB.TenCLB,
                              NgayThongBao = e.NgayThongBao,
                              MoTa = e.MoTa,
                              TenFile=e.TenFile,
                              ContentType=e.ContentType,
                              File=e.File,
                              NoiDung=e.NoiDung
                          };

            return View(dsTB);
        }

        // GET: Admin/ThongBaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.ThongBao.SingleOrDefault(n => n.ID == id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ThongBaosViewModels
            {
                ID = e.ID,
                TieuDe = e.TieuDe,
                CLB = e.CLB.TenCLB,
                NgayThongBao = e.NgayThongBao,
                MoTa = e.MoTa,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung

            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");    
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThongBaosViewModels thongBao, HttpPostedFileBase upload)
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
                    ThongBao thongBaos = new ThongBao();
                    thongBaos.File = Myfile;
                    thongBaos.TenFile = fileName;
                    thongBaos.ContentType = contentType;
                    thongBaos.IdCLB = thongBao.IdCLB;
                    thongBaos.TieuDe = thongBao.TieuDe;
                    thongBaos.MoTa = thongBao.MoTa;
                    thongBaos.NgayThongBao = thongBao.NgayThongBao;
                    thongBaos.NoiDung = thongBao.NoiDung;
                    db.ThongBao.Add(thongBaos);
                    db.SaveChanges();

                }
                else
                {
                    ThongBao thongBaos = new ThongBao();
                    thongBaos.IdCLB = thongBao.IdCLB;
                    thongBaos.TieuDe = thongBao.TieuDe;
                    thongBaos.MoTa = thongBao.MoTa;
                    thongBaos.NgayThongBao = thongBao.NgayThongBao;
                    thongBaos.NoiDung = thongBao.NoiDung;
                    db.ThongBao.Add(thongBaos);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", thongBao.IdCLB);
            return View(thongBao);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao e = db.ThongBao.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ThongBaosViewModels
            {
                ID = e.ID,
                TieuDe = e.TieuDe,
                NgayThongBao = e.NgayThongBao,         
                MoTa = e.MoTa,
                TenFile = e.TenFile,
                ContentType = e.ContentType,
                File = e.File,
                NoiDung = e.NoiDung,
                CLB = e.CLB.TenCLB,
                IdCLB = e.IdCLB,
            };
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThongBaosViewModels thongBao, HttpPostedFileBase upload)
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
                    ThongBao thongBaos = db.ThongBao.Find(thongBao.ID);
                    thongBaos.File = Myfile;
                    thongBaos.TenFile = fileName;
                    thongBaos.ContentType = contentType;
                    thongBaos.IdCLB = thongBao.IdCLB;
                    thongBaos.TieuDe = thongBao.TieuDe;
                    thongBaos.MoTa = thongBao.MoTa;
                    thongBaos.NgayThongBao = thongBao.NgayThongBao;
                    thongBaos.NoiDung = thongBao.NoiDung;
                    db.SaveChanges();

                }
                else
                {
                    ThongBao thongBaos = db.ThongBao.Find(thongBao.ID);
                    thongBaos.IdCLB = thongBao.IdCLB;
                    thongBaos.TieuDe = thongBao.TieuDe;
                    thongBaos.MoTa = thongBao.MoTa;
                    thongBaos.NgayThongBao = thongBao.NgayThongBao;
                    thongBaos.NoiDung = thongBao.NoiDung;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = thongBao.ID });
            }
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", thongBao.IdCLB);
            return View(thongBao);
        }

        public ActionResult DeleteConfirmed(ThongBaosViewModels tbvm)
        {
            ThongBao thongBao = db.ThongBao.Find(tbvm.ID);
            db.ThongBao.Remove(thongBao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public FileResult DocumentDownloadFileDinhKem(int? id)
        {
            var tb = db.ThongBao.Where(u => u.ID == id).FirstOrDefault();
            return File(tb.File, tb.ContentType, tb.TenFile);
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
