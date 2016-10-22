using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using LuxtourOnline.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web;
using NLog;
using Microsoft.Owin.Infrastructure;

[assembly: OwinStartup(typeof(LuxtourOnline.App_Start.Startup))]

namespace LuxtourOnline.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.CreatePerOwinContext(() => new SiteDbContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<RoleManager<AppUserRole>>((options, context) =>
                new RoleManager<AppUserRole>(
                    new RoleStore<AppUserRole>(context.Get<SiteDbContext>())));

            app.CreatePerOwinContext<AppSignInManager>((options, context) => AppSignInManager.Create(options, context));


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/User/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(31),
            });

        }

    }
}
