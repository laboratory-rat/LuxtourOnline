using LuxtourOnline.Models;
using LuxtourOnline.Utilites;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class BaseAppController : Controller, IDisposable
    {
        protected string _lang = "";

        protected readonly string[] AvalibleLangs = new string[] { "en", "ru", "uk" };
        protected readonly string COOKIE_TITLE = "AwdsSFwWQRTGSDWR";
        protected readonly string COOKIE_KEY = "asdwd3651sf8ef";
        protected Logger _logger = LogManager.GetCurrentClassLogger();

        protected AppUserManager _userManager { get { return HttpContext.GetOwinContext().Get<AppUserManager>(); } }
        protected RoleManager<AppUserRole> _roleManager { get { return HttpContext.GetOwinContext().Get<RoleManager<AppUserRole>>(); } }
        protected SiteDbContext _selfContext = null;
        protected SiteDbContext _context {
            get
            {
                if (_selfContext == null)
                    _selfContext = HttpContext.GetOwinContext().Get<SiteDbContext>();
                return _selfContext;
            }
            set
            {
                _selfContext = value;
            }
        }



        protected AppSignInManager _signInManager { get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); } }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (_lang != null)
                ViewBag.Lang = _lang;
            else
                ViewBag.Lang = Constants.DefaultLanguage;

            base.OnActionExecuted(filterContext);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                string language = (string)requestContext.RouteData.Values["language"] ?? "";
                string baseLang = language;
                language = language.ToLower();

                if (!AppConsts.Langs.Contains(language))
                {
                    language = AppConsts.DefaultLanguage;
                }

                var session = requestContext.HttpContext.Session;

                if (session["language"] == null || string.IsNullOrEmpty(session["language"].ToString()))
                {
                    HttpCookie cookie = requestContext.HttpContext.Request.Cookies[COOKIE_TITLE] ?? null;

                    if (cookie == null)
                    {
                        var lang = language == AppConsts.DefaultLanguage ? SelectLanguage(requestContext) : language;// SelectLanguage(requestContext, language);
                        SetLangCookie(lang, requestContext);

                        session.Add("language", lang);
                        _lang = lang;
                    }
                    else
                    {
                        session.Add("language", cookie[COOKIE_KEY]);
                        _lang = cookie[COOKIE_KEY];
                    }
                }
                else if (session["language"].ToString() != language)
                {
                    if (baseLang == "")
                    {
                        _lang = session["language"].ToString();
                        
                    }
                    else
                    {
                        session["language"] = language;
                        SetLangCookie(language, requestContext);
                        _lang = language;
                    }
                }
                else
                {
                    _lang = AppConsts.Langs.Contains(session["language"].ToString()) ? session["language"].ToString() : AppConsts.DefaultLanguage;
                }
                
            }
            catch(Exception ex)
            {
                _logger.Error<Exception>("Can't set language cookie! {0}", ex);
                _lang = "en";
            }


            base.Initialize(requestContext);
        }
        
        protected string SelectLanguage(RequestContext context, string defaultLanguage = "")
        {
            string result = "";

            var langs = context.HttpContext.Request.UserLanguages;

            foreach(var l in langs)
            {
                var ll = l.Substring(0, 2).ToLower();

                if (AppConsts.Langs.Contains(ll))
                {
                    result = ll;
                    break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                if (string.IsNullOrEmpty(defaultLanguage))
                    result = AppConsts.DefaultLanguage;
                else
                    result = defaultLanguage;
            }

            return result;
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

        protected void ChangeLang(string newLang, RequestContext context)
        {
            newLang = newLang.ToLower();

            if (AppConsts.Langs.Contains(newLang) && newLang != _lang)
            {
                _lang = newLang;
                SetLangCookie(newLang, context);
            }
        }

        protected override void Execute(RequestContext requestContext)
        {
            base.Execute(requestContext);
        }

        protected AppUser GetCurrentUser()
        {
            AppUser user = null;
            using (var c = _context)
            {
                user = GetCurrentUser(c);
            }

            return user;
        }

        protected AppUser GetCurrentUser(SiteDbContext context)
        {
            var store = new UserStore<AppUser>(context);
            var userManager = new UserManager<AppUser>(store);
            return userManager.FindByEmail(User.Identity.Name);
        }

        protected virtual string GetUrl()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
        }

        protected override void Dispose(bool disposing)
        {
            if (_selfContext != null)
                _selfContext.Dispose();

            base.Dispose(disposing);
        }
    }
}