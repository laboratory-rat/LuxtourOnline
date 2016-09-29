using LuxtourOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LuxtourOnline.Repos
{
    public class BaseRepo : IDisposable
    {
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

        protected Logger _log = LogManager.GetCurrentClassLogger();

        protected string _basePath = HttpContext.Current.Request.PhysicalApplicationPath;

        public AppUser GetUser(string id)
        {
            var manager = _userManager;
            return manager.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public AppUser GetCurrentUser()
        {
            string name = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
            return user;
        }

        public List<string> GetUserRoles()
        {
            return GetUserRoles(GetCurrentUser().Id);
        }

        public List<string> GetUserRoles(string id)
        {
            var manager = _userManager;
            return manager.GetRoles(id).ToList();
        }

        public async Task SaveAsync()
        {
            try
            {
                var result = _context.GetValidationErrors();

                if (result.Count() == 0)
                    await _context.SaveChangesAsync();
                else
                {
                    string e = result.ToString();

                    throw new Exception(string.Format("Can't save model: {0}", e));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (_currentContext != null)
                _currentContext.Dispose();
        }
    }
}