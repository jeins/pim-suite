using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
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
            User user;
            if (ModelState.IsValid)
            {
                using (DataContext context = new DataContext())
                {
                    user = context.Users.Single(p => p.Email == model.EmailAddress);
                }
                if (user != null && string.Equals(user.Password, model.Password))
                {
                    ModelState.Clear();
                    return Request.CreateResponse(HttpStatusCode.Accepted, true);
                    //TODO: 
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "False password!");
                }     

                
            }
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Missing or wrong user credentials!");
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
