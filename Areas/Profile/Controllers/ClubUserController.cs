using ClubPortalMS.Models;
using System;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    [CustomAuthenticationFilter]
  
    public class ClubUserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        #region' Thông báo CLB
        //Thông báo CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult ThongBao_CLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            List<CLB> clb = db.CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new CLBShowViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID,
                                   HinhCLB=i.HinhCLB
                               };
            ViewBag.DsCLB = Dsclbthamgia;

            List<ThongBao> thongBaos = db.ThongBao.ToList();
            var DsTBGanDay = from e in thanhVien_clb
                             join d in thongBaos on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDtvien == IdTvien
                             && e.IDRoles == 1
                             select new ThongBaoViewModel
                             {
                                 ThanhVien_CLB = e,
                                 ThongBao = d
                             };
            ViewBag.DsTBGanDay = DsTBGanDay.OrderByDescending(x => x.ThongBao.ID).Take(3).ToList();
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult ThongBaoCLB(int? id,int? page)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThongBao> thongBaos = db.ThongBao.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsThongBao = from e in thanhVien_clb                             
                               join i in thongBaos on e.IDCLB equals i.IdCLB into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               && i.IdCLB == id
                               select new ThongBaoViewModel
                               {
                                   ThanhVien_CLB = e,
                                   ThongBao = i
                               };
            ViewBag.idCLB = id;
            ViewBag.DsThongBao = DsThongBao.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult BaiVietTBCLB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBao thongBao = db.ThongBao.Find(id);
            ViewBag.idCLB = thongBao.IdCLB;
            if (thongBao == null)
            {
                return HttpNotFound();
            }
            return View(thongBao);
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public FileResult DocumentDownloadTB(int? id)
        {
            var thongBao = db.ThongBao.Where(u => u.ID == id).FirstOrDefault();
            return File(thongBao.File, thongBao.ContentType, thongBao.TenFile);
        }
        #endregion
        #region' Đăng ký CLB

        [HttpGet]
        public ActionResult DangKyCLB()
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
                                  && e.IDRoles == 1
                                  select new ViewModel1
                                 {
                                     ThanhVien_CLB = e,
                                     CLB = i
                                 };

            var hai = from a in thanhVien_clb
                                 where (a.IDtvien == IdTvien && a.IDRoles == 1) || (a.IDRoles==2 && a.IDtvien == IdTvien)
                                 select a;
            var ba = clb.Select(sc => sc.ID)
              .Union(hai.Select(st => st.IDCLB));          
            var DsclbchThamGia =
              from id in ba
              join sc in clb on id equals sc.ID into jsc
              from sc in jsc.DefaultIfEmpty()
              join st in hai on id equals st.IDCLB into jst
              from st in jst.DefaultIfEmpty()
              where st == null ^ sc == null
              select new ViewModel2
              { IDCLB = sc.ID,
                TenCLB = sc.TenCLB
              };
           
            ViewBag.Dsclbthamgia = Dsclbthamgia;   
            ViewBag.Test = new SelectList(DsclbchThamGia, "IDCLB","TenCLB");
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyCLB([Bind(Include = "ID,LyDoDkyCLB,IDCLB")] DangKy dangKy)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
      
                if (Session["UserId"] == null)
                {
                    return HttpNotFound();
                }
               
                var userAccount = db.DangKy.Where(u => u.IdTv == IdTvien && u.IDCLB == (dangKy.IDCLB)).FirstOrDefault();
                if (userAccount!=null)
                {
                    ViewBag.Messages = "Có lỗi xảy ra!!!";
                }
                else 
                {
                    var thanhVien = db.ThanhVien.SingleOrDefault(n => n.ID == IdTvien);
                    dangKy.SDT = thanhVien.SDT;
                    dangKy.MSSV = thanhVien.MSSV;
                    dangKy.Ten = thanhVien.Ten;
                    dangKy.Email = thanhVien.Mail;
                    dangKy.IdTv = IdTvien;
                    dangKy.NgayDangKy = DateTime.Now;
                    db.DangKy.Add(dangKy);
                    db.SaveChanges();
                    ViewBag.Message = "Đăng ký thành công!!!";
                }
            
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new ViewModel1
                               {
                                   ThanhVien_CLB = e,
                                   CLB = i
                               };
            var hai = from a in thanhVien_clb
                      where a.IDtvien == IdTvien && a.IDRoles == 1
                      select a;
            var ba = clb.Select(sc => sc.ID)
              .Union(hai.Select(st => st.IDCLB));
            var DsclbchThamGia =
              from id in ba
              join sc in clb on id equals sc.ID into jsc
              from sc in jsc.DefaultIfEmpty()
              join st in hai on id equals st.IDCLB into jst
              from st in jst.DefaultIfEmpty()
              where st == null ^ sc == null
              select new ViewModel2
              {
                  IDCLB = sc.ID,
                  TenCLB = sc.TenCLB
              };
            ViewBag.Dsclbthamgia = Dsclbthamgia;
            ViewBag.Test = new SelectList(DsclbchThamGia, "IDCLB", "TenCLB");
            return View(dangKy);
        }
        #endregion
        #region Phản hồi CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        [HttpGet]
        public ActionResult PhanHoiCLB()
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
                               && e.IDRoles == 1
                               select new PhanHoiCLBViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID
                               };
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhanHoiCLB(PhanHoiViewModel phanHoiViewModel)
        {
            int UserID = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
                ThanhVien thanhViens = db.ThanhVien.Find(UserID);
                PhanHoi phanHoi = new PhanHoi();
                phanHoi.Idtv = UserID;
                phanHoi.SDT = thanhViens.SDT;
                phanHoi.Ten = thanhViens.Ho + thanhViens.Ten;
                phanHoi.Email = thanhViens.Mail;
                phanHoi.TGphanhoi = DateTime.Now;
                phanHoi.NoiDung = phanHoiViewModel.NoiDung;
                phanHoi.IdCLB = phanHoiViewModel.IdCLB;
                db.PhanHoi.Add(phanHoi);
                db.SaveChanges();
                ViewBag.Messsage = "Cảm ơn bạn đã phản hồi";
            }
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == UserID
                               && e.IDRoles == 1
                               select new PhanHoiCLBViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID
                               };
            ViewBag.DsCLB = new SelectList(Dsclbthamgia, "IdCLB", "TenCLB");
            return View(phanHoiViewModel);
        }

        #endregion
        #region Danh sách Hoạt động CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult HoatDong_CLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            List<CLB> clb = db.CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new CLBShowViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID,
                                   HinhCLB = i.HinhCLB
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            List<QLDSHoatDong> hoatDongs = db.QLDSHoatDong.ToList();
            var DsHDGanDay = from e in thanhVien_clb
                             join d in hoatDongs on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDtvien == IdTvien
                             && e.IDRoles == 1
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 QLDSHoatDong = d
                             };
            ViewBag.DsHDGanDay = DsHDGanDay.OrderByDescending(x => x.QLDSHoatDong.ID).Take(3).ToList();
            return View()
