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
using LuxtourOnline.Utilites;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "manager, admin")]
    public class ManagerController : BaseAppController
    {
        protected ManagerRepo _repository { get { return new ManagerRepo(); } }

        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        #region Tour

        protected int _defaultCount = 10;

        public ActionResult TourList(int count = 10, int page = 0, int rate = -1, string title = "")
        {
            if (count < 1)
                count = _defaultCount;

            List<ListTourModel> list = new List<ListTourModel>();

            try
            {
                using (var repo = new ManagerRepo())
                {
                    list = repo.GetTourList(_lang, count, page);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            PageTourModel model = new PageTourModel(list, count, page);

            return View(model);
        }

        [HttpGet]
        public ActionResult EditTour(int id = -1)
        {
            EditTourModel model;

            try
            {
                if (id < 0)
                    model = new EditTourModel();
                else
                {
                    using (var repo = _repository)
                    {
                        model = repo.GetTourEditModel(id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("TourList");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTour(EditTourModel model)
        {
            var files = Request.Files;

            if (model.Id < 0 && model.Image == null)
                ModelState.AddModelError("", "Image is required");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    await repo.EditTour(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("TourList");
        }

        public ActionResult RemoveTour(int id, string lang = "")
        {
            RemoveTourModel model;

            if (string.IsNullOrEmpty(lang) || !AppConsts.Langs.Contains(lang))
                lang = _lang;

            try
            {
                using (var repo = _repository)
                {
                    model = repo.GetRemoveTourModel(id, lang);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("TourList");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveTour(RemoveTourModel model)
        {
            try
            {
                using (var repo = _repository)
                {
                    repo.RemoveTour(model);
                    await repo.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return RedirectToAction("TourList");
        }

        #endregion

        #region Hotels

        [HttpGet]
        public ActionResult HotelsList(string lang = "")
        {
            List<ManagerHotelList> model;

            if (!AppConsts.Langs.Contains(lang))
                lang = _lang;

            try
            {
                using (var repo = new ManagerRepo())
                {
                    model = repo.GetHotelList(lang);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveHotel(int id)
        {
            try
            {
                using (var repo = new ManagerRepo())
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
            {
                return RedirectToAction("HotelsList");
            }

            try
            {
                using (var repo = new ManagerRepo())
                {
                    repo.RemoveHotel(model);
                    await repo.SaveAsync();

                    var user = repo.GetCurrentUser();

                    _logger.Info($"deleted hotel. title: {model.Title}, delete images = {model.DeleteImages}, User: {user.FullName}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("HotelsList");
        }

        [HttpGet]
        public ActionResult EditHotel(int id = -1)
        {
            if (id >= 0)
            {
                try
                {
                    using (var c = _context)
                    {
                        if (!c.Hotels.Any(h => h.Id == id))
                            return RedirectToAction("HotelsList");
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(ex);
                    return RedirectToAction("Index");
                }
            }


            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public ActionResult GetHotelEdit(int id)
        {
            ManagerHotelEdit model = null;

            try
            {
                using (var repo = new ManagerRepo())
                {
                    if (id > 0)
                        model = repo.GetHotelEditModel(id);
                    else
                        model = repo.GetHotelEditModel();
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("HotelsList");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditHotel(ManagerHotelEdit model)
        {

            try
            {
                using (var repo = new ManagerRepo())
                {
                    repo.EditHotel(model);
                    await repo.SaveAsync();

                    var user = repo.GetCurrentUser();
                    _logger.Info($"User {user.FullName} created new hotel");

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }


            return Json("success");
        }

        [HttpGet]
        public ActionResult EditApartments(int id)
        {
            ManagerEditApartmentsModel model;

            try
            {
                using (var repo = _repository)
                {
                    model = repo.GetApartments(id);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(model);
        }

        public async Task<ActionResult> EditApartments(ManagerEditApartmentsModel model)
        {
            try
            {
                using (var repo = _repository)
                {
                    repo.EditApartments(model);
                    await repo.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return Json("success");
        }
        #endregion

        #region Utilites
        [HttpPost]
        public ActionResult UploadImage()
        {
            var image = Request.Files[0];
            HotelImage model;

            try
            {
                using (var repo = new ManagerRepo())
                {
                    model = repo.UploadImage(image);
                }

            }
            catch (Exception ex)
            {
                _logger.Error<Exception>(ex);
                return Json(new { status = "error", message = "bad file" });
            }

            return Json(new { status = "success", image = model });
        }

        [HttpPost]
        public ActionResult RemoveTmpImage(HotelImage image)
        {
            if (image.New && System.IO.File.Exists(image.Path))
            {
                System.IO.File.Delete(image.Path);
            }

            return Json("success");
        }

        #endregion Utilites





    }
}