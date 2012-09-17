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

    }
}
