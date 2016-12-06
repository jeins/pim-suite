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
        private IMessageRepository _messageRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                this.userRepository = new UserRepository(new DataContext());
                _messageRepository = new MessageRepository(new DataContext());
                var username = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
                var user = userRepository.GetUserByUsername(username);
                if (user == null)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                    Response.Redirect("/");
                }
                else
                {
                    var unReadMessages = _messageRepository.GetNotificationOfUnReadMessage(user.UserId);
                    ViewBag.FullName = user.Firstname + " " + user.Lastname;
                    ViewBag.User = user;
                    ViewBag.TotalUnReadMessage = unReadMessages.Count();
                    ViewBag.UnReadMessage = unReadMessages;
                }
                ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            }
        }
    }
}