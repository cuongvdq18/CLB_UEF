using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.User;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class DBUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/DBUsers
        public ActionResult Index()
        {
            List<DBUser> users = db.DBUser.ToList();
            var ds = from e in users
                          select new UserViewModel
                          {
                              ID = e.ID,
                              FirstName = e.FirstName,
                              LastName = e.LastName,
                              Username = e.Username,
                              Email = e.Email,
                              Identifier = e.Identifier,
                              EmailConfirmation = e.EmailConfirmation,
                              HashedPassword = e.HashedPassword,
                              Salt = e.Salt,
                              IsLocked = e.IsLocked,
                              DateCreated = e.DateCreated,
                              IsDeleted = e.IsDeleted,
                              ActivationCode = e.ActivationCode,
                              NgayXoa = e.NgayXoa,
                              UserDeleted = e.UserDeleted
                              
                          };

            return View(ds);
        }

        // GET: Admin/DBUsers/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBUser.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new UserViewModel
            {
                ID = data.ID,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Username = data.Username,
                Email = data.Email,
                Identifier = data.Identifier,
                EmailConfirmation = data.EmailConfirmation,
                HashedPassword = data.HashedPassword,
                Salt = data.Salt,
                IsLocked = data.IsLocked,
                DateCreated = data.DateCreated,
                IsDeleted = data.IsDeleted,
                ActivationCode = data.ActivationCode,
                NgayXoa = data.NgayXoa,
                UserDeleted = data.UserDeleted

            };
            return View(viewModel);
        }

        public ActionResult KhoaTK(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBUser.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                data.IsLocked = true;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id});
            }
            return View(data);
        }
        public ActionResult MoTK(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBUser.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                data.IsLocked = false;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            return View(data);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            DBUser data = db.DBUser.Find(id);
            ThanhVien thanhvien = db.ThanhVien.Find(id);
            db.ThanhVien.Remove(thanhvien);
            db.DBUser.Remove(data);
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
