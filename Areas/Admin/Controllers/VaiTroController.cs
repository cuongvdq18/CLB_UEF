using ClubPortalMS.Models;
using ClubPortalMS.ViewModel.User;
using CustomAuthorizationFilter.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class VaiTroController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var ds = from e in db.DBRoles
                         select new RoleViewModel
                         {
                             ID = e.ID,
                             Name = e.Name,
                             MoTa = e.MoTa
                         };

            return View(ds);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBRoles.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new RoleViewModel
            {
                ID = data.ID,
                Name = data.Name,
                MoTa = data.MoTa

            };
            return View(viewModel);
        }


        public ActionResult Create()
        {
            var createRole = new RoleViewModel();
            return View(createRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                DBRoles role = new DBRoles();
                role.ID = roleView.ID;
                role.Name = roleView.Name;
                role.MoTa = roleView.MoTa;
                db.DBRoles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleView);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBRoles.SingleOrDefault(n => n.ID == id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var viewModel = new RoleViewModel
            {
                ID = data.ID,
                Name = data.Name,
                MoTa = data.MoTa

            };
            return View(viewModel);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleViewModel roleView, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DBRoles.SingleOrDefault(n => n.ID == id);

            if (data == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                data.ID = roleView.ID;
                data.Name = roleView.Name;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleView);
        }

        public ActionResult DeleteConfirmed(RoleViewModel role)
        {

            DBRoles roles = db.DBRoles.Find(role.ID);
            db.DBRoles.Remove(roles);
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