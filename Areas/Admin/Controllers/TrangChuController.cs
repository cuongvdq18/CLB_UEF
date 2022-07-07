using CustomAuthorizationFilter.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Admin.Controllers
{
    public class TrangChuController : Controller
    {
        [CustomAuthenticationFilter]
        [CustomAuthorize("Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}