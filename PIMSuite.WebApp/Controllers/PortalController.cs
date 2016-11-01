using PIMSuite.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    [Authorize]
    public class PortalController : Controller
    {
        // Constructors

        public PortalController()
        {
            _dataContext = new DataContext();
        }

        // Fields

        private readonly DataContext _dataContext;

        // Methods

        public ActionResult Index()
        {
            ViewBag.UserCount = _dataContext.Users.Count();
            return View();
        }
    }
}