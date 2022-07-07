using ClubPortalMS.Models;
using CustomAuthorizationFilter.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class QuanLyDkyCLBController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult QLDkyCLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<DkyCLB> dangKies = db.DkyCLB.ToList();
            List<LoaiCLB> loaiCLBs = db.LoaiCLB.ToList();
            var DsDangKyCLB = from e in dangKies
                              join i in loaiCLBs on e.IDLoaiCLB equals i.IDLoaiCLB into table
                              from i in table.ToList()
                              select new ViewModel1 
                              { 
                                  DkyCLB = e,
                                  LoaiCLB = i
                              };

            ViewBag.DsDangKyCLB = DsDangKyCLB;
            return View();
        }
        public ActionResult ThemCLB(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            DkyCLB dangKy = db.DkyCLB.Find(id);
            CLB cLB = new CLB();
            cLB.IdLoaiCLB = dangKy.IDLoaiCLB;
            cLB.TenCLB = dangKy.TenCLB;
            cLB.NgayThanhLap = DateTime.Now;
            cLB.HinhCLB = "/Hinh/HinhCLB/CLB_default.png";
            db.CLB.Add(cLB);
            ThanhVien_CLB thanhVien_CLB = new ThanhVien_CLB();
            thanhVien_CLB.IDCLB = cLB.ID;
            thanhVien_CLB.IDtvien = dangKy.IdTvien;
            thanhVien_CLB.IDRoles = 2;
            var userrole = from e in db.DBUserRoles
                           where e.UserID == dangKy.IdTvien
                           select new 
                           { 
                               roleid=e.RoleID,
                               userid = e.UserID
                           };
            if (userrole == null|| userrole.Count() == 0)
            {
                DBUserRoles userroless = new DBUserRoles();
                userroless.RoleID = 2;
                userroless.UserID = dangKy.IdTvien;
                db.DBUserRoles.Add(userroless);
            }
            else
            {
                Boolean hasRole1 = false;
                Boolean hasRole2 = false;
                foreach (var role in userrole)
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
                foreach (var role in userrole)
                {
                    if (hasRole1 && !hasRole2)
                    {
                        DBUserRoles userroles = new DBUserRoles();
                        userroles.RoleID = 2;
                        userroles.UserID = dangKy.IdTvien;
                        db.DBUserRoles.Add(userroles);
                    }
                }
              
            }
            db.ThanhVien_CLB.Add(thanhVien_CLB);
            db.DkyCLB.Remove(dangKy);
            db.SaveChanges();
            return RedirectToAction("QLDkyCLB");
        }
        public ActionResult TuChoiCLB(int? id)
        {
            DkyCLB dangKy = db.DkyCLB.Find(id);
            db.DkyCLB.Remove(dangKy);
            db.SaveChanges();
            return RedirectToAction("QLDkyCLB");
        }
    }
}
