using ClubPortalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Text;
using PagedList.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HLV", "Admin")]
    public class QuanLyHoatDongController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLHoatDong_CLB()
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
                                   HinhCLB = i.HinhCLB,
                                   TenCLB = i.TenCLB,
                                   IDCLB = i.ID,
                                   Mota = i.Mota,
                                   NgayThanhLap = i.NgayThanhLap
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }
        public ActionResult QLHoatDong(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<QLDSHoatDong> hoatDongs = db.QLDSHoatDong.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsHoatDong = from e in thanhVien_clb
                             join d in hoatDongs on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDCLB == id && e.IDRoles == 2
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 //CLB = i,
                                 QLDSHoatDong = d
                             };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsHoatDong = DsHoatDong.OrderByDescending(x => x.QLDSHoatDong.ID).ToList();
            return View();
        }
        public ActionResult XemHD(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QLDSHoatDong hoatDong = db.QLDSHoatDong.Find(id);
            if (hoatDong == null)
            {
                return HttpNotFound();
            }
            return View(hoatDong);
        }

        // GET: Admin/ThongBaos/Create
        [HttpGet]
        public ActionResult TaoHD()
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
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD");
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoHD([Bind(Include = "ID,ChuDe,MoTa,NgayBatDau,NgayKetThuc,NoiDung,DiaDiem,File,IdCLB,IdLoaiHD")] QLDSHoatDong hoatDong, HttpPostedFileBase uploadfile)
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
                    hoatDong.File = Myfile;
                    hoatDong.TenFile = fileName;
                    hoatDong.ContentType = contentType;
                    db.QLDSHoatDong.Add(hoatDong);
                    db.SaveChanges();
                   
                }
                else
                {
                    db.QLDSHoatDong.Add(hoatDong);
                    db.SaveChanges();
                  
                }
                return RedirectToAction("QLHoatDong_CLB", new { id = IdTvien });
            }         
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
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD", hoatDong.IdLoaiHD);
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB",hoatDong.IdCLB);
            return View(hoatDong);
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var hoatDong = db.QLDSHoatDong.Where(u => u.ID == id).FirstOrDefault();
            return File(hoatDong.File, hoatDong.ContentType, hoatDong.TenFile);
        }
        // GET: Admin/ThongBaos/Edit/5
        public ActionResult SuaHD(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QLDSHoatDong hoatDong = db.QLDSHoatDong.Find(id);
            if (hoatDong == null)
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
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD", hoatDong.IdLoaiHD);
            return View(hoatDong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHD([Bind(Include = "ID,ChuDe,MoTa,NgayBatDau,NgayKetThuc,NoiDung,DiaDiem,File,IdCLB,IdLoaiHD")] QLDSHoatDong hoatDong, HttpPostedFileBase upload)
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
                    hoatDong.File = Myfile;
                    hoatDong.TenFile = fileName;
                    hoatDong.ContentType = contentType;
                    db.Entry(hoatDong).State = EntityState.Modified;
                    db.SaveChanges();
                   
                }
                else
                {
                    QLDSHoatDong hoatDongs = db.QLDSHoatDong.Find(hoatDong.ID);
                        hoatDongs.IdCLB = hoatDong.IdCLB;
                        hoatDongs.ChuDe = hoatDong.ChuDe;
                        hoatDongs.Mota = hoatDong.Mota;
                        hoatDongs.NgayBatDau = hoatDong.NgayBatDau;
                        hoatDongs.NgayKetThuc = hoatDong.NgayKetThuc;
                        hoatDongs.NoiDung = hoatDong.NoiDung;
                        hoatDongs.DiaDiem = hoatDong.DiaDiem;
                        hoatDongs.IdLoaiHD = hoatDong.IdLoaiHD;
                        db.SaveChanges();
                    
                }
                return RedirectToAction("XemHD", new { id=hoatDong.ID });
            }
            ViewBag.IdLoaiHD = new SelectList(db.LoaiHD, "ID", "TenLoaiHD", hoatDong.IdLoaiHD);
            return View(hoatDong);
        }

        public ActionResult XacNhanXoaHD(int? id)
        {
            QLDSHoatDong hoatDong = db.QLDSHoatDong.Find(id);
            db.QLDSHoatDong.Remove(hoatDong);
            db.SaveChanges();
            return RedirectToAction("QLHoatDong", new { id = hoatDong.IdCLB });
        }

        [HttpGet]
        public ActionResult DiemDanh( int? id, int? id2) {
            List<TTNhatKy> tTNhatKies = db.TTNhatKy.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsthanhVienHD = from e in thanhVien_clb
                                join i in tTNhatKies on e.IDtvien equals i.IdThanhVien into table
                                from i in table.ToList()
                                where e.IDRoles == 1
                                && i.IDHoatDong == id
                                && e.IDCLB == id2
                                select new DiemDanhCLBViewModel
                                {
                                 ID = i.ID,
                                 DiemDanh = i.DiemDanh,
                                
                                 ThanhVien_CLB =e
                               };
            ViewBag.IdHD = id;
            return View(DsthanhVienHD.ToList()); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiemDanh( List<DiemDanhCLBViewModel> DsthanhVienHD)
        {
            if (ModelState.IsValid)
            {
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                if (DsthanhVienHD != null)
                {
                    foreach (DiemDanhCLBViewModel ttnhatKy in DsthanhVienHD)
                    {
                        TTNhatKy tTNhatKy = db.TTNhatKy.Find(ttnhatKy.ID);
                        tTNhatKy.DiemDanh = ttnhatKy.DiemDanh;
                    }                
                    db.SaveChanges();
                    return RedirectToAction("QLHoatDong_CLB", new { id = IdTvien });
                }
            }
            return View(DsthanhVienHD);
        }
    }
}