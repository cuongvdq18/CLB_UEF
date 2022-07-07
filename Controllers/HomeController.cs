using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClubPortalMS.Models;


namespace ClubPortalMS.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "TrangChu", new { area = "Customer" });
        }
   
    }
}