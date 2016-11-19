using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class LocationController : BaseController
    {
        private LocationRepository locationRepository;

        public LocationController()
        {
            this.locationRepository = new LocationRepository(new DataContext());
        }

        // GET: Location
        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View(locationRepository.GetLocations());
        }
    }
}