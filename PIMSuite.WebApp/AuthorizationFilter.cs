using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public IUserRepository userRepository;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            if (HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                this.userRepository = new UserRepository(new DataContext());
                var username = HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;
                var user = userRepository.GetUserByUsername(username);
                if (user == null)
                {
                    HttpContext.Current.GetOwinContext().Authentication.SignOut();
                    return;
                }
                if (user.IsValidated)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(403, "User is not validated!");
                                        

                }
            }
            else
            {
                filterContext.Result = filterContext.Result = new HttpUnauthorizedResult();
            }
            }  


            // Check for authorization

        }
}


