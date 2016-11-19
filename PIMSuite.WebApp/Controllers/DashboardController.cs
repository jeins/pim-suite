using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
