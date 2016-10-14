using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionesGM_MVC.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }
        
        [Authorize(Roles = "Gestor")]
        public ActionResult About()
        {
            return View();
        }
    }
}
