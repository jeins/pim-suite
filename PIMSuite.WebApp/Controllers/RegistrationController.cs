using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Persistence.Validators;
using PIMSuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PIMSuite.Utilities.Auth;
using System.Net;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class RegistrationController : BaseController
    {
        public ILocationRepository locationRepository;
        public IDepartmentRepository departmentRepository;

        public RegistrationController()
        {
            this.userRepository = new UserRepository(new DataContext());
            ViewBag.Departments = new SelectList(new DataContext().Departments, "Name", "Name");
            ViewBag.Locations = new SelectList(new DataContext().Locations, "Name", "Name");
        }



        // GET: Registration
        [AuthorizationFilter]
        public ActionResult Index()
        {
            //Überblick zu den Benutzer-Kontos
            //Später kann gelöscht werden oder für die Adminkonsole genutzt
            
            return View(userRepository.GetUsers()); 
        }

        public ActionResult Registration()
        {
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Dashboard/");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            var userval = new UserValidator();
            if (ModelState.IsValid && userval.Validate(user).IsValid)
            {
                var hh = new HashHelper();
                //TODO: Bis auf Weiteres zum besseren Testen immer aktiv
                user.isAdmin = true;
                user.Password = hh.Hash(user.Password);

                userRepository.InsertUser(user);
                userRepository.Save();
                ModelState.Clear();
                ViewBag.Message = user.Firstname + " " + user.Lastname + " " + "wurde erfolgreich registriert!";
                Response.Redirect("/");
            }
            else
            {
                string errors = string.Join("\n", userval.Validate(user).Errors);
                ViewBag.Message = errors;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Validation(String token)
        {
            User user;
            using (DataContext context = new DataContext())
            {
                user = context.Users.SingleOrDefault(p => p.ValidationToken == token); 

                if (user == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Wrong validation token!");
                }

            }
            return View();
        }
    }
}