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
    public class DBRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/DBRoles
        public ActionResult Index()
        {
            List<DBRoles> albums = db.DBRoles.ToList();
            var dsRole = from e in albums
                          select new RoleViewModel
                          {
                              ID = e.ID,
                              Name = e.Name,
                              MoTa = e.MoTa
                          };
            return View(dsRole);
        }

        // GET: Admin/DBRoles/Details/5
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

        // GET: Admin/DBRoles/Create
        public ActionResult Create()
        {
            var createRole = new RoleViewModel();
            return View(createRole);
        }

        // POST: Admin/DBRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleViewModel roleView,int? id)
        {
            if (ModelState.IsValid)
            {
                DBRoles dBRoles = new DBRoles();
                dBRoles.ID = roleView.ID;
                dBRoles.Name = roleView.Name;
                dBRoles.MoTa = roleView.MoTa;
                db.DBRoles.Add(dBRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleView);
        }

        // GET: Admin/DBRoles/Edit/5
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

        // POST: Admin/DBRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleViewModel roleView,int? id)
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
                data.MoTa = roleView.MoTa;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleView);
        }

        // GET: Admin/DBRoles/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/DBRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RoleViewModel roleView,int id)
        {
            DBRoles data = db.DBRoles.Find(id);
            data.ID = roleView.ID;
            data.Name = roleView.Name;
            data.MoTa = roleView.MoTa;
            db.DBRoles.Remove(data);
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
