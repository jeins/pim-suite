using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers.API
{
    public class NotificationController : ApiController
    {
        // Constructors

        public NotificationController()
        {
            _notificationRepository = new NotificationRepository(new DataContext());
        }

        // Fields

        private readonly INotificationRepository _notificationRepository;

        // Methods

        [HttpPost]
        public HttpResponseMessage ClearNotifications()
        {
            var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
            _notificationRepository.ClearNotificationsForUser(userId);
            _notificationRepository.Save();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        } 
    }
}