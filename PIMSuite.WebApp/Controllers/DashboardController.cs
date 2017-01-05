using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}