;        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult HoatDongCLB(int? id,string message,string messages)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<QLDSHoatDong> QLDShoatDongs = db.QLDSHoatDong.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsHoatDong = from e in thanhVien_clb
                             join i in QLDShoatDongs on e.IDCLB equals i.IdCLB into table
                             from i in table.ToList()
                             where e.IDtvien == IdTvien
                             && e.IDRoles == 1
                             && i.IdCLB == id
                             select new HoatDongViewModel
                             {
                                 ThanhVien_CLB = e,
                                 QLDSHoatDong = i,
                             };
            ViewBag.Message = message;
            ViewBag.Messages = messages;
            ViewBag.idCLB = id;
            ViewBag.DsHoatDong = DsHoatDong.OrderByDescending(x => x.QLDSHoatDong.ID).ToList();
            return View(DsHoatDong);
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult ThamGiaHD(int? id)
        {
            string message = null;
            string messages = null;
            if (ModelState.IsValid)
            {
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                QLDSHoatDong qLDSHoatDong = db.QLDSHoatDong.Find(id);            
                var userAccount = db.TTNhatKy.Where(u => u.IDHoatDong==id).FirstOrDefault();
                if (userAccount != null)
                {
                    message = "Bạn đã tham gia hoạt động này rồi!!";
                }
                else
                {
                    messages = "Tham gia thành công!!";
                    var user = new TTNhatKy()
                    {
                        IdThanhVien = IdTvien,
                        TGThamGia = DateTime.Now,
                        SKDaThamGia = qLDSHoatDong.ChuDe,
                        IDHoatDong = qLDSHoatDong.ID,
                        DiemDanh = false
                    };
                    db.TTNhatKy.Add(user);
                    db.SaveChanges();
                }
                return RedirectToAction("HoatDongCLB", new { id = qLDSHoatDong.IdCLB, message= message, messages= messages });
            }
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult BaiVietHDCLB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QLDSHoatDong hoatDong = db.QLDSHoatDong.Find(id);
            ViewBag.idCLB = hoatDong.IdCLB;
            if (hoatDong == null)
            {
                return HttpNotFound();
            }
            return View(hoatDong);
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public FileResult DocumentDownloadHD(int? id)
        {
            var hoatDong = db.QLDSHoatDong.Where(u => u.ID == id).FirstOrDefault();
            return File(hoatDong.File, hoatDong.ContentType, hoatDong.TenFile);
        }
        #endregion
        #region Ds sự kiện CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult SuKien_CLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            List<CLB> clb = db.CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new CLBShowViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID,
                                   HinhCLB = i.HinhCLB
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            List<SuKien> suKiens = db.SuKien.ToList();
            var DsSKGanDay = from e in thanhVien_clb
                             join d in suKiens on e.IDCLB equals d.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDtvien == IdTvien
                             && e.IDRoles == 1
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 SuKien = d
                             };
            ViewBag.DsSKGanDay = DsSKGanDay.OrderByDescending(x => x.SuKien.ID).Take(3).ToList();
            return View()
