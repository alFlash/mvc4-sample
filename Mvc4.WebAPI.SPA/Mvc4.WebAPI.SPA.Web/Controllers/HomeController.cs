using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc4.WebAPI.SPA.Web.Controllers
{
    public class HomeController : Controller
    {
        public IQueryable<FakeData.FakeData> Index()
        {
            return FakeData.FakeData.GetList().AsQueryable();
        }
    }
}
