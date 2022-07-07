using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.HoatDong;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class HoatDongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/HoatDongs
        public ActionResult Index()
        {
            List<HoatDong> hoatDongs = db.HoatDong.ToList();
            var ds = from e in hoatDongs
                     select new HoatDongsViewModel
                     {
                         ID = e.ID,
                         Ten = e.Ten,
                         MoTa = e.MoTa,
                         NoiDung = e.NoiDung,
                         KeyWord = e.KeyWord,
                         URL = e.URL,
                         HinhAnhBaiViet = e.HinhAnhBaiViet,
                         HinhAnhChiTiet = e.HinhAnhChiTiet,
                         NgayDang = e.NgayDang,
                         TenNguoiDang = e.TenNguoiDang
                     };

            return View(ds);
        }

        // GET: Admin/HoatDongs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.HoatDong.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new HoatDongsViewModel
            {
                ID = data.ID,
                Ten = data.Ten,
                MoTa = data.MoTa,
                NoiDung = data.NoiDung,
                KeyWord = data.KeyWord,
                URL = data.URL,
                HinhAnhBaiViet = data.HinhAnhBaiViet,
                HinhAnhChiTiet = data.HinhAnhChiTiet,
                NgayDang = data.NgayDang,
                TenNguoiDang = data.TenNguoiDang

            };
            return View(viewModel);
        }

        // GET: Admin/HoatDongs/Create
        public ActionResult Create()
        {
            var create = new HoatDongsViewModel();
            return View(create);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HoatDongsViewModel hoatDong)
        {
            if (ModelState.IsValid)
            {
                if (hoatDong.ImageFile == null || hoatDong.ImageDetailFile == null)
                {
                    ViewBag.Message = "Bạn chưa chọn ảnh cho bài viết";
                    return View(hoatDong);
                }
                string hinhanh = Path.GetFileNameWithoutExtension(hoatDong.ImageFile.FileName);
                string hinhanhchitiet = Path.GetFileNameWithoutExtension(hoatDong.ImageDetailFile.FileName);

                string imgExtension = Path.GetExtension(hoatDong.ImageFile.FileName);

                string hinhanhchitietExtension = Path.GetExtension(hoatDong.ImageDetailFile.FileName);

                hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
                hinhanhchitiet = hinhanhchitiet + DateTime.Now.ToString("yyyymmssfff") + hinhanhchitietExtension;

                hoatDong.HinhAnhBaiViet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanh;
                hoatDong.HinhAnhChiTiet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanhchitiet;
                hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanh);
                hinhanhchitiet = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanhchitiet);
                hoatDong.ImageFile.SaveAs(hinhanh);
                hoatDong.ImageDetailFile.SaveAs(hinhanhchitiet);


                HoatDong hoatDongs = new HoatDong();
                hoatDongs.ID = hoatDong.ID;
                hoatDongs.Ten = hoatDong.Ten;
                hoatDongs.MoTa = hoatDong.MoTa;
                hoatDongs.NoiDung = hoatDong.NoiDung;
                hoatDongs.HinhAnhBaiViet = hoatDong.HinhAnhBaiViet;
                hoatDongs.HinhAnhChiTiet = hoatDong.HinhAnhChiTiet;
                hoatDongs.NgayDang = DateTime.Now;
                hoatDongs.LuotView = 0;
                hoatDongs.TenNguoiDang = hoatDong.TenNguoiDang;
                db.HoatDong.Add(hoatDongs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoatDong);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.HoatDong.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new HoatDongsViewModel
            {
                ID = data.ID,
                Ten = data.Ten,
                MoTa = data.MoTa,
                NoiDung = data.NoiDung,
                KeyWord = data.KeyWord,
                URL = data.URL,
                HinhAnhBaiViet = data.HinhAnhBaiViet,
                HinhAnhChiTiet = data.HinhAnhChiTiet,
                NgayDang = data.NgayDang,
                TenNguoiDang = data.TenNguoiDang

            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoatDongsViewModel hoatDongsViewModel, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.HoatDong.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            if (hoatDongsViewModel.ImageFile == null && hoatDongsViewModel.ImageDetailFile == null)
            {
                data.ID = hoatDongsViewModel.ID;
                data.Ten = hoatDongsViewModel.Ten;
                data.MoTa = hoatDongsViewModel.MoTa;
                data.NoiDung = hoatDongsViewModel.NoiDung;
                data.KeyWord = hoatDongsViewModel.KeyWord;
                data.URL = hoatDongsViewModel.URL;
                data.NgayDang = hoatDongsViewModel.NgayDang;
                data.TenNguoiDang = hoatDongsViewModel.TenNguoiDang;

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hoatDongsViewModel.ImageFile == null)
            {
                string hinhanhchitiet = Path.GetFileNameWithoutExtension(hoatDongsViewModel.ImageDetailFile.FileName);
                string hinhanhchitietExtension = Path.GetExtension(hoatDongsViewModel.ImageDetailFile.FileName);
                hinhanhchitiet = hinhanhchitiet + DateTime.Now.ToString("yyyymmssfff") + hinhanhchitietExtension;
                hoatDongsViewModel.HinhAnhChiTiet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanhchitiet;
                hinhanhchitiet = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanhchitiet);
                hoatDongsViewModel.ImageDetailFile.SaveAs(hinhanhchitiet);
                data.ID = hoatDongsViewModel.ID;
                data.Ten = hoatDongsViewModel.Ten;
                data.MoTa = hoatDongsViewModel.MoTa;
                data.HinhAnhChiTiet = hoatDongsViewModel.HinhAnhChiTiet;
                data.NoiDung = hoatDongsViewModel.NoiDung;
                data.KeyWord = hoatDongsViewModel.KeyWord;
                data.URL = hoatDongsViewModel.URL;
                data.NgayDang = hoatDongsViewModel.NgayDang;
                data.TenNguoiDang = hoatDongsViewModel.TenNguoiDang;

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hoatDongsViewModel.ImageDetailFile == null)
            {
                string hinhanh = Path.GetFileNameWithoutExtension(hoatDongsViewModel.ImageFile.FileName);
                string imgExtension = Path.GetExtension(hoatDongsViewModel.ImageFile.FileName);
                hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
                hoatDongsViewModel.HinhAnhBaiViet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanh;
                hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanh);
                hoatDongsViewModel.ImageFile.SaveAs(hinhanh);
                data.ID = hoatDongsViewModel.ID;
                data.Ten = hoatDongsViewModel.Ten;
                data.MoTa = hoatDongsViewModel.MoTa;
                data.HinhAnhBaiViet = hoatDongsViewModel.HinhAnhBaiViet;
                data.NoiDung = hoatDongsViewModel.NoiDung;
                data.KeyWord = hoatDongsViewModel.KeyWord;
                data.URL = hoatDongsViewModel.URL;
                data.NgayDang = hoatDongsViewModel.NgayDang;
                data.TenNguoiDang = hoatDongsViewModel.TenNguoiDang;

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hoatDongsViewModel.ImageFile != null && hoatDongsViewModel.ImageDetailFile != null)
            {
                string hinhanh = Path.GetFileNameWithoutExtension(hoatDongsViewModel.ImageFile.FileName);
                string hinhanhchitiet = Path.GetFileNameWithoutExtension(hoatDongsViewModel.ImageDetailFile.FileName);
                string imgExtension = Path.GetExtension(hoatDongsViewModel.ImageFile.FileName);
                string hinhanhchitietExtension = Path.GetExtension(hoatDongsViewModel.ImageDetailFile.FileName);
                hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
                hinhanhchitiet = hinhanhchitiet + DateTime.Now.ToString("yyyymmssfff") + hinhanhchitietExtension;
                hoatDongsViewModel.HinhAnhBaiViet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanh;
                hoatDongsViewModel.HinhAnhChiTiet = "/Areas/Admin/Resource/HinhAnh/TinTuc/" + hinhanhchitiet;
                hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanh);
                hinhanhchitiet = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/TinTuc/"), hinhanhchitiet);
                hoatDongsViewModel.ImageFile.SaveAs(hinhanh);
                hoatDongsViewModel.ImageDetailFile.SaveAs(hinhanhchitiet);

                if (ModelState.IsValid)
                {
                    data.ID = hoatDongsViewModel.ID;
                    data.Ten = hoatDongsViewModel.Ten;
                    data.MoTa = hoatDongsViewModel.MoTa;
                    data.NoiDung = hoatDongsViewModel.NoiDung;
                    data.KeyWord = hoatDongsViewModel.KeyWord;
                    data.URL = hoatDongsViewModel.URL;
                    data.HinhAnhBaiViet = hoatDongsViewModel.HinhAnhBaiViet;
                    data.HinhAnhChiTiet = hoatDongsViewModel.HinhAnhChiTiet;
                    data.NgayDang = hoatDongsViewModel.NgayDang;
                    data.TenNguoiDang = hoatDongsViewModel.TenNguoiDang;

                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(hoatDongsViewModel);
        }


        public ActionResult DeleteConfirmed(int id)
        {
            HoatDong hoatDong = db.HoatDong.Find(id);
            db.HoatDong.Remove(hoatDong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
