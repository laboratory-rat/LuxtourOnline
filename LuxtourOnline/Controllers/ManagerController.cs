using LuxtourOnline.Models;
using LuxtourOnline.Models.Account;
using LuxtourOnline.Models.Manager;
using LuxtourOnline.Repos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Net;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "manager, admin")]
    public class ManagerController : BaseAppController
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tour(int count = 10, int offcet = 0)
        {
            List<ListTourModel> model = new List<ListTourModel>();

            using (var repo = new ManagerRepo())
            {
                model = repo.GetTourList(_lang, count, offcet);
            }

            return View(model);
        }

        #region Hotels
        [HttpGet]
        public ActionResult HotelsList(string lang = "en")
        {
            List<ManagerHotelList> model;

            if (!AvalibleLangs.Contains(lang))
                lang = "en";

            try
            {
                using (var repo = new ManagerRepo(_context))
                {
                    model = repo.GetHotelList(lang);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway, "F*ck, some server error. Connect to administrator.");
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult RemoveHotel(int id)
        {
            try
            {
                using (var repo = new ManagerRepo(_context))
                {
                    ManagerHotelRemove model = repo.GetRemoveHotelModel(id);

                    if (model == null)
                        return RedirectToAction("HotelsList");

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("HotelsList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveHotel(ManagerHotelRemove model)
        {
            if (model == null)
                return RedirectToAction("HotelsList");

            try
            {
                using (var repo = new ManagerRepo(_context))
                {
                    repo.RemoveHotel(model.Id);
                    await repo.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("HotelsList");
        }

        [HttpGet]
        public ActionResult CreateHotel()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateHotel(ManagerHotel hotel)
        {
            try
            {
                using (var repo = new ManagerRepo(_context))
                {
                    repo.CreateHotel(hotel);

                    await repo.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return Json("error");
            }
            
            return Json("success");
        }
        #endregion

        [HttpPost]
        public ActionResult UploadImage()
        {
            var image = Request.Files[0];
            string url;

            try
            {
                using (var repo = new ManagerRepo(_context))
                {
                    url = repo.UploadImage(image);
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error<Exception>(ex);
                return Json(new { status = "error", message = "bad file"});
            }

            return Json(new { status = "success", url = url });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserLoginModel model = new UserLoginModel();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindAsync(model.Email, model.Password);
            if (user != null)
            {
                bool inRole = await _userManager.IsInRoleAsync(user.Id, "manager") || await _userManager.IsInRoleAsync(user.Id, "admin");
                if (inRole)
                {
                    await _signInManager.SignInAsync(user, false, model.Remember);
                    return RedirectToAction("Index", "Manager");
                }
            }

            ModelState.AddModelError("", "Bad email or password");
            return View();
        }

    }



}