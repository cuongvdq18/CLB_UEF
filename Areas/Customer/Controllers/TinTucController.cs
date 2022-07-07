using System.Data.Entity;
using ClubPortalMS.Areas.Customer.DAO;
using System.Web.Mvc;
using ClubPortalMS.Models;
using System.Net;
using System.Linq;

namespace ClubPortalMS.Areas.Customer.Controllers
{
    public class TinTucController : Controller
    {
        // GET: Customer/TinTuc
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            var ListDAO = new ListDAO();
            ViewBag.ListAllNews = ListDAO.ListAllNew(page);
            return View(db.TinTucs.ToList());
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
            TinTuc tinTuc = db.TinTucs.Find(id);
            tinTuc.LuotView = tinTuc.LuotView + 1;
            db.Entry(tinTuc).State = EntityState.Modified;
            db.SaveChanges();
            if (tinTuc == null)
            {
                return HttpNotFound();
            }
            return View(tinTuc);
        }

    }
}