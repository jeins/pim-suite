using PIMSuite.Persistence;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
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