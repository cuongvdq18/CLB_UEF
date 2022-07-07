
using ClubPortalMS.Areas.Customer.DAO;
using System.Web.Mvc;
using ClubPortalMS.Models;

namespace ClubPortalMS.Areas.Customer.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Customer/Albums
        public ActionResult Index()
        {
            var ListDAO = new ListDAO();
            ViewBag.ListAllAlbums = ListDAO.ListAllAlbums();
            return View();
        }
    }
}