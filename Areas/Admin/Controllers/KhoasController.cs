using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.Khoa;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class KhoasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Khoas
        public ActionResult Index()
        {
            List<Khoa> albums = db.Khoa.ToList();
            var dsKhoa = from e in albums
                          select new KhoaViewModel
                          {
                              ID = e.ID,
                              TenKhoa = e.TenKhoa
                          };

            return View(dsKhoa);
        }

        // GET: Admin/Khoas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Khoa.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new KhoaViewModel
            {
                ID = data.ID,
                TenKhoa = data.TenKhoa

            };
            return View(viewModel);
        }

        // GET: Admin/Khoas/Create
        public ActionResult Create()
        {
            var createKhoa = new KhoaViewModel();
            return View(createKhoa);
        }

        // POST: Admin/Khoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KhoaViewModel khoaView)
        {
            if (ModelState.IsValid)
            {
                Khoa khoa = new Khoa();
                khoa.ID = khoaView.ID;
                khoa.TenKhoa = khoaView.TenKhoa;
                db.Khoa.Add(khoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khoaView);
        }

        // GET: Admin/Khoas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Khoa.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new KhoaViewModel
            {
                ID = data.ID,
                TenKhoa = data.TenKhoa

            };
            return View(viewModel);
        }

        // POST: Admin/Khoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( KhoaViewModel khoaView,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var khoa = db.Khoa.SingleOrDefault(n => n.ID == id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                khoa.ID = khoaView.ID;
                khoa.TenKhoa = khoaView.TenKhoa;
                db.Entry(khoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khoaView);
        }

        public ActionResult DeleteConfirmed(int id)
        {

            Khoa khoa = db.Khoa.Find(id);
            db.Khoa.Remove(khoa);
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
