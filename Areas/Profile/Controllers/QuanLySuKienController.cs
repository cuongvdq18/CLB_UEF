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
    public class QuanLySuKienController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult QLSuKien_CLB()
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
        public ActionResult QLSuKien(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<SuKien> suKiens = db.SuKien.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsSuKien = from e in thanhVien_clb
                             join d in suKiens on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDCLB == id && e.IDRoles == 2
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 SuKien = d
                             };
            int? idCLB = id;
            ViewBag.idCLB = idCLB;
            ViewBag.DsSuKien = DsSuKien.OrderByDescending(x => x.SuKien.ID).ToList(); 
            return View();
        }
        public ActionResult XemSK(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuKien suKien = db.SuKien.Find(id);
            if (suKien == null)
            {
                return HttpNotFound();
            }
            return View(suKien);
        }

        // GET: Admin/ThongBaos/Create
        [HttpGet]
        public ActionResult TaoSK()
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
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK");
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoSK([Bind(Include = "ID,TieuDeSK,MoTa,NgayBatDau,NgayKetThuc,NoiDung,DiaDiem,File,IdCLB,IdLoaiSK")] SuKien suKien, HttpPostedFileBase uploadfile)
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
                    suKien.File = Myfile;
                    suKien.TenFile = fileName;
                    suKien.ContentType = contentType;
                    db.SuKien.Add(suKien);
                    db.SaveChanges();
                    return RedirectToAction("QLSuKien_CLB", new { id = IdTvien });
                }
                else
                {
                    db.SuKien.Add(suKien);
                    db.SaveChanges();
                    return RedirectToAction("QLSuKien_CLB", new { id = IdTvien });
                }
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
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK", suKien.IdLoaiSK);
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB", suKien.IdCLB);
            return View(suKien);
        }
        [HttpGet]
        public FileResult DocumentDownload(int? id)
        {
            var suKien = db.SuKien.Where(u => u.ID == id).FirstOrDefault();
            return File(suKien.File, suKien.ContentType, suKien.TenFile);
        }
        // GET: Admin/ThongBaos/Edit/5
        public ActionResult SuaSK(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuKien suKien = db.SuKien.Find(id);
            if (suKien == null)
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
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK", suKien.IdLoaiSK);
            return View(suKien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSK([Bind(Include = "ID,TieuDeSK,MoTa,NgayBatDau,NgayKetThuc,NoiDung,DiaDiem,File,IdCLB,IdLoaiSK")] SuKien suKien, HttpPostedFileBase upload)
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
                    suKien.File = Myfile;
                    suKien.TenFile = fileName;
                    suKien.ContentType = contentType;
                    db.Entry(suKien).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }
             
                 else
                {
                    SuKien suKiens = db.SuKien.Find(suKien.ID);
                        suKiens.IdCLB = suKien.IdCLB;
                        suKiens.TieuDeSK = suKien.TieuDeSK;
                        suKiens.MoTa = suKien.MoTa;
                        suKiens.NgayBatDau = suKien.NgayBatDau;
                        suKiens.NgayKetThuc = suKien.NgayKetThuc;
                        suKiens.NoiDung = suKien.NoiDung;
                        suKiens.DiaDiem = suKien.DiaDiem;
                        suKiens.IdLoaiSK = suKien.IdLoaiSK;
                        db.SaveChanges();
                }
                return RedirectToAction("XemSK", new { id = suKien.ID });
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
            ViewBag.IdLoaiSK = new SelectList(db.LoaiSuKien, "ID", "TenLoaiSK", suKien.IdLoaiSK);
            return View(suKien);
        }

        public ActionResult XacNhanXoaSK(int? id)
        {
            SuKien suKien = db.SuKien.Find(id);
            db.SuKien.Remove(suKien);
            db.SaveChanges();
            return RedirectToAction("QLSuKien", new {id= suKien.IdCLB });
        }
        [HttpGet]
        public ActionResult DiemDanh(int? id, int? id2)
        {
            List<TTNhatKy> tTNhatKies = db.TTNhatKy.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsthanhVienSK = from e in thanhVien_clb
                                join i in tTNhatKies on e.IDtvien equals i.IdThanhVien into table
                                from i in table.ToList()
                                where e.IDRoles == 1
                                && i.IDSuKien == id
                                && e.IDCLB == id2
                                select new DiemDanhCLBViewModel
                                {
                                    ID = i.ID,
                                   
                                    DiemDanh = i.DiemDanh,
                                    ThanhVien_CLB = e
                                };
            ViewBag.IdSK = id;
            return View(DsthanhVienSK.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiemDanh(List<DiemDanhCLBViewModel> DsthanhVienSK)
        {
            if (ModelState.IsValid)
            {
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                if (DsthanhVienSK != null)
                {
                    foreach (DiemDanhCLBViewModel ttnhatKy in DsthanhVienSK)
                    {
                        TTNhatKy tTNhatKy = db.TTNhatKy.Find(ttnhatKy.ID);
                        tTNhatKy.DiemDanh = ttnhatKy.DiemDanh;
                    }
                    db.SaveChanges();
                    return RedirectToAction("QLSuKien_CLB", new { id = IdTvien });
                }
            }
            return View(DsthanhVienSK);
        }
    }
}