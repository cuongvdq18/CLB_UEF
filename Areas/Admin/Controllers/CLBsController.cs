using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class CLBsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> CLB = db.CLB.ToList();
            List<LoaiCLB> loaiCLB = db.LoaiCLB.ToList();
            var clb = from e in CLB
                      join d in loaiCLB on e.IdLoaiCLB equals d.IDLoaiCLB
                      select new ViewModel.CLB.CLBViewModels
                      {
                              ID = e.ID,
                              LoaiCLB = d.TenLoaiCLB,
                              TenCLB = e.TenCLB,
                              LienHe = e.LienHe,
                              Mota = e.Mota,
                              Email=e.Email,
                              HinhCLB =e.HinhCLB,
                              FanPage=e.FanPage,
                              NgayThanhLap=e.NgayThanhLap
                      };


            return View(clb.ToList().OrderBy(x => x.ID));
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.CLB.SingleOrDefault(n => n.ID == id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ViewModel.CLB.CLBViewModels
            {
                ID = e.ID,
                LoaiCLB = e.LoaiCLB.TenLoaiCLB,
                TenCLB = e.TenCLB,
                LienHe = e.LienHe,
                Mota = e.Mota,
                Email = e.Email,
                FanPage = e.FanPage,
                NgayThanhLap = e.NgayThanhLap,
                HinhCLB=e.HinhCLB

            };
            return View(viewModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.CLB.SingleOrDefault(n => n.ID == id);
            if (e == null)
            {
                return HttpNotFound();
            }
            var viewModel = new Models.CLBViewModels
            {
                ID = e.ID,
                IdLoaiCLB = e.IdLoaiCLB,
                TenCLB = e.TenCLB,
                LienHe = e.LienHe,
                Mota = e.Mota,
                Email = e.Email,
                FanPage = e.FanPage,
                NgayThanhLap = e.NgayThanhLap,
                HinhCLB = e.HinhCLB,
                ImageFile = e.ImageFile
            };
            ViewBag.IdLoaiCLB = new SelectList(db.LoaiCLB, "IDLoaiCLB", "TenLoaiCLB");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.CLBViewModels cLBViewModel)
        {
            int UserID = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
                if (cLBViewModel.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(cLBViewModel.ImageFile.FileName);
                    string extension = Path.GetExtension(cLBViewModel.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    cLBViewModel.HinhCLB = "/Hinh/HinhCLB/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Hinh/HinhCLB/"), fileName);
                    cLBViewModel.ImageFile.SaveAs(fileName);

                    CLB cLB = db.CLB.Find(cLBViewModel.ID);
                    cLB.Email = cLBViewModel.Email;
                    cLB.FanPage = cLBViewModel.FanPage;
                    cLB.IdLoaiCLB = cLBViewModel.IdLoaiCLB;
                    cLB.Mota = cLBViewModel.Mota;
                    cLB.TenCLB = cLBViewModel.TenCLB;
                    cLB.LienHe = cLBViewModel.LienHe;
                    cLB.NgayThanhLap = cLBViewModel.NgayThanhLap;
                    cLB.HinhCLB = cLBViewModel.HinhCLB;
                    cLB.ImageFile = cLBViewModel.ImageFile;
                    db.SaveChanges();
                    ViewBag.IdLoaiCLB = new SelectList(db.LoaiCLB, "IDLoaiCLB", "TenLoaiCLB", cLB.IdLoaiCLB);
                }
                else
                {
                    CLB cLB = db.CLB.Find(cLBViewModel.ID);
                    cLB.Email = cLBViewModel.Email;
                    cLB.IdLoaiCLB = cLBViewModel.IdLoaiCLB;
                    cLB.Mota = cLBViewModel.Mota;
                    cLB.TenCLB = cLBViewModel.TenCLB;
                    cLB.LienHe = cLBViewModel.LienHe;
                    cLB.FanPage = cLBViewModel.FanPage;
                    cLB.NgayThanhLap = cLBViewModel.NgayThanhLap;
                    db.SaveChanges();
                    ViewBag.IdLoaiCLB = new SelectList(db.LoaiCLB, "IDLoaiCLB", "TenLoaiCLB", cLB.IdLoaiCLB);
                }

            }
            return RedirectToAction("Details", new { id = cLBViewModel.ID });
        }
        public ActionResult DeleteConfirmed(ViewModel.CLB.CLBViewModels cLBViewModels)
        {
            CLB data = db.CLB.Find(cLBViewModels.ID);
            var tv_clb = db.ThanhVien_CLB.Where(u => u.IDCLB.ToString().Equals(cLBViewModels.ID.ToString())).FirstOrDefault();
            db.ThanhVien_CLB.Remove(tv_clb);
            db.CLB.Remove(data);
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
