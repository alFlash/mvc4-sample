using System.Collections.Generic;
using System.Web.Mvc;

namespace Mvc4.WebAPI.SPA.Web.Controllers
{
    public class HomeController : Controller
    {
        public List<FakeData.FakeData> Index()
        {
            return FakeData.FakeData.GetList();
        }
    }
}
