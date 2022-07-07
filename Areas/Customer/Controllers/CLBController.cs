using ClubPortalMS.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Customer.Controllers
{
    public class CLBController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult DsCLBtheoLoai(int? id,int? page)
        {
            List<CLB> clb = db.CLB.ToList();
            var dsclb = from e in clb
                        where e.IdLoaiCLB == id
                        select e;
            ViewBag.Dsclb = dsclb.OrderBy(x => x.ID).ToList().ToPagedList(page ?? 1, 6);
            ViewBag.IDloaiclb = id;
            return View();
        }
        public ActionResult ChiTietCLB(int? id)
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
                HinhCLB = e.HinhCLB

            };
            var slTV = from c in db.ThanhVien
                       join d in db.ThanhVien_CLB on c.ID equals d.IDtvien
                       where d.IDCLB == id
                       select c;
            var slHD = from c in db.QLDSHoatDong
                       where c.IdCLB == id
                       select c;
            var slSK = from d in db.SuKien
                       where d.IdCLB == id
                       select d;
            ViewBag.slTV = slTV.Count();
            ViewBag.slHD = slHD.Count();
            ViewBag.slSK = slSK.Count();
            return View(viewModel);
        }
    }
}