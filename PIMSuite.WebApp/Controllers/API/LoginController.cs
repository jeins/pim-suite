using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PIMSuite.WebApp.Controllers.API
{
    public class LoginController : ApiController
    {
        public sealed class LoginModel
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginModel model)
        {
            // validate

            // set cookie

            return Request.CreateResponse(HttpStatusCode.Accepted, true);
        }

        public sealed class RegisterModel
        {
            //TODO
        }

        [HttpPost]
        public HttpResponseMessage Register(RegisterModel model)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
