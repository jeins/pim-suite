using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using PIMSuite.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class AdministrationController : BaseController
    {
        public IDepartmentRepository departmentRepository;
        public ILocationRepository locationRepository;
        public IDomainRepository domainRepository;

        public AdministrationController()
        {
            departmentRepository = new DepartmentRepository(new DataContext());
            locationRepository = new LocationRepository(new DataContext());
            domainRepository = new DomainRepository(new DataContext());
        }

        // GET: Administration
        public ActionResult Index()
        {
            AdministrationModel am = new AdministrationModel();
            am.Users = userRepository.GetUsers();
            am.Departments = departmentRepository.GetDepartments();
            am.Locations = locationRepository.GetLocations();
            am.Domains = domainRepository.GetDomains();
            return View(am);
        }
    }
}