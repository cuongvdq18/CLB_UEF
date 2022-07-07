using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using ClubPortalMS.Models;
using CustomAuthorizationFilter.Infrastructure;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class DBUserRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/DBUserRoles
        public ActionResult Index()
        {

            var dBUserRoles = db.DBUserRoles.Include(d => d.DBRoles).Include(d => d.DBUser);
            return View(dBUserRoles.ToList());
        }

        // GET: Admin/DBUserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBUserRoles dBUserRoles = db.DBUserRoles.Find(id);
            if (dBUserRoles == null)
            {
                return HttpNotFound();
            }
            return View(dBUserRoles);
        }

        // GET: Admin/DBUserRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.DBRoles, "ID", "Name");
            ViewBag.UserID = new SelectList(db.DBUser, "ID", "FirstName");
            return View();
        }

        // POST: Admin/DBUserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,RoleID")] DBUserRoles dBUserRoles)
        {
            if (ModelState.IsValid)
            {
                db.DBUserRoles.Add(dBUserRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.DBRoles, "ID", "Name", dBUserRoles.RoleID);
            ViewBag.UserID = new SelectList(db.DBUser, "ID", "FirstName", dBUserRoles.UserID);
            return View(dBUserRoles);
        }

        // GET: Admin/DBUserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBUserRoles dBUserRoles = db.DBUserRoles.Find(id);
            if (dBUserRoles == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.DBRoles, "ID", "Name", dBUserRoles.RoleID);
            ViewBag.UserID = new SelectList(db.DBUser, "ID", "FirstName", dBUserRoles.UserID);
            return View(dBUserRoles);
        }

        // POST: Admin/DBUserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,RoleID")] DBUserRoles dBUserRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dBUserRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.DBRoles, "ID", "Name", dBUserRoles.RoleID);
            ViewBag.UserID = new SelectList(db.DBUser, "ID", "FirstName", dBUserRoles.UserID);
            return View(dBUserRoles);
        }

        // GET: Admin/DBUserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBUserRoles dBUserRoles = db.DBUserRoles.Find(id);
            if (dBUserRoles == null)
            {
                return HttpNotFound();
            }
            return View(dBUserRoles);
        }

        // POST: Admin/DBUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DBUserRoles dBUserRoles = db.DBUserRoles.Find(id);
            db.DBUserRoles.Remove(dBUserRoles);
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
