using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyPhanHoiController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLPhanHoi_CLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb                                
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 2
                               select new CLBDaThamGiaViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IDCLB = i.ID,
                                   HinhCLB = i.HinhCLB,
                                   Mota = i.Mota,
                                   NgayThanhLap = i.NgayThanhLap
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }

        // GET: Profile/QuanLyPhanHoi
        public ActionResult QLPhanHoi(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<PhanHoi> phanHois = db.PhanHoi.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsPhanHoi = from e in thanhVien_clb
                             join d in phanHois on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDCLB == id && e.IDRoles == 1 && e.IDtvien == d.Idtv
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 //CLB = i,
                                 PhanHoi = d
                             };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsPhanHoi = DsPhanHoi;
            return View();
        }

        // GET: Profile/QuanLyPhanHoi/Details/5
        public ActionResult XemPH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanHoi phanHoi = db.PhanHoi.Find(id);
            if (phanHoi == null)
            {
                return HttpNotFound();
            }
            return View(phanHoi);
        }
      
     
      
        public ActionResult XacNhanXoaPH(int id)
        {
            PhanHoi phanHoi = db.PhanHoi.Find(id);
            db.PhanHoi.Remove(phanHoi);
            db.SaveChanges();
            return RedirectToAction("QLPhanHoi", new { id= phanHoi.IdCLB});
        }

    }
}
