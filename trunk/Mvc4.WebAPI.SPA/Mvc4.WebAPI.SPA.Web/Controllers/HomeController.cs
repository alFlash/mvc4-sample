using System.Web.Mvc;

namespace Mvc4.WebAPI.SPA.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
