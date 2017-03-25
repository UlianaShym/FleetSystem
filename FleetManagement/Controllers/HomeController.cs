using System.Linq;
using System.Web.Mvc;
using FleetManagement.DataAccess.Entities;
using FleetManagement.DataAccess.Repositories;

namespace FleetManagement.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Car> _repository = null;
        public HomeController()
        {
            this._repository = new Repository<Car>();
        }

        void TestData()
        {
            var car1 = new Car
            {
                Model = "X-Class Pickup",
                Desription = "The truck Mercedes is attempting to tap into a global interest in trucks as lifestyle vehicles. The five-passenger, mid-size X-Class also is entering a fast-growing segment of the pickup truck market.",
                ProduceYear = 2013,
                ImageUrl = "Truck_1_200px.png"
            };
            var car2 = new Car
            {
                Model = "X-Class Pickup",
                Desription = "Long relegated to the back of the pack, the Titan has new duds that finally give it a fighting chance. A 5.6-liter V-8 makes 394 lb-ft of torque and mates to a seven-speed automatic and rear- or four-wheel drive.",
                ProduceYear = 2013,
                ImageUrl = "Truck_2_200px.png"
            };
            var car3 = new Car
            {
                Model = "Nissan",
                Desription = "Ford sent Fiesta a stock V10 engine mated to a heavy-duty automatic transmission, Tim Estes, president of Fiesta Parade Floats, told Trucks.com. This engine makes 320 horsepower and 460 pound-feet of torque.",
                ProduceYear = 2013,
                ImageUrl = "Truck_3_200px.png"
            };
            var car4 = new Car
            {
                Model = "Nissan Titan",
                Desription = "The legendary F-150—with an aluminum bed and body—earns a 2017 10Best award. The base 3.5-liter V-6 (253 lb-ft), optional 2.7-liter turbo V-6 (375 lb-ft), and optional 5.0-liter V-8 (387 lb-ft) all pair with six-speed automatics.",
                ProduceYear = 2013,
                ImageUrl = "Truck_4_200px.png"
            };
            _repository.AddNew(car1);
            _repository.AddNew(car2);
            _repository.AddNew(car3);
            _repository.AddNew(car4);
        }
        public ActionResult Index()
        {
            //TestData(); 
            var cars = _repository.GetAll().Take(4).ToList();

            return View(cars);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult Cars()
        //{
        //    ViewBag.Message = "Your Cars page.";

        //    return View();
        //}
        public ActionResult Car()
        {

            return View();
        }
    }
}
