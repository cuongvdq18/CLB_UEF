using ClubPortalMS.Models;
using CustomAuthorizationFilter.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyLichTapController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLLichTap_CLB()
        { 
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            List<NhiemVu_ThanhVien> nhiemVus = db.NhiemVu_ThanhVien.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 2
                               select new CLBShowViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID,
                                   HinhCLB = i.HinhCLB
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }
        public ActionResult QLLichTap(int? id, int? page)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<LichTap> lichTaps = db.LichTap.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsLichTap = from e in thanhVien_clb
                             join d in lichTaps on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDCLB == id && e.IDRoles == 2
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 //CLB = i,
                                 LichTap = d
                             };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsLichTap = DsLichTap.OrderByDescending(x => x.LichTap.ID).ToList();
            return View();
        }
        public ActionResult XemLT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichTap lichTap = db.LichTap.Find(id);
            if (lichTap == null)
            {
                return HttpNotFound();
            }
            return View(lichTap);
        }
        [HttpGet]
        public ActionResult TaoLT()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 2
                               select new PhanHoiCLBViewModel
                               {
                                   IdTvien = e.IDtvien,
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID
                               };
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoLT([Bind(Include = "ID,TieuDe,NgayBatDau,NgayKetThuc,DiaDiem,IdCLB")] LichTap lichTap)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            if (ModelState.IsValid)
            {
                    db.LichTap.Add(lichTap);
                    db.SaveChanges();
                    var thanhvien_CLB = from e in thanhVien_clb
                                        where e.IDCLB == lichTap.IdCLB
                                        && e.IDRoles == 1
                                        select new ViewModel1
                                        {
                                            ThanhVien_CLB = e,
                                        };
                    foreach (ViewModel1 dsTV in thanhvien_CLB)
                    {
                        LichTap_ThanhVien lichTap_ThanhVien = new LichTap_ThanhVien();
                        lichTap_ThanhVien.IdLT = lichTap.ID;
                        lichTap_ThanhVien.IDTvien = dsTV.ThanhVien_CLB.IDtvien;
                        db.LichTap_ThanhVien.Add(lichTap_ThanhVien);
                        db.SaveChanges();
                    }
                    return RedirectToAction("QLLichTap_CLB");
            
            }
            List<CLB> clb = db.CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 2
                               select new PhanHoiCLBViewModel
                               {
                                   IdTvien = e.IDtvien,
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID
                               };
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View(lichTap);
        }
        public ActionResult SuaLT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichTap lichTap = db.LichTap.Find(id);
            if (lichTap == null)
            {
                return HttpNotFound();
            }
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 2
                               select new PhanHoiCLBViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID
                               };
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View(lichTap);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaLT([Bind(Include = "ID,TieuDe,NgayBatDau,NgayKetThuc,DiaDiem,IdCLB")] LichTap lichTap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichTap).State = EntityState.Modified;
                db.SaveChanges();
                List<LichTap_ThanhVien> lichTap_ThanhViens = db.LichTap_ThanhVien.ToList();
                var lichtrinh_thanhVien = from e in lichTap_ThanhViens
                                    where e.IdLT == lichTap.ID
                                    select new ViewModel1
                                    {
                                        LichTap_ThanhVien = e
                                    };
                foreach (ViewModel1 dsTV in lichtrinh_thanhVien)
                {
                    LichTap_ThanhVien lichTap_ThanhVien =  db.LichTap_ThanhVien.Find(dsTV.LichTap_ThanhVien.ID);
                    db.LichTap_ThanhVien.Remove(lichTap_ThanhVien);
                    db.SaveChanges();
                }

                List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
                var thanhvien_CLB = from e in thanhVien_clb
                                    where e.IDCLB == lichTap.IdCLB
                                    && e.IDRoles == 1
                                    select new ViewModel1
                                    {
                                        ThanhVien_CLB = e,
                                    };
                foreach (ViewModel1 dsTV in thanhvien_CLB)
                {
                    LichTap_ThanhVien lichTap_ThanhVien = new LichTap_ThanhVien();
                    lichTap_ThanhVien.IdLT = lichTap.ID;
                    lichTap_ThanhVien.IDTvien = dsTV.ThanhVien_CLB.IDtvien;
                    db.LichTap_ThanhVien.Add(lichTap_ThanhVien);
                    db.SaveChanges();
                }
                return RedirectToAction("QLLichTap", new { id = lichTap.IdCLB });
            }
            return View(lichTap);
        }

     

        public ActionResult XacNhanXoaLT(int id)
        {
            LichTap lichTap = db.LichTap.Find(id);
            List<LichTap_ThanhVien> lichTap_ThanhViens = db.LichTap_ThanhVien.ToList();
            var lichtrinh_thanhVien = from e in lichTap_ThanhViens
                                      where e.IdLT == lichTap.ID
                                      select new ViewModel1
                                      {
                                          LichTap_ThanhVien = e
                                      };
            foreach (ViewModel1 dsTV in lichtrinh_thanhVien)
            {
                LichTap_ThanhVien lichTap_ThanhVien = db.LichTap_ThanhVien.Find(dsTV.LichTap_ThanhVien.ID);
                db.LichTap_ThanhVien.Remove(lichTap_ThanhVien);
                db.SaveChanges();
            }
            db.LichTap.Remove(lichTap);
            db.SaveChanges();
            return RedirectToAction("QLLichTap",new { id=lichTap.IdCLB});
        }

    }
}