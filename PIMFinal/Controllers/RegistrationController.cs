using DataPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMFinal.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            DataContext context = new DataContext();
            
          return View(context.Users.ToList());
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
                DataContext context = new DataContext();
                context.Users.Add(user);
                context.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.Message = user.Vorname + " " + user.Nachname + "wurde erfolgreich registriert";
            return View();
        }
    }
}