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
using System.Text;
using System.Security.Cryptography;

namespace PIMSuite.WebApp.Controllers
{
    public class RegistrationController : BaseController
    {
        public ILocationRepository locationRepository;
        public IDepartmentRepository departmentRepository;
        public const int validationTokenSize = 24;

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
                user.ValidationToken = GenerateValidationToken(validationTokenSize);
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
                else
                {
                    user.ValidationToken = null;
                    user.isValidated = true;
                    userRepository.UpdateUser(user);
                    userRepository.Save();
                    ModelState.Clear();
                    ViewBag.Message = user.Firstname + " " + user.Lastname + " " + "wurde erfolgreich validiert!";
                    Response.Redirect("/");
                }

            }
            return View();
        }

        private String GenerateValidationToken(int size) //TODO: Methode auslagern
        {
            {
                char[] chars = new char[62];
                chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[1];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    data = new byte[size];
                    crypto.GetNonZeroBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length)]);
                }
                return result.ToString();
            }
        }

    }
}