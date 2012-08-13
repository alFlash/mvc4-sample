using System.Collections.Generic;
using System.Web.Mvc;
using MVC.Core.Attributes;
using MVC4.Sample.Common.Entities;
using MVC4.Sample.Common.ViewModels;

namespace MVC4.Sample.Web.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "Welcome, Guest";
            var model = new HomeViewModel
                            {
                                Student = "Hoang Gia",
                                Employee = "Duy Truong",
                                Peoples = People.GetLists()
                            };
            return View(model);
        }

        public PartialViewResult Student(HomeViewModel viewModel)
        {
            return PartialView(viewModel);
        }

        [HttpPost]
        [ActionCommand(ButtonName= "Submit", ButtonValue= "Submit")] 
        public ActionResult Index(HomeViewModel viewModel)
        {
            ViewBag.Title = "Welcome, peoples";

            return View(viewModel);
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
            var model = new HomeViewModel
            {
                Student = "Hoang Gia",
                Employee = "Duy Truong",
                Peoples = People.GetLists()
            };
            return View("Index", model);
        }
    }
}
