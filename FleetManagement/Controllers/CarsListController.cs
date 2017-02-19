using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManagement.Controllers
{
    public class CarsListController : Controller
    {
        //
        // GET: /CarsList/

        public ActionResult Index()
        {
            return View();
        }

    }
}
