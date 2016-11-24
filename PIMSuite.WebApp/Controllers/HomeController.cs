﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Utilities.Auth;

namespace PIMSuite.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Testdaten
            IDepartmentRepository departmentRepository = new DepartmentRepository(new DataContext());
            ILocationRepository locationRepository = new LocationRepository(new DataContext());
            IUserRepository userRepository = new UserRepository(new DataContext());

            if (departmentRepository.GetDepartments().Count() == 0)
            {
                departmentRepository.InsertDepartment(new Department { Name = "IT-Support" });
                departmentRepository.InsertDepartment(new Department { Name = "Sekretariat" });
            }
            departmentRepository.Save();
            if (locationRepository.GetLocations().Count() == 0)
            {
                locationRepository.InsertLocation(new Location { Name = "Berlin" });
                locationRepository.InsertLocation(new Location { Name = "Hamburg" });
            }
            locationRepository.Save();

            if (userRepository.GetUsers().Count() == 0)
            {
                User user = new Persistence.Entities.User
                {
                    Firstname = "Max",
                    Lastname = "Mustermann",
                    Username = "maxm",
                    Email = "max@mail.org",
                    PhoneNumber = "12345678",
                    DepartmentName = "IT-Support",
                    LocationName = "Berlin",
                    Password = new HashHelper().Hash("mustermann"),
                    isAdmin = true

                };
                userRepository.InsertUser(user);
                userRepository.Save();
            }

            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Dashboard/");
            }

            return View();
        }
    }
}