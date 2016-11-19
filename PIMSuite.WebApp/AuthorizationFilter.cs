using System;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
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
                return;
            }
            // Check for authorization
            else
            {
                filterContext.Result = filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
