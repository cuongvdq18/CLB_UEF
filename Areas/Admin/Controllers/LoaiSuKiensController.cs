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
    public class LoaiSuKiensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/LoaiSuKiens
        public ActionResult Index()
        {
            List<LoaiSuKien> loaisk = db.LoaiSuKien.ToList();
            var dsSK = from e in loaisk
                       select new LoaiSuKienViewModel
                          {
                              ID = e.ID,
                              TenLoaiSK = e.TenLoaiSK,
                              TrangThai = e.TrangThai
                          };

            return View(dsSK);
        }

        // GET: Admin/LoaiSuKiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.LoaiSuKien.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new LoaiSuKienViewModel
            {
                ID = data.ID,
                TenLoaiSK = data.TenLoaiSK,
                TrangThai = data.TrangThai

            };
            return View(viewModel);
        }

        // GET: Admin/LoaiSuKiens/Create
        public ActionResult Create()
        {
            var createLoaiSK = new LoaiSuKienViewModel();
            return View(createLoaiSK);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoaiSuKienViewModel loaiSuKienView)
        {
            if (ModelState.IsValid)
            {
                LoaiSuKien loaiSuKien = new LoaiSuKien();
                loaiSuKien.ID = loaiSuKienView.ID;
                loaiSuKien.TenLoaiSK = loaiSuKienView.TenLoaiSK;
                loaiSuKien.TrangThai = loaiSuKienView.TrangThai;
                db.LoaiSuKien.Add(loaiSuKien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiSuKienView);
        }

        // GET: Admin/LoaiSuKiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.LoaiSuKien.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new LoaiSuKienViewModel
            {
                ID = data.ID,
                TenLoaiSK = data.TenLoaiSK,
                TrangThai = data.TrangThai

            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LoaiSuKienViewModel loaiSuKienView,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var loaiSuKien = db.LoaiSuKien.SingleOrDefault(n => n.ID == id);
            if (loaiSuKien == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                loaiSuKien.ID = loaiSuKienView.ID;
                loaiSuKien.TenLoaiSK = loaiSuKienView.TenLoaiSK;
                loaiSuKien.TrangThai = loaiSuKienView.TrangThai;
                db.Entry(loaiSuKien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiSuKienView);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            LoaiSuKien loaiSuKien = db.LoaiSuKien.Find(id);
            db.LoaiSuKien.Remove(loaiSuKien);
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
