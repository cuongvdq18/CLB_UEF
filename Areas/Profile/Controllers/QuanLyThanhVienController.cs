using ClubPortalMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyThanhVienController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        #region QUản lý thành viên CLB cho role hlv
        public ActionResult CLBQL()
            {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals d.ID into table1
                                   //from d in table1.ToList()
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

        public ActionResult QuanLyThanhVien( int? id)
        {
     
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThanhVien> thanhVien = db.ThanhVien.ToList();
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsThanhVien = from e in thanhVien_clb
                               join d in thanhVien on e.IDtvien equals d.ID into table1
                               from d in table1.ToList()
                               //join i in clb on e.IDCLB equals i.ID into table
                               //from i in table.ToList()
                               where  e.IDRoles == 1 
                               && e.IDCLB == id
                               select new ViewModel1
                               {
                                   ThanhVien_CLB = e,
                                   //CLB = i,
                                   ThanhVien = d
                               };
            ViewBag.DsThanhVien = DsThanhVien;
            return View();
        }
        public ActionResult XemChiTietTV(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien thanhVien = db.ThanhVien.Find(id);
            if (thanhVien == null)
            {
                return HttpNotFound();
            }
            return View(thanhVien);
        }

        public ActionResult LoaiTruThanhVien(int id) {
            ThanhVien_CLB thanhVien = db.ThanhVien_CLB.Find(id);
            db.ThanhVien_CLB.Remove(thanhVien);
            db.SaveChanges();
            return RedirectToAction("QuanLyThanhVien",new { id=thanhVien.IDCLB});
        }
        #endregion

    }
}