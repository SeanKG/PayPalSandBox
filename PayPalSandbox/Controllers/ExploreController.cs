using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayPalSandbox.Controllers
{
    public class ExploreController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Explore";

            return View();
        }
    }
}
