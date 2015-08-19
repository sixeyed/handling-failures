using System.Web.Mvc;

namespace Sixeyed.HandlingFailures.Web.Controllers
{
    public class ConfirmationController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Version = "V1";
            return View();
        }

        public ActionResult V2()
        {
            ViewBag.Version = "V2";
            return View("Index");
        }

        public ActionResult V3()
        {
            ViewBag.Version = "V3";
            return View("Index");
        }
    }
}