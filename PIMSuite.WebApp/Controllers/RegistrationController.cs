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
using PIMSuite.Utilities.Mail;

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
                var hashhelper = new HashHelper();                
                //TODO: Bis auf Weiteres zum besseren Testen immer aktiv
                user.IsAdmin = true;
                user.Password = hashhelper.Hash(user.Password);
                user.ValidationToken = TokenGenerator.GenerateValidationToken(validationTokenSize);
                userRepository.InsertUser(user);
                userRepository.Save();
                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " " + "wurde erfolgreich registriert!";                
                EmailHelper.SendMail("smtp.gmail.com", "PIMSuiteASP@gmail.com", "noreplyASP", user.Email, "Willkommen bei Pim-Suite!", "Herzlich Willkiomen in der PIM-Suite!\nBitte klicken Sie auf den Link um Ihre Registrierung abzuschließen: " + Request.Url.GetLeftPart(UriPartial.Authority) + "/Registration/Validation?token=" + user.ValidationToken + " \nSollte der Link nicht funktionieren, kopieren Sie Ihn in die Adresszeile Ihres Browsers.\n\nWir wünschen viel Spaß mit Ihrer PIM-Suite");
                Response.Redirect("/?infoMessage=A validation link has been sent to your mail-address, please check your mails and click the link to validate!");
            }
            else
            {
                string errors = string.Join("\n", userval.Validate(user).Errors);
                ViewBag.Message = errors;
            }
            return View();
        }

        [HttpGet]
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
                    user.IsValidated = true;
                    //userRepository.UpdateUser(user);
                    userRepository.ValidateUser(user, true);
                    userRepository.Save();
                    ModelState.Clear();
                    ViewBag.Message = user.FirstName + " " + user.LastName + " " + "wurde erfolgreich validiert!";
                    Response.Redirect("/?successMessage=" + user.Email + " has been successfully validated! You can now login, congratulations!");
                }
            }
            return View();
        }
    }
}