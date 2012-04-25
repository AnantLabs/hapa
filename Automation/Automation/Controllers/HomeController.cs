using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Start your automation test with us.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About this framework and us.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact us if you need to.";

            return View();
        }
    }
}
