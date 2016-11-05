using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
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
            return View();
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
                using (DataContext context = new DataContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                ModelState.Clear();
            }
            return View();
        }
    }
}