using FleetManagement.DataAccess.DbContext;
using FleetManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Index()
        {
            var cars = _repository.GetAll().ToList();
            return View();
        }

    }
}
