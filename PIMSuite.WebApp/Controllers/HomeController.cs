using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*var dc = new DataContext();
            dc.Users.Add(new User()
            {
                Abteilung = "...",
                Email = "asd@asd.de",
                Nachname = "asd",
                Passwort = "sdsd",
                Username = "dff",
                Vorname = "dfdf"
            });*/

            return View();
        }
    }
}