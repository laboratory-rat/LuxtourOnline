using LuxtourOnline.Models;
using LuxtourOnline.Models.Account;
using LuxtourOnline.Models.Manager;
using LuxtourOnline.Repos;
using LuxtourOnline.Utilites;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize]
    public class UserController : BaseAppController
    {
        UserRepo _repository { get { return new UserRepo(); } }
        protected int _usersPerPage = 15;

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                using (var repo = _repository)
                {
                    var user = repo.GetCurrentUser();
                    SelfUserModel model = new SelfUserModel(user, string.Join(", ", repo.GetUserRoles(user.Id)));
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public ActionResult Password()
        {
            try
            {
                ChangePassword model;

                using (var repo = _repository)
                {
                    var user = repo.GetCurrentUser();

                    model = new ChangePassword() { Id = user.Id };
                }

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Password(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    bool success = await repo.ChangePassword(model);

                    if (!success)
                    {
                        ModelState.AddModelError("", "Bad password");
                        return View(model);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Email()
        {
            ChangeEmail model;

            try
            {
                using (var repo = _repository)
                {
                    var user = repo.GetCurrentUser();
                    model = new ChangeEmail(user);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Email(ChangeEmail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    bool success = await repo.ChangeEmail(model);

                    if (!success)
                    {
                        ModelState.AddModelError("", "Bad password");
                        return View(model);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Fullname()
        {
            ChangeFullName model;

            try
            {
                using (var repo = _repository)
                {
                    var user = repo.GetCurrentUser();

                    model = new ChangeFullName(user);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Fullname(ChangeFullName model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    bool s = await repo.ChangeFullName(model);

                    if (!s)
                    {
                        ModelState.AddModelError("", "Bad password");
                        return View(model);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PhoneNumber()
        {
            ChangePhoneNumber model;

            try
            {
                using (var repo = _repository)
                {
                    model = repo.ChangePhoneModel();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PhoneNumber(ChangePhoneNumber model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                using (var repo = _repository)
                {
                    bool result = await repo.ChangePhoneNumber(model);

                    if (!result)
                    {
                        ModelState.AddModelError("", "Bad password");
                        model.Password = "";
                        return View(model);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        [Authorize(Roles = "manager, admin")]
        public ActionResult Update(string id)
        {
            UpdateUserModel model;

            try
            {
                using (var repo = _repository)
                {
                    var user = repo.GetUser(id);
                    var role = string.Join(", ", repo.GetUserRoles(user.Id));

                    model = new UpdateUserModel(user, role);
                }

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("UserList");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> Update(UpdateUserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    await repo.UpdateUser(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("UserList");
        } 

        [HttpGet]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> ToggleAllowTelGrub(string userId)
        {
            try
            {
                var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    user.AllowTelGrub = !user.AllowTelGrub;
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            var url = Request.UrlReferrer.ToString();

            return Redirect(url);
        }

        [HttpGet]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> UserList(int page = 1, string email = "", string fullname = "", string role = "", string phone = "")
        {
            if (page < 1)
                page = 1;

            try
            {
                using (var repo = _repository)
                {
                    ListUserModel model = await repo.UserList(_usersPerPage, page, email, fullname, role, phone);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("UserList");
            }
        }

        // GET: User
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> Display(string id)
        {
            try
            {
                using (var repo = _repository)
                {
                    DisplayUserModel model = await repo.GetDisplayUser(id);
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("UserList");
            }
        }
    
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult Remove(string id)
        {
            try
            {
                using (var repo = new UserRepo())
                {
                    RemoveUserModel model = repo.GetUserToRemove(id);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("UserList");
            }
        }

        [HttpGet]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> RemoveConfirm(string id)
        {
            try
            {
                using (var repo = new UserRepo())
                {
                    await repo.RemoveUser(id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("UserList");
        }


        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult Create()
        {
            CreateUserModel model = new CreateUserModel();

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> Create(CreateUserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string email = model.Email;
            var manager = _userManager;
            var roleManager = _roleManager;

            var role = model.RoleToUse;

            if (!_roleManager.RoleExists(role))
                ModelState.AddModelError("", "Bad role name");

            if ((User.IsInRole("manager") || User.IsInRole("content_manager")) && role == "admin")
                ModelState.AddModelError("", "No access for action.");

            if (await manager.FindByEmailAsync(email) != null)
                ModelState.AddModelError("", "Email already exists");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var password = AppRandom.RandomString(10);

                AppUser user = new AppUser(email, model.FullName, Request.UserHostAddress, model.PhoneNumber);
                await manager.CreateAsync(user, password);
                await manager.AddToRoleAsync(user.Id, role);

                var current = User.Identity.Name;

                _logger.Info($"User {current} created new user: {user.FullName} / {user.Email} with role: {role}");

                ViewBag.Email = email;
                ViewBag.Password = password;



                return View("CreateUserSuccess");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("UserList");//return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error. Go back later...");
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ChangeLanguage(string lang)
        {
            var controller = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Home").Trim('/');
            var action = (Request.UrlReferrer.Segments.Skip(3).Take(1).SingleOrDefault() ?? "Index").Trim('/');

            ChangeLang(lang, HttpContext.Request.RequestContext);

            return RedirectToAction(action, controller, new { language = lang });   
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Index", "Home");
            }

            UserLoginModel model = new UserLoginModel();
            model.RedirectUrl = ReturnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindAsync(model.Email, model.Password);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, false, model.Remember);

                if (!string.IsNullOrEmpty(model.RedirectUrl))
                    return Redirect(model.RedirectUrl);

                bool isManager = await _userManager.IsInRoleAsync(user.Id, "manager") || await _userManager.IsInRoleAsync(user.Id, "admin");
                if (isManager)
                    return RedirectToAction("Index", "Manager");
                else
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Bad email or password");
            return View();
        }

    }
}
