using System;

namespace Spring.Net.Sample.ConsoleApp.MyCodes
{
    public class TaxiDriver: IVehicleDriver
    {
        public void Drive()
        {
            Console.WriteLine("I'm a taxi driver...");
        }
    }
}
