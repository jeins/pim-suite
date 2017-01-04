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
        // Fields

        public IUserRepository userRepository;
        private IMessageRepository _messageRepository;
        private INotificationRepository _notificationRepository;

        // Methods

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                userRepository = new UserRepository(new DataContext());
                _messageRepository = new MessageRepository(new DataContext());
                _notificationRepository = new NotificationRepository(new DataContext());
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
                    ViewBag.FullName = user.FirstName + " " + user.LastName;
                    ViewBag.User = user;
                    ViewBag.TotalUnReadMessage = unReadMessages.Count();
                    ViewBag.UnReadMessage = unReadMessages;
                }
                ViewBag.ActionName = ControllerContext.RouteData.Values["action"].ToString();
                ViewBag.ControllerName = ControllerContext.RouteData.Values["controller"].ToString();
            }
        }
    }
}