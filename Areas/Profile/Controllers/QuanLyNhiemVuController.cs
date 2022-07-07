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
    public class QuanLyNhiemVuController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLNhiemVu_CLB()
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
        public ActionResult QLNhiemVu(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<NhiemVu> nhiemVus = db.NhiemVu.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsNhiemVu = from e in thanhVien_clb
                            join d in nhiemVus on e.IDCLB equals d.IdCLB into table1
                            from d in table1.ToList()
                            where e.IDCLB == id && e.IDRoles == 2
                            select new ViewModel1
                            {
                                ThanhVien_CLB = e,
                                //CLB = is
                                NhiemVu = d
                            };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsNhiemVu = DsNhiemVu.OrderByDescending(x => x.NhiemVu.ID).ToList();
            return View();
        }
        public ActionResult XemNV(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhiemVu nhiemVu = db.NhiemVu.Find(id);
            if (nhiemVu == null)
            {
                return HttpNotFound();
            }
            return View(nhiemVu);
        }

        // GET: Admin/ThongBaos/Create
        [HttpGet]
        public ActionResult TaoNV()
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
        public ActionResult TaoNV([Bind(Include = "ID,TieuDe,MoTa,ThoiGianKetThuc,File,IdCLB")] NhiemVu nhiemVu, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                if (uploadfile != null)
                {
                    int filelength = uploadfile.ContentLength;
                    string fileName = uploadfile.FileName;
                    string contentType = uploadfile.ContentType;
                    byte[] Myfile = new byte[filelength];
                    uploadfile.InputStream.Read(Myfile, 0, filelength);
                    nhiemVu.File = Myfile;
                    nhiemVu.TenFile = fileName;
                    nhiemVu.ContentType = contentType;
                    db.NhiemVu.Add(nhiemVu);
                    db.SaveChanges();
                    List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
                    var thanhvien_CLB = from e in thanhVien_clb
                                        where e.IDCLB == nhiemVu.IdCLB
                                        && e.IDRoles == 1
                                        select new ViewModel1
                                        {
                                            ThanhVien_CLB = e,
                                        };
                    foreach (ViewModel1 dsTV in thanhvien_CLB)
                    {
                        NhiemVu_ThanhVien nhiemVu_ThanhVien = new NhiemVu_ThanhVien();
                        nhiemVu_ThanhVien.IdNv = nhiemVu.ID;
                        nhiemVu_ThanhVien.IdTVien = dsTV.ThanhVien_CLB.IDtvien;
                        db.NhiemVu_ThanhVien.Add(nhiemVu_ThanhVien);
                        db.SaveChanges();
                    }
                    return RedirectToAction("QLNhiemVu_CLB", new { id = IdTvien });
                }
                else
                {
                    db.NhiemVu.Add(nhiemVu);
                    db.SaveChanges();
                    List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
                    var thanhvien_CLB = from e in thanhVien_clb
                                        where e.IDCLB == nhiemVu.IdCLB
                                        && e.IDRoles == 1
                                        select new ViewModel1
                                        {
                                            ThanhVien_CLB = e,
                                        };
                    foreach (ViewModel1 dsTV in thanhvien_CLB)
                    {
                        NhiemVu_ThanhVien nhiemVu_ThanhVien = new NhiemVu_ThanhVien();
                        nhiemVu_ThanhVien.IdNv = nhiemVu.ID ;
                        nhiemVu_ThanhVien.IdTVien = dsTV.ThanhVien_CLB.IDtvien;
                        db.NhiemVu_ThanhVien.Add(nhiemVu_ThanhVien);
                        db.SaveChanges();
                    }
                    return RedirectToAction("QLNhiemVu_CLB", new { id = IdTvien });
                }
            }
            return View(nhiemVu);
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var nhiemVu = db.NhiemVu.Where(u => u.ID == id).FirstOrDefault();
            return File(nhiemVu.File, nhiemVu.ContentType, nhiemVu.TenFile);
        }
        [HttpGet]
        public FileResult DocumentExDownload(int? id)
        {
            var nhiemVu = db.NhiemVu_ThanhVien.Where(u => u.ID == id).FirstOrDefault();
            return File(nhiemVu.FileNop, nhiemVu.ContentType, nhiemVu.TenFileNop);
        }
        [HttpGet]
        public ActionResult SuaNV(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhiemVu nhiemVu = db.NhiemVu.Find(id);
            if (nhiemVu == null)
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
            return View(nhiemVu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNV([Bind(Include = "ID,TieuDe,MoTa,ThoiGianKetThuc,File,IdCLB")] NhiemVu nhiemVu, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    int filelength = upload.ContentLength;
                    string fileName = upload.FileName;
                    string contentType = upload.ContentType;
                    byte[] Myfile = new byte[filelength];
                    upload.InputStream.Read(Myfile, 0, filelength);
                    nhiemVu.File = Myfile;
                    nhiemVu.TenFile = fileName;
                    nhiemVu.ContentType = contentType;
                    db.Entry(nhiemVu).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    
                    NhiemVu nhiemVus = db.NhiemVu.Find(nhiemVu.ID);

                        nhiemVus.IdCLB = nhiemVu.IdCLB;
                        nhiemVus.TieuDe = nhiemVu.TieuDe;
                        nhiemVus.MoTa = nhiemVu.MoTa;
                        nhiemVus.ThoiGianKetThuc = nhiemVu.ThoiGianKetThuc;
                        db.SaveChanges();

                }
                //Sửa lại nhiệm vụ cho thành viên theo CLB
                List<NhiemVu_ThanhVien> nhiemVu_ThanhViens = db.NhiemVu_ThanhVien.ToList();
                var nhiemVu_thanhVien = from e in nhiemVu_ThanhViens
                                          where e.IdNv == nhiemVu.ID
                                          select new ViewModel1
                                          {
                                              NhiemVu_ThanhVien = e
                                          };
                foreach (ViewModel1 dsTV in nhiemVu_thanhVien)
                {
                    NhiemVu_ThanhVien nhiemVu_ThanhVien = db.NhiemVu_ThanhVien.Find(dsTV.NhiemVu_ThanhVien.ID);
                    db.NhiemVu_ThanhVien.Remove(nhiemVu_ThanhVien);
                    db.SaveChanges();
                }

                List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
                var thanhvien_CLB = from e in thanhVien_clb
                                    where e.IDCLB == nhiemVu.IdCLB
                                    && e.IDRoles == 1
                                    select new ViewModel1
                                    {
                                        ThanhVien_CLB = e,
                                    };
                foreach (ViewModel1 dsTV in thanhvien_CLB)
                {
                    NhiemVu_ThanhVien nhiemVu_ThanhVien = new NhiemVu_ThanhVien();
                    nhiemVu_ThanhVien.IdNv = nhiemVu.ID;
                    nhiemVu_ThanhVien.IdTVien = dsTV.ThanhVien_CLB.IDtvien;
                    db.NhiemVu_ThanhVien.Add(nhiemVu_ThanhVien);
                    db.SaveChanges();
                }
                return RedirectToAction("XemNV", new { id=nhiemVu.ID });
            }
            return View(nhiemVu);
        }
 
        public ActionResult XacNhanXoaNV(int? id)
        {
            NhiemVu nhiemVu = db.NhiemVu.Find(id);
            List<NhiemVu_ThanhVien> nhiemVu_ThanhViens = db.NhiemVu_ThanhVien.ToList();
            var nhiemVu_thanhVien = from e in nhiemVu_ThanhViens
                                    where e.IdNv == nhiemVu.ID
                                    select new ViewModel1
                                    {
                                        NhiemVu_ThanhVien = e
                                    };
            foreach (ViewModel1 dsTV in nhiemVu_thanhVien)
            {
                NhiemVu_ThanhVien nhiemVu_ThanhVien = db.NhiemVu_ThanhVien.Find(dsTV.NhiemVu_ThanhVien.ID);
                db.NhiemVu_ThanhVien.Remove(nhiemVu_ThanhVien);
                db.SaveChanges();
            }
            db.NhiemVu.Remove(nhiemVu);
            db.SaveChanges();
            return RedirectToAction("QLNhiemVu", new {id= nhiemVu.IdCLB });
        }
        public ActionResult XemBaiNop(int? id, int? id2)
        {
            List<NhiemVu_ThanhVien> nhiemVu_ThanhVien = db.NhiemVu_ThanhVien.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsthanhVienNV = from e in thanhVien_clb
                                join i in nhiemVu_ThanhVien on e.IDtvien equals i.IdTVien into table
                                from i in table.ToList()
                                where e.IDRoles == 1
                                && i.IdNv == id
                                && e.IDCLB == id2
                                select new ViewModel1
                                {
                                    NhiemVu_ThanhVien = i,
                                    ThanhVien_CLB = e
                                };
            ViewBag.IDNV = id;
            ViewBag.DsthanhVienNV = DsthanhVienNV.OrderByDescending(x => x.NhiemVu_ThanhVien.ID).ToList();
            return View();
        }

    }
}