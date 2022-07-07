
using ClubPortalMS.Areas.Customer.DAO;
using System.Web.Mvc;
using ClubPortalMS.Models;
using System.Net;
using System.Linq;
using System.Data.Entity;

namespace ClubPortalMS.Areas.Customer.Controllers
{
    public class HoatDongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customer/HoatDongs
        public ActionResult Index(int? page)
        {
            var ListDAO = new ListDAO();
            ViewBag.ListAllHD = ListDAO.ListAllHDs(page);
            return View(db.HoatDong.ToList());
        }
        public ActionResult BaiViet(int? id)
        {
            var ListDAO = new ListDAO();
            ViewBag.ListNews = ListDAO.ListNews(3);
            ViewBag.ListHD = ListDAO.listHD(3);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDong hoatDong = db.HoatDong.Find(id);
            hoatDong.LuotView = hoatDong.LuotView + 1;
            db.Entry(hoatDong).State = EntityState.Modified;
            db.SaveChanges();
            if (hoatDong == null)
            {
                return HttpNotFound();
            }
            return View(hoatDong);
        }
    }
}
