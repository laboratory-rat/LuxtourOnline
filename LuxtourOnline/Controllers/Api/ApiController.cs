using LuxtourOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.ObjectModel;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Threading;
using System.Threading.Tasks;

namespace LuxtourOnline.Controllers.Api
{
    public class AppApiController : ApiController, IDisposable
    {
        #region Attr

        protected Logger _logger = LogManager.GetCurrentClassLogger();
        protected AppUserManager _userManager { get { return HttpContext.Current.GetOwinContext().Get<AppUserManager>(); } }
        protected RoleManager<AppUserRole> _roleManager { get { return HttpContext.Current.GetOwinContext().Get<RoleManager<AppUserRole>>(); } }

        protected SiteDbContext _currentContext = null;
        protected SiteDbContext _context
        {
            get
            {
                if (_currentContext == null)
                    _currentContext = HttpContext.Current.GetOwinContext().Get<SiteDbContext>();

                return _currentContext;
            }
        }

        #endregion

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            //string ip = "";

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }


        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (_currentContext != null)
                _currentContext.Dispose();

            base.Dispose(disposing);
        }
    }

    #endregion
}


