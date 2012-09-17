using System;
using Spring.Context.Support;
using Spring.Net.Sample.ConsoleApp.MyCodes;

namespace Spring.Net.Sample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = ContextRegistry.GetContext())
            {
                var myCar = (IVehicle) ctx.GetObject("TheCar");
                myCar.Run();
                Console.ReadKey();
            }
        }
    }
}
