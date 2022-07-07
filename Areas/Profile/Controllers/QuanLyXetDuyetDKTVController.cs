using ClubPortalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyXetDuyetDKTVController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLXetDuyetTV_CLB()
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
                                   Mota = i.Mota,
                                   HinhCLB = i.HinhCLB,
                                   NgayThanhLap = i.NgayThanhLap
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }
        public ActionResult QLXetDuyetTV(int? id, int? page)
        {

            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<DangKy> dangKies = db.DangKy.ToList();
            var DsTvDangKy = from e in dangKies
                               where  e.IDCLB == id
                               select new ViewModel1
                               {
                                 DangKy=e
                               };
            ViewBag.DsTvDangKy = DsTvDangKy;
            return View();
        }
        public ActionResult ThemTV(int? id)
        {
            DangKy dangKy = db.DangKy.Find(id);
            ThanhVien_CLB thanhVien_CLB = new ThanhVien_CLB();
            thanhVien_CLB.IDCLB = dangKy.IDCLB;
            thanhVien_CLB.IDtvien = dangKy.IdTv;
            thanhVien_CLB.IDRoles = 1;
            var userroles = from e in db.DBUserRoles
                           where e.UserID == dangKy.IdTv
                           select new
                           {
                               roleid = e.RoleID,
                               userid = e.UserID
                           };
            if (userroles == null || userroles.Count() == 0)
            {
                DBUserRoles userrole = new DBUserRoles();
                userrole.RoleID = 1;
                userrole.UserID = dangKy.IdTv;
                db.DBUserRoles.Add(userrole);
            }
            else
            {
                Boolean hasRole1 = false;
                Boolean hasRole2 = false;
                foreach (var role in userroles)
                {
                    if (role.roleid == 1)
                    {
                        hasRole1 = true;
                    }
                    if (role.roleid == 2)
                    {
                        hasRole2 = true;
                    }
                }
                foreach (var role in userroles)
                {
                    if (!hasRole1 && hasRole2)
                    {
                        DBUserRoles userrole = new DBUserRoles();
                        userrole.RoleID = 1;
                        userrole.UserID = dangKy.IdTv;
                        db.DBUserRoles.Add(userrole);
                    }
                }
            }
           

            db.ThanhVien_CLB.Add(thanhVien_CLB);
            db.DangKy.Remove(dangKy);
            db.SaveChanges();
            return RedirectToAction("QLXetDuyetTV", new {id});
        }
        public ActionResult TuChoiTV(int? id)
        {
            DangKy dangKy = db.DangKy.Find(id);
            db.DangKy.Remove(dangKy);
            db.SaveChanges();
            return RedirectToAction("QLXetDuyetTV", new {id});
        }
    }
}