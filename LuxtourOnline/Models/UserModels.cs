using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;

namespace LuxtourOnline.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser() : base() { }
        public AppUser(string email) : base(email)
        {
            Email = email;
        }

        public AppUser(string email, string fullName, string ip, string phone) : base(email)
        {
            Email = email;
            PhoneNumber = phone;
            RegIp = ip;
            RegDate = DateTime.Now.Date.ToShortDateString();
        }

        public string FullName { get; set; }
        public string RegIp { get; set; }
        public bool Active { get; set; } = true;
        public string RegDate { get; set; } = DateTime.Now.Date.ToShortDateString();
    }

    public class AppUserRole : IdentityRole
    {
        public AppUserRole()
        {
        }

        public AppUserRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 7,
                RequireLowercase = false,
                RequireDigit = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };
        }

        public async void AddUserAsync(string fullName, string email, string password, string ip)
        {
            AppUser user = new AppUser() { Email = email, UserName = fullName, PasswordHash = (new PasswordHasher().HashPassword(password)), RegIp = ip, RegDate = DateTime.Now.Date.ToShortDateString() };
            await Store.CreateAsync(user);
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<SiteDbContext>()));

            // optionally configure your manager
            // ...

            return manager;
        }

        public static AppUserManager Create(IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<SiteDbContext>()));
            return manager;
        }

    }

    public class AppSignInManager : SignInManager<AppUser, string>
    {
        public AppSignInManager(UserManager<AppUser, string> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }

}