using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.CLB;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class LoaiCLBsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/LoaiCLBs
        public ActionResult Index()
        {
            List<LoaiCLB> loaiCLB = db.LoaiCLB.ToList();
            var ds = from e in loaiCLB
                          select new LoaiCLBViewModel
                          {
                              IDLoaiCLB = e.IDLoaiCLB,
                              TenLoaiCLB = e.TenLoaiCLB
                          };

            return View(ds);
        }

        // GET: Admin/LoaiCLBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.LoaiCLB.SingleOrDefault(n => n.IDLoaiCLB == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new LoaiCLBViewModel
            {
                IDLoaiCLB = data.IDLoaiCLB,
                TenLoaiCLB = data.TenLoaiCLB

            };
            return View(viewModel);
        }

        // GET: Admin/LoaiCLBs/Create
        public ActionResult Create()
        {
            var createLoaiCLB = new LoaiCLBViewModel();
            return View(createLoaiCLB);
        }

        // POST: Admin/LoaiCLBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLoaiCLB,TenLoaiCLB")] LoaiCLBViewModel loaiCLBView)
        {
            if (ModelState.IsValid)
            {
                LoaiCLB loaiCLB = new LoaiCLB();
                loaiCLB.IDLoaiCLB = loaiCLBView.IDLoaiCLB;
                loaiCLB.TenLoaiCLB = loaiCLBView.TenLoaiCLB;

                db.LoaiCLB.Add(loaiCLB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiCLBView);
        }

        // GET: Admin/LoaiCLBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.LoaiCLB.SingleOrDefault(n => n.IDLoaiCLB == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new LoaiCLBViewModel
            {
                IDLoaiCLB = data.IDLoaiCLB,
                TenLoaiCLB = data.TenLoaiCLB

            };
            return View(viewModel);
        }

        // POST: Admin/LoaiCLBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LoaiCLBViewModel loaiCLBView, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.LoaiCLB.SingleOrDefault(n => n.IDLoaiCLB == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                data.IDLoaiCLB = loaiCLBView.IDLoaiCLB;
                data.TenLoaiCLB = loaiCLBView.TenLoaiCLB;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiCLBView);
        }

        public ActionResult DeleteConfirmed(LoaiCLBViewModel loaiCLBView,int id)
        {
            LoaiCLB loaiCLB = db.LoaiCLB.Find(id);
            db.LoaiCLB.Remove(loaiCLB);
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
