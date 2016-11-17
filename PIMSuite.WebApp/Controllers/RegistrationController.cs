using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        public IUserRepository userRepository;
        public ILocationRepository locationRepository;
        public IDepartmentRepository departmentRepository;

        public RegistrationController()
        {
            this.userRepository = new UserRepository(new DataContext());
            //this.locationRepository = new LocationRepository(new DataContext());
            //this.departmentRepository = new DepartmentRepository(new DataContext());
        }



        // GET: Registration
        public ActionResult Index()
        {

            //Überblick zu den Benutzer-Kontos
            //Später kann gelöscht werden oder für die Adminkonsole genutzt
            
            return View(userRepository.GetUsers()); 
        }

        public ActionResult Registration()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    userRepository.InsertUser(user);
                    userRepository.Save();
                    ModelState.Clear();
                    ViewBag.Message = user.Firstname + " " + user.Lastname + " " + "wurde erfolgreich registriert!";
                }
            }
            return View();
        }
    }
}