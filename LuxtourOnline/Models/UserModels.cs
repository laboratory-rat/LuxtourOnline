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
using LuxtourOnline.WebUI;
using LuxtourOnline.Models.TelGrub;
using LuxtourOnline.Models.Products;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LuxtourOnline.Models
{

    #region System 

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
            FullName = fullName;
            PhoneNumber = phone;
            RegIp = ip;
            RegDate = DateTime.Now.Date.ToShortDateString();
        }

        public AppUser (string email, string fullName, string ip, string phone, string city) : this(email, fullName, ip, phone)
        {
            City = city;
        }

        public string FullName { get; set; }
        public string RegIp { get; set; }
        public bool Active { get; set; } = true;
        public string City { get; set; } = "";
        public string RegDate { get; set; } = DateTime.Now.Date.ToShortDateString();

        public bool AllowTelGrub { get; set; } = false;

        [Required]
        public virtual List<TelGrubModel> TelGrubs { get; set; } = new List<TelGrubModel>();

        [Required]
        public virtual List<Order> Orders { get; set; } = new List<Order>();
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

    #endregion System

    public class SelfUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public SelfUserModel()
        {

        }

        public SelfUserModel(AppUser user, string roles)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            Role = roles;
            PhoneNumber = user.PhoneNumber;
        }
    }

    public class ChangePassword
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm { get; set; }
    }

    public class ChangeEmail
    {


        [Required]
        public string Id { get; set; }

        [EmailAddress]
        [Required]
        public string NewEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string  Password { get; set; }

        public ChangeEmail()
        {

        }

        public ChangeEmail(AppUser user)
        {
            Id = user.Id;
        }
    }

    public class ChangeFullName
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ChangeFullName()
        {

        }

        public ChangeFullName(AppUser user)
        {
            Id = user.Id;
            FullName = user.FullName;
        }

    }

    public class ChangePhoneNumber
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ChangePhoneNumber()
        {

        }

        public ChangePhoneNumber(AppUser user)
        {
            Id = user.Id;
            PhoneNumber = user.PhoneNumber;
        }
    }


    #region Manager

    public class UpdateUserModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        public string Role { get; set; }

        public string NewRole { get; set; }

        public bool AllowTelGrub { get; set; }

        public UpdateUserModel()
        {

        }

        public UpdateUserModel(AppUser user, string role)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Role = role;
            AllowTelGrub = user.AllowTelGrub;
        }

    }

    public class ListUserModel
    {
        public PagingInfo Paging { get; set; }
        public List<DisplayUserModel> Users { get; set; }
    }

    public class DisplayUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
        public string CreatedDate { get; set; }
        public bool AllowTelGrub { get; set; }
    }

    public class CreateUserModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string RoleToUse { get; set; }
    }

    public class RemoveUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public RemoveUserModel()
        {

        }

        public RemoveUserModel(AppUser user)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
        }
    }

    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string RedirectUrl { get; set; }

    }

    public class UserRegistrationModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string Confirm { get; set; }
    }

    #endregion Manager

    public class AccountDisplayModel
    {
        public List<OrderStatusModel> Orders { get; set; } = new List<OrderStatusModel>();
        public AppUser User { get; set; } = null;

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus LastStatus
        {
            get { return Orders.Count > 0 ? Orders[0].Status : OrderStatus.Null; }
        }


        public AccountDisplayModel()
        {

        }

        public AccountDisplayModel(List<Order> orders, AppUser user) : this()
        {
            foreach(var o in orders.OrderByDescending(x => x.OrderDate))
            {
                Orders.Add(new OrderStatusModel(o));
            }
        }
    }

    public class OrderStatusModel
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }

        public OrderStatusModel()
        {

        }

        public OrderStatusModel(Order data) : this()
        {
            Id = data.Id;
            Status = data.Status;
        }
    }


}