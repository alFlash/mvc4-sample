using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4.Sample.Common.ViewModels;

namespace MVC4.Sample.Web.Controllers.Partials
{
    public class ParentViewController : Controller
    {
        //
        // GET: /ParentView/

        public ActionResult Index()
        {
            var viewModel = new ParentViewModel
                                {
                                    ParentView = "This is the ParentView",
                                    PartialView = new PartialViewViewModel
                                                      {
                                                          PartialView = "This is the PartialView"
                                                      }
                                };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ParentViewModel viewModel)
        {
            return View(viewModel);
        }

    }
}
