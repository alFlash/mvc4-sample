using System.Collections.Generic;
using System.Web.Mvc;
using MVC4.Sample.Common.Entities;
using MVC4.Sample.Common.ViewModels.Knockoutjs;
using MVC4.Sample.Web.ViewModels;

namespace MVC4.Sample.Web.Controllers.Knockout
{
    public class KnockoutjsController : Controller
    {
        public ActionResult Index(string viewName)
        {
            var viewModel = new Vehicle
                                {
                                    Cars = new List<Car>
                                               {
                                                   new Car
                                                       {
                                                           Name = "Truck",
                                                           Wheels = new List<Wheel>
                                                                        {
                                                                            new Wheel {WheelName = "TopLeft"},
                                                                            new Wheel {WheelName = "TopRight"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                        }
                                                       },
                                                   new Car
                                                       {
                                                           Name = "Taxi",
                                                           Wheels = new List<Wheel>
                                                                        {
                                                                            new Wheel {WheelName = "TopLeft"},
                                                                            new Wheel {WheelName = "TopRight"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                        }
                                                       }
                                               }
                                };
            return !string.IsNullOrWhiteSpace(viewName) ? View(viewName, viewModel) : View(viewModel);
        }

        public ActionResult GetVehicle(int id)
        {
            var viewModel = new Vehicle
                                {
                                    Cars = new List<Car>
                                               {
                                                   new Car
                                                       {
                                                           Name = string.Format("Truck{0}", id),
                                                           Wheels = new List<Wheel>
                                                                        {
                                                                            new Wheel {WheelName = "TopLeft"},
                                                                            new Wheel {WheelName = "TopRight"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                        }
                                                       },
                                                   new Car
                                                       {
                                                           Name = string.Format("Taxi{0}", id),
                                                           Wheels = new List<Wheel>
                                                                        {
                                                                            new Wheel {WheelName = "TopLeft"},
                                                                            new Wheel {WheelName = "TopRight"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                            new Wheel {WheelName = "BottomLeft"},
                                                                        }
                                                       }
                                               }
                                };
            return Json(viewModel);
        }

        public ActionResult RecursiveFolder()
        {
            var viewModel = new RecursiveFolderViewModel
                {
                    Template = new RecursiveFolder(),
                    Folders = new List<RecursiveFolder>()
                };
            for (var i = 0; i < 10; i++)
            {
                viewModel.Folders.Add(new RecursiveFolder
                    {
                        Id = i,
                        Name = string.Format("Folder {0}", i),
                        ParentId = null
                    });
            }
            return View(viewModel);
        }

    }
}
