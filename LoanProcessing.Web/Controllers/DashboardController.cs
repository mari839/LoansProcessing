using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanProcessing.Web.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View("Dashboard"); 
        }

        public ActionResult Empty()
        {
            return View(); 
        }
    }

}