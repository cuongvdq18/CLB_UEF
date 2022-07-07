using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using System.IO;
using ClubPortalMS.ViewModel.Album;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class AlbumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<Album> albums = db.Album.ToList();
            var dsAlbum = from e in albums
                          select new AlbumViewModel{ 
                             ID = e.ID,
                             TieuDe = e.TieuDe,
                             HinhAnh = e.HinhAnh,
                             Video = e.Video,
                             MoTa = e.MoTa
                          };

            return View(dsAlbum);
        }

        // GET: Admin/Albums/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Album.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new AlbumViewModel
            {
                ID = data.ID,
                TieuDe = data.TieuDe,
                HinhAnh = data.HinhAnh,
                MoTa = data.MoTa

            };
            return View(viewModel);
        }

        public ActionResult Create()
        {
            var create = new AlbumViewModel();
            return View(create);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlbumViewModel albumView)
        {
            if(albumView.ImageFile == null)
            {
                ViewBag.Message = "Bạn chưa tải ảnh nào lên";
                return View(albumView);
            }
            string hinhanh = Path.GetFileNameWithoutExtension(albumView.ImageFile.FileName);
            string imgExtension = Path.GetExtension(albumView.ImageFile.FileName);
            hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
            albumView.HinhAnh = "/Areas/Admin/Resource/HinhAnh/Albums/" + hinhanh;
            hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/Albums/"), hinhanh);
            albumView.ImageFile.SaveAs(hinhanh);
            if (ModelState.IsValid)
            {
                Album album = new Album();
                album.ID = albumView.ID;
                album.TieuDe = albumView.TieuDe;
                album.HinhAnh = albumView.HinhAnh;
                album.MoTa = albumView.MoTa;
                db.Album.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(albumView);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Album.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new AlbumViewModel
            {
                ID = data.ID,
                TieuDe = data.TieuDe,
                HinhAnh = data.HinhAnh,
                Video = data.Video,
                MoTa = data.MoTa

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlbumViewModel albumView,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Album.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            if (albumView.ImageFile == null)
            {
                if (ModelState.IsValid)
                {
                    data.ID = albumView.ID;
                    data.TieuDe = albumView.TieuDe;
                    data.MoTa = albumView.MoTa;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            string hinhanh = Path.GetFileNameWithoutExtension(albumView.ImageFile.FileName);

            string imgExtension = Path.GetExtension(albumView.ImageFile.FileName);

            hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;

            albumView.HinhAnh = "/Areas/Admin/Resource/HinhAnh/Albums/" + hinhanh;
            hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/Albums/"), hinhanh);
            albumView.ImageFile.SaveAs(hinhanh);
            
            if (ModelState.IsValid)
            {
                data.ID = albumView.ID;
                data.TieuDe = albumView.TieuDe;
                data.HinhAnh = albumView.HinhAnh;
                data.Video = albumView.Video;
                data.MoTa = albumView.MoTa;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(albumView);
        }
       
        public ActionResult DeleteConfirmed(AlbumViewModel albumView)
        {
            Album data = db.Album.Find(albumView.ID); 
            db.Album.Remove(data);
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
