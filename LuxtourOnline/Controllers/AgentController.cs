using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuxtourOnline.Models.Account;
using LuxtourOnline.Models;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace LuxtourOnline.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private AppUserManager _userManager { get { return HttpContext.GetOwinContext().Get<AppUserManager>(); } }
        private RoleManager<AppUserRole> _roleManager { get { return HttpContext.GetOwinContext().Get<RoleManager<AppUserRole>>(); } }
        private AppSignInManager _signInManager { get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); } }



        // GET: Agent
        public ActionResult Index()
        {

            ViewBag.user = User.Identity.GetUserName();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string redirectUrl)
        {
            UserLoginModel model = new UserLoginModel() { RedirectUrl = redirectUrl };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            using (SiteDbContext db = new SiteDbContext())
            {
                var manager = _userManager;
                var user = await manager.FindAsync(model.Email, model.Password);

                if (user != null)
                {
                    var sign = _signInManager;
                    await sign.SignInAsync(user, false, model.Remember);

                    if (model.RedirectUrl == "")
                        model.RedirectUrl = "agent/index";

                    return RedirectToRoute(model.RedirectUrl);       
                }
            }

            ModelState.AddModelError("", "No such user");
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser(model.Email);
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.RegIp = Request.UserHostAddress;
            user.RegDate = DateTime.Now.Date.ToShortDateString();

            using (SiteDbContext db = new SiteDbContext())
            {
                var userManager = new AppUserManager(new UserStore<AppUser>(db));
                var result = await userManager.CreateAsync(user, model.Password);
              
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user.Id, "agent");
                    return RedirectToAction("Index");
                }
            }


            return View();
        }
    }
}