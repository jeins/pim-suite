using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        public IUserRepository userRepository;

        public DashboardController()
        {
            this.userRepository = new UserRepository(new DataContext());
        }

        // GET: Dashboard
        [AuthorizationFilter]
        public ActionResult Index()
        {
            var username = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
            var user = userRepository.GetUserByUsername(username);
            ViewBag.FullName = user.Firstname + " " + user.Lastname;
            return View();
        }

        
    }
}
