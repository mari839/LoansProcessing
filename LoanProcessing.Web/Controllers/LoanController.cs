using System.Web.Mvc;

namespace LoanProcessing.Web.Controllers
{
    public class LoanController : Controller
    {
        public ActionResult Applications()
        {
            return View();
        }
    }
}
