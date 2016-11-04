using PIMSuite.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            using (PIMDBEntities db = new PIMDBEntities())
            {
                return View(db.User.ToList());
            }
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
                using (PIMDBEntities db= new PIMDBEntities())
                {
                    db.User.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.Vorname+" "+user.Nachname+ " "+ "wurde erfolgreich registriert!";
            }
            return View();
        }
    }
}