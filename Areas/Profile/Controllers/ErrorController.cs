using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClubPortalMS.Areas.Profile.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Profile/Error
        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}