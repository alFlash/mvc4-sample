using System.Web.Mvc;
using FluentHibernate.DAO.Models;
using FluentHibernate.DAO.Repositories;

namespace FluentHibernate.Sample.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.GetAll<People>());
        }

    }
}
