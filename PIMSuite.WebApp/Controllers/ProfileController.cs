using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private DataContext _dataContext;

        public ProfileController()
        {
            _dataContext = new DataContext();;
        }

        // GET: Profile
        public ActionResult Index(string sort)
        {
            //TODO:: entities should be in english
            var users = from u in _dataContext.Users
                        select u;

            switch (sort)
            {
                case "desc":
                    users = users.OrderByDescending(u => u.Nachname);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    users = users.OrderBy(u => u.Nachname);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }
            
            return View(users.ToList());
        }
    }
}