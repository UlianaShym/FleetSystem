using System.Collections.Generic;
using FleetManagement.DataAccess.Entities;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using FleetManagement.DataAccess.Repositories;

namespace FleetManagement.Controllers
{
    public class CarsListController : Controller
    {
        //
        // GET: /CarsList/
        private IRepository<Car> _repository = null;
        public CarsListController()
        {
            this._repository = new Repository<Car>();
        }

        public ActionResult Index(int? page)
        {
            var cars = _repository.GetAll().ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(cars.ToPagedList(pageNumber, pageSize));
        }

    }
}
