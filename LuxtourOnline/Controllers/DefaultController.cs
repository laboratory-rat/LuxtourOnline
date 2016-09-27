using LuxtourOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LuxtourOnline.Controllers
{
    public class BaseAppController : Controller
    {
        protected string _lang = "";

        protected readonly string[] AvalibleLangs = new string[] { "en", "ru", "uk" };
        protected readonly string COOKIE_TITLE = "AwdsSFwWQRTGSDWR";
        protected readonly string COOKIE_KEY = "asdwd3651sf8ef";
        protected Logger _logger = LogManager.GetCurrentClassLogger();

        protected AppUserManager _userManager { get { return HttpContext.GetOwinContext().Get<AppUserManager>(); } }
        protected RoleManager<AppUserRole> _roleManager { get { return HttpContext.GetOwinContext().Get<RoleManager<AppUserRole>>(); } }
        protected SiteDbContext _context { get { return HttpContext.GetOwinContext().Get<SiteDbContext>(); } }

        protected AppSignInManager _signInManager { get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); } }

        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                string language = (string)requestContext.RouteData.Values["language"] ?? "";
                language = language.ToLower();

                if (!string.IsNullOrEmpty(language) && AvalibleLangs.Contains(language))
                {
                    _lang = language;
                    SetLangCookie(language, requestContext);
                }
                else
                {
                    var cookie = requestContext.HttpContext.Request.Cookies[COOKIE_TITLE] ?? new HttpCookie(COOKIE_TITLE);
                    if (string.IsNullOrEmpty(cookie[COOKIE_KEY]) || !AvalibleLangs.Contains(cookie[COOKIE_KEY]))
                    {
                        var l = requestContext.HttpContext.Request.UserLanguages[0].ToLower().Substring(0, 2);

                        if (!AvalibleLangs.Contains(l))
                            l = "en";

                        SetLangCookie(l, requestContext);
                    }
                    else
                    {
                        _lang = cookie[COOKIE_KEY];
                    }
                }
                
            }
            catch(Exception ex)
            {
                _logger.Error<Exception>("Can't set language cookie! {0}", ex);
                _lang = "en";
            }


            base.Initialize(requestContext);
        }
        
        protected virtual void SetLangCookie(string lang, RequestContext context)
        {
            lang = lang.ToLower();

            if (!AvalibleLangs.Contains(lang))
            {
                _logger.Error("Bad language as cookie param: {0}", lang);
                return;
            }

            var cookie = new HttpCookie(COOKIE_TITLE);
            cookie[COOKIE_KEY] = lang;
            cookie.Expires = DateTime.Now.AddDays(365);

            context.HttpContext.Response.SetCookie(cookie);
        }

        protected override void Execute(RequestContext requestContext)
        {
            base.Execute(requestContext);
        }
    }
}