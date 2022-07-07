using ClubPortalMS.Areas.Customer.DAO;
using System.Web.Mvc;
using ClubPortalMS.Models;
using System.Linq;
using System.Collections.Generic;

namespace ClubPortalMS.Areas.Customer.Controllers
{
    public class TrangChuController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Customer/Home
        public ActionResult Index()
        {
            List<TinTuc> tt =  db.TinTucs.OrderByDescending(x => x.ID).ToList();
            List<HoatDong> hd =  db.HoatDong.OrderByDescending(x => x.ID).ToList();
            tt.RemoveAt(0);
            hd.RemoveAt(0);
            var ListDAO = new ListDAO();
            var listNews = tt;
            var listHD= hd;
            ViewBag.ListNewsFirst = ListDAO.ListNews(1);
            ViewBag.ListNews = listNews.Take(3).ToList();
            ViewBag.ListHDFirst = ListDAO.listHD(1);
            ViewBag.ListHD = listHD.Take(3).ToList();
            ViewBag.Slides = new SlideDao().ListAll();
            ViewBag.ListAlbums = new ListDAO().ListAlbums(9);
            return View();
        } 
    }
}