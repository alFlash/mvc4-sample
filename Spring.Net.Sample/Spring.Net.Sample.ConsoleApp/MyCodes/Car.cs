namespace Spring.Net.Sample.ConsoleApp.MyCodes
{
    public class Car : IVehicle
    {
        private readonly IWheel _wheel;
        private readonly IVehicleDriver _driver;

        public Car(IWheel wheel, IVehicleDriver driver)
        {
            _wheel = wheel;
            _driver = driver;
        }
        public void Run()
        {
            _wheel.Run();
            _driver.Drive();
        }
    }

}
