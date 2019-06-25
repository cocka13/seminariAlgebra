using System.Web.Mvc;

namespace Aplikacija.Controllers
{
    public class GlobalErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}