;
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult SuKienCLB( int? id , string message, string messages)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<SuKien> suKiens = db.SuKien.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var DsSuien = from e in thanhVien_clb
                             join i in suKiens on e.IDCLB equals i.IdCLB into table
                             from i in table.ToList()
                             where e.IDtvien == IdTvien
                             && e.IDRoles == 1
                             && e.IDCLB == id
                             select new SukienViewModel
                             {
                                 ThanhVien_CLB = e,
                                 SuKien = i
                             };
            ViewBag.Message = message;
            ViewBag.Messages = messages;
            ViewBag.idCLB = id;
            ViewBag.DsSukien = DsSuien.OrderByDescending(x => x.SuKien.ID).ToList(); ;
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult ThamGiaSK(int? id)
        {
            string message = null;
            string messages = null;
            if (ModelState.IsValid)
            {
               
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                SuKien suKien = db.SuKien.Find(id);
                var userAccount = db.TTNhatKy.Where(u => u.IDSuKien==id).FirstOrDefault();
                if (userAccount != null)
                {
                    message = "Bạn đã tham gia hoạt động này rồi!!";
                }
                else
                {
                    var user = new TTNhatKy()
                    {
                        IdThanhVien = IdTvien,
                        TGThamGia = DateTime.Now,
                        SKDaThamGia = suKien.TieuDeSK,
                        IDSuKien = suKien.ID,
                        DiemDanh = false
                    };
                    db.TTNhatKy.Add(user);
                    db.SaveChanges();
                    messages = "Tham gia thành công!!";
                }
                return RedirectToAction("SuKienCLB", new { id = suKien.IdCLB, message = message, messages = messages });
            }
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public FileResult DocumentDownloadSK(int? id)
        {
            var suKien = db.SuKien.Where(u => u.ID == id).FirstOrDefault();
            return File(suKien.File, suKien.ContentType, suKien.TenFile);
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult BaiVietSKCLB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuKien suKien = db.SuKien.Find(id);
            ViewBag.idCLB = suKien.IdCLB;
            if (suKien == null)
            {
                return HttpNotFound();
            }
            return View(suKien);
        }
        #endregion
        #region Nhật ký CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult NhatKyCLB(int? page)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<TTNhatKy> tTNhatKies = db.TTNhatKy.ToList();
            var listNhatKy = from e in tTNhatKies
                             where e.IdThanhVien == IdTvien
                             select e;
            ViewBag.ListNhatKy = listNhatKy.OrderByDescending(x => x.ID).ToList();
            return View();
        }
        #endregion
        #region Đăng ký thành lập CLB

        [HttpGet]
        public ActionResult DangKyTLCLB()
        {
            ViewBag.IDLoaiCLB = new SelectList(db.LoaiCLB, "IDLoaiCLB", "TenLoaiCLB");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyTLCLB([Bind(Include = "ID,TenCLB,IDLoaiCLB,LyDoThanhLap")] DkyCLB dkyCLB)
        {
            if (ModelState.IsValid)
            {
                int IdTvien = Convert.ToInt32(Session["UserId"]);
                dkyCLB.IdTvien = IdTvien;         
                db.DkyCLB.Add(dkyCLB);
                db.SaveChanges();
                ViewBag.Messsage = "Đăng ký thành công!!!";
            }
            ViewBag.IDLoaiCLB = new SelectList(db.LoaiCLB, "IDLoaiCLB", "TenLoaiCLB", dkyCLB.IDLoaiCLB);
            return View(dkyCLB);
        }
        #endregion
        #region Lịch tap
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult LichTap_CLB()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                                   //join d in thanhVien on e.IDtvien equals IdTvien into table1
                                   //from d in table1.ToList()
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new CLBShowViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   IdCLB = i.ID,
                                   HinhCLB = i.HinhCLB
                               };
            ViewBag.Dsclbthamgia = Dsclbthamgia;
            List<LichTap_ThanhVien> lichTaps = db.LichTap_ThanhVien.ToList();
            var DsLTGanDay = from e in thanhVien_clb
                             join d in lichTaps on e.IDCLB equals d.LichTap.IdCLB into table1
                             from d in table1.ToList()
                             where e.IDtvien == d.IDTvien 
                             && d.IDTvien == IdTvien
                             && e.IDRoles == 1
                             
                             select new ViewModel1
                             {
                                 ThanhVien_CLB = e,
                                 LichTap_ThanhVien = d
                             };
            ViewBag.DsLTGanDay = DsLTGanDay.OrderByDescending(x => x.LichTap_ThanhVien.IdLT).Take(3).ToList();
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult LichTap(int? id)
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<LichTap_ThanhVien> lichTap_ThanhVien = db.LichTap_ThanhVien.ToList();
            var DsLichTap = from e in lichTap_ThanhVien
                              where e.LichTap.IdCLB == id
                              && e.IDTvien == IdTvien
                              select e;        
            ViewBag.idCLB = id;
            ViewBag.DsLichTap = DsLichTap.ToList().OrderByDescending(x => x.LichTap.ID).ToList();
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult BaiVietLTCLB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichTap lichTap = db.LichTap.Find(id);
            ViewBag.idCLB = lichTap.IdCLB;
            if (lichTap == null)
            {
                return HttpNotFound();
            }
            return View(lichTap);
        }
        #endregion
        #region CLB
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult DSCLB()
        {
            int IdTvien = Convert.ToInt32(Session["UserId"]);
            List<CLB> clb = db.CLB.ToList();
            List<ThanhVien_CLB> thanhVien_clb = db.ThanhVien_CLB.ToList();
            var Dsclbthamgia = from e in thanhVien_clb
                               join i in clb on e.IDCLB equals i.ID into table
                               from i in table.ToList()
                               where e.IDtvien == IdTvien
                               && e.IDRoles == 1
                               select new CLBDaThamGiaViewModel
                               {
                                   TenCLB = i.TenCLB,
                                   HinhCLB = i.HinhCLB,
                                   IDCLB = i.ID,
                                   Mota = i.Mota,
                                   NgayThanhLap = i.NgayThanhLap
                               };
            ViewBag.DsCLB = Dsclbthamgia;
            return View();
        }
        [CustomAuthorize("Member", "HLV", "Admin")]
        public ActionResult ChiTietCLB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLB cLB = db.CLB.Find(id);
            if (cLB == null)
            {
                return HttpNotFound();
            }
            return View(cLB);
        }
        #endregion
    }
}