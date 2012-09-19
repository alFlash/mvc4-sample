using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4.Sample.Web.ViewModels;

namespace MVC4.Sample.Web.Controllers.Knockout
{
    public class KnockoutjsController : Controller
    {
        public ActionResult Index()
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
            return View(viewModel);
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

    }
}
