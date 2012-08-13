using System.Collections.Generic;
using System.Web.Mvc;
using MVC.Core.Attributes;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Web.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "Welcome, Guest";

            return View(People.GetLists());
        }

        [HttpPost]
        [ActionCommand(ButtonName= "Submit", ButtonValue= "Submit")] 
        public ActionResult Index(List<People> peoples)
        {
            ViewBag.Title = "Welcome, peoples";

            return View(peoples);
        }

        [HttpPost]
        [ActionCommand(ButtonName = "Cancel", ButtonValue = "Cancel")] 
        public ActionResult Cancel()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return View("Index", People.GetLists());
        }
    }
}
