using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using PIMSuite.Persistence;

[assembly: OwinStartup(typeof(PIMSuite.WebApp.App_Start.Startup))]

namespace PIMSuite.WebApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cleanup table connection
            var db = new DataContext();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE[Connections]");

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/")
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            // SignalR hub configuration
            app.MapSignalR();
        }
    }
}
