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
using ClubPortalMS.ViewModel.Poster;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class PostersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Posters
        public ActionResult Index()
        {
            List<Poster> poster = db.Poster.ToList();
            var ds = from e in poster
                          select new PosterViewModel
                          {
                              ID = e.ID,
                              TenPoster = e.TenPoster,
                              HinhAnh = e.HinhAnh,
                              Status = e.Status,
                              
                          };

            return View(ds);
        }

        // GET: Admin/Posters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Poster.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PosterViewModel
            {
                ID = data.ID,
                TenPoster = data.TenPoster,
                HinhAnh = data.HinhAnh,
                Status = data.Status

            };
            return View(viewModel);
        }

        // GET: Admin/Posters/Create
        public ActionResult Create()
        {
            var createPoster = new PosterViewModel();
            return View(createPoster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PosterViewModel posterView,int? id)
        {
            if (posterView.ImageFile == null)
            {
                ViewBag.Message = "Chưa có hình ảnh nào được chọn!!";
                return View(posterView);
            }
            string hinhanh = Path.GetFileNameWithoutExtension(posterView.ImageFile.FileName);
            string imgExtension = Path.GetExtension(posterView.ImageFile.FileName);
            hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
            posterView.HinhAnh = "/Areas/Admin/Resource/HinhAnh/Poster/" + hinhanh;
            hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/Poster/"), hinhanh);

            posterView.ImageFile.SaveAs(hinhanh);
            
            if (ModelState.IsValid)
            {
                Poster poster = new Poster();
                poster.ID = posterView.ID;
                poster.TenPoster = posterView.TenPoster;
                poster.HinhAnh = posterView.HinhAnh;
                poster.Status = posterView.Status;
                db.Poster.Add(poster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(posterView);
        }

        // GET: Admin/Posters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Poster.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PosterViewModel
            {
                ID = data.ID,
                TenPoster = data.TenPoster,
                HinhAnh = data.HinhAnh,
                Status = data.Status

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PosterViewModel posterView, int? id)
        {
         
            if (ModelState.IsValid)
            {
                var data = db.Poster.SingleOrDefault(n => n.ID == id);
                if (data == null)
                {
                    return HttpNotFound();
                }
                if(posterView.ImageFile == null) 
                {
                    data.ID = posterView.ID;
                    data.TenPoster = posterView.TenPoster;
                    data.Status = posterView.Status;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                string hinhanh = Path.GetFileNameWithoutExtension(posterView.ImageFile.FileName);
                string imgExtension = Path.GetExtension(posterView.ImageFile.FileName);
                hinhanh = hinhanh + DateTime.Now.ToString("yyyymmssfff") + imgExtension;
                posterView.HinhAnh = "/Areas/Admin/Resource/HinhAnh/Poster/" + hinhanh;
                hinhanh = Path.Combine(Server.MapPath("~/Areas/Admin/Resource/HinhAnh/Poster/"), hinhanh);
                posterView.ImageFile.SaveAs(hinhanh);
                data.ID = posterView.ID;
                data.TenPoster = posterView.TenPoster;
                data.HinhAnh = posterView.HinhAnh;
                data.Status = posterView.Status;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posterView);
        }


        public ActionResult DeleteConfirmed(PosterViewModel posterView)
        {
            Poster poster = db.Poster.Find(posterView.ID);         
            db.Poster.Remove(poster);
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
