using ClubPortalMS.Models;
using PagedList.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyThongBaoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLTBCLB()
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
                                   HinhCLB=i.HinhCLB,
                                   Mota = i.Mota,
                                   NgayThanhLap = i.NgayThanhLap
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }
        public ActionResult QLThongBao(int? id, int? page)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThongBao> thongBaos = db.ThongBao.ToList();
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsThongBao = from e in thanhVien_clb
                             join d in thongBaos on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                                 //join i in clb on e.IDCLB equals i.ID into table
                                 //from i in table.ToList()
                             where e.IDCLB == id && e.IDRoles == 2
                             select new ThongBaoViewModel
                             {
                                 ThanhVien_CLB = e,
                                 //CLB = i,
                                 ThongBao = d
                             };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsThongBao = DsThongBao.OrderByDescending(x => x.ThongBao.ID).ToList();
            return View();
        }
        public ActionResult XemChiTietTB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao thongBao = db.ThongBao.Find(id);
            if (thongBao == null)
            {
                return HttpNotFound();
            }
            return View(thongBao);
        }

        // GET: Admin/ThongBaos/Create
        [HttpGet]
        public ActionResult TaoTB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            // List<ThanhVien> thanhVien = db.ThanhVien.ToList();
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
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
            return View();
        }

        // POST: Admin/ThongBaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoTB([Bind(Include = "ID,TieuDe,MoTa,IdCLB,NgayThongBao,NoiDung,File")] ThongBao thongBao, HttpPostedFileBase uploadfile)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            { 
                if (uploadfile != null)
                {
                    int filelength = uploadfile.ContentLength;
                    string fileName = uploadfile.FileName;
                    string contentType = uploadfile.ContentType;
                    byte[] Myfile = new byte[filelength];
                    uploadfile.InputStream.Read(Myfile, 0, filelength);
                    thongBao.File = Myfile;
                    thongBao.TenFile = fileName;
                    thongBao.ContentType = contentType;
                    db.ThongBao.Add(thongBao);
                    db.SaveChanges();
                   
                }
                else
                {
                    db.ThongBao.Add(thongBao);
                    db.SaveChanges();
                }
                return RedirectToAction("QLTBCLB", new { id = IdTvien });
            }
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
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
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", thongBao.IdCLB);
            return View(thongBao);
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var thongBao = db.ThongBao.Where(u => u.ID == id).FirstOrDefault();
            return File(thongBao.File, thongBao.ContentType, thongBao.TenFile);
        }
        // GET: Admin/ThongBaos/Edit/5
        public ActionResult SuaTB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao thongBao = db.ThongBao.Find(id);
            if (thongBao == null)
            {
                return HttpNotFound();
            }
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            // List<ThanhVien> thanhVien = db.ThanhVien.ToList();
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
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
            return View(thongBao);
        }

        // POST: Admin/ThongBaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTB([Bind(Include = "ID,TieuDe,MoTa,IdCLB,NgayThongBao,NoiDung,File")] ThongBao thongBao, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {   if (upload != null) { 
                int filelength = upload.ContentLength;
                string fileName = upload.FileName;
                string contentType = upload.ContentType;
                byte[] Myfile = new byte[filelength];
                upload.InputStream.Read(Myfile, 0, filelength);
                thongBao.File = Myfile;
                thongBao.TenFile = fileName;
                thongBao.ContentType = contentType;
                db.Entry(thongBao).State = EntityState.Modified;
                db.SaveChanges();
                
                }
                else
                {
                    ThongBao thongBaos = db.ThongBao.Find(thongBao.ID);

                        thongBaos.IdCLB = thongBao.IdCLB;
                        thongBaos.TieuDe = thongBao.TieuDe;
                        thongBaos.MoTa = thongBao.MoTa;
                        thongBaos.NgayThongBao = thongBao.NgayThongBao;
                        thongBaos.NoiDung = thongBao.NoiDung;
                        db.SaveChanges();

                }
            }
           
            ViewBag.IdCLB = new SelectList(db.CLB, "ID", "TenCLB", thongBao.IdCLB);
            return RedirectToAction("XemChiTietTB", new { id = thongBao.ID });
        }

        public ActionResult XacNhanXoaTB(int id)
        {
            ThongBao thongBao = db.ThongBao.Find(id);
            db.ThongBao.Remove(thongBao);
            db.SaveChanges();
            return RedirectToAction("QLThongBao", new { id = thongBao.IdCLB });

        }
    }
}