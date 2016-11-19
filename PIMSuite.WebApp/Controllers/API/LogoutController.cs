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

namespace PIMSuite.WebApp.Controllers.API
{
    public class LogoutController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return Request.CreateResponse(HttpStatusCode.Accepted, true);
        }
    }
}