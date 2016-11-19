using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PIMSuite.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public IUserRepository userRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            try
            {
                this.userRepository = new UserRepository(new DataContext());
                var username = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
                var user = userRepository.GetUserByUsername(username);
                ViewBag.FullName = user.Firstname + " " + user.Lastname;
                ViewBag.User = user;
            }
            catch (Exception e) { }
        }

    }
}