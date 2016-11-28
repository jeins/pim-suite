using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.SignalR;
using PIMSuite.Persistence.Repositories;
using PIMSuite.WebApp.SignalRHub;

namespace PIMSuite.WebApp.Controllers.API
{
    public class LogoutController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            // Change user status to Offline
            var userId = HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId();
            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            IConnectionRepository connectionRepository = new ConnectionRepository(new DataContext());
            connectionRepository.RemoveUser(new Guid(userId), null);
            hub.Clients.All.onUserDisconnected(userId);

            HttpContext.Current.GetOwinContext().Authentication.SignOut();

            return Request.CreateResponse(HttpStatusCode.Accepted, true);
        }
    }
}