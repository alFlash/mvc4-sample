using System.Collections.Generic;

namespace MVC4.Sample.Web.ViewModels
{
    public class Vehicle
    {
        public List<Car> Cars { get; set; }
        public List<Bycicle> Bycicles { get; set; } 
    }

    public class Bycicle
    {
        public string Name { get; set; }
        public List<Wheel> Wheels { get; set; }
    }

    public class Wheel
    {
        public string WheelName { get; set; }
    }

    public class Car
    {
        public string Name { get; set; }
        public List<Wheel> Wheels { get; set; }
    }
}