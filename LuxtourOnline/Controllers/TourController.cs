using LuxtourOnline.Models;
using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "manager, admin")]
    public class TourController : BaseAppController, IDisposable
    {
        protected int _perPage = 20;

        [HttpGet]
        // GET: Tour
        public ActionResult Index(int? page)
        {
            if (page == null || (int)page < 1)
                page = 1;

            TourDisplayList model;

            try
            {
                var tours = _context.Tours.OrderByDescending(x => x.ModifyDate).Skip(((int)page - 1) * _perPage).Take(_perPage).ToList();

                model = new TourDisplayList(tours, (int)page, _perPage, _lang);
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public ActionResult Modify(int? id)
        {
            try
            {
                TourModifyModel model;

                if (id != null && id > 0)
                {
                    Tour data = _context.Tours.Where(x => x.Id == (int)id && !x.Deleted).FirstOrDefault();

                    if (data != null)
                    {
                        model = new TourModifyModel(data);
                        return View(model);
                    }
                }

                model = new TourModifyModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveChangesJson(TourModifyModel model)
        {
            if (!ModelState.IsValid)
            {
                string errors = "";

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        errors += error.ErrorMessage + "\n";
                }

                return Json(new { Result = "error", Data = errors }, JsonRequestBehavior.AllowGet);
            }

            Tour tour;

            if (model.Id < 0)
            {
                var image = ImageMaster.MoveTmpImage(model.Image);
                tour = Tour.Create(model, image, GetCurrentUser(_context));

                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();
            }
            else
            {
                tour = _context.Tours.Where(x => x.Id == model.Id).FirstOrDefault();
                var user = GetCurrentUser(_context);


                if (tour == null)
                    return null;


                if (model.Image.Id != tour.Image.Id)
                {
                    var image = ImageMaster.MoveTmpImage(model.Image);

                    image.Tour = tour;

                    ImageMaster.RemoveImage(tour.Image);

                    tour.Image.Copy(image);
                }

                tour.ModifyData(model, user);

                await _context.SaveChangesAsync();
            }

            return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Display(int id, string language = "")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = Constants.DefaultLanguage;
            }

            try
            {
                Tour tour;

                if ((tour = _context.Tours.Where(x => x.Id == id && !x.Deleted).FirstOrDefault()) == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                TourDisplayModel model = new TourDisplayModel(tour, language);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                Tour tour = _context.Tours.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();

                if (tour == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                if (tour.Orders != null && tour.Orders.Count > 0)
                {
                    tour.Enable = false;
                    tour.Deleted = true;
                }
                else
                {
                    if (tour.Image != null)
                    {
                        ImageMaster.RemoveImage(tour.Image);

                        _context.SiteImages.Remove(tour.Image);
                    }

                    _context.Tours.Remove(tour);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}