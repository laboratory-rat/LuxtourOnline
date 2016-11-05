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
    public class HotelController : BaseAppController, IDisposable
    {
        // GET: Hotel

        protected int _perPage = 20;

        [HttpGet]
        [Route("Hotel/Index")]
        [Route("Hotel/List")]
        [Route("Hotel/Modify/All")]
        [Route("Hotel/XyuSosi")]
        public ActionResult Index(int? page = 1)
        {
            if (page == null || page < 0)
                page = 1;

            var hotels = _context.Hotels.OrderByDescending(x => x.ModifyDate).Skip(((int)page - 1) * _perPage).Take(_perPage).ToList();

            HotelDisplayList model = new HotelDisplayList(hotels, (int)page, _perPage);

            return View(model);
        }

        public ActionResult Modify(int? id)
        {
            try
            {
                HotelDisplayModel model;

                if (id != null && id > 0)
                {
                    Hotel hotel = _context.Hotels.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();

                    if (hotel != null)
                    {
                        model = new HotelDisplayModel(hotel);
                        return View(model);
                    }
                }

                model = HotelDisplayModel.Create();
                return View(model);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }



            throw new Exception();
        }

        public async Task<ActionResult> SaveChangesAsync(HotelDisplayModel model)
        {
            try
            {
                if (model.Id < 0)
                {
                    Hotel hotel = new Hotel(model, GetCurrentUser(_context));

                    _context.Hotels.Add(hotel);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Hotel hotel = _context.Hotels.Where(x => x.Id == model.Id && !x.Deleted).FirstOrDefault();

                    if (hotel == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    }

                    hotel.Modify(model, GetCurrentUser(_context));
                    await _context.SaveChangesAsync();
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }

            catch(Exception ex)
            {
                _logger.Error(ex);
                throw new Exception("Fatal error");
            }

        } 

        [HttpGet]
        public ActionResult Display(int id, string language = "")
        {
            try
            {
                var hotel = _context.Hotels.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();

                if(hotel == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                HotelRemoveModel model = new HotelRemoveModel(hotel, language);
                return View(model);
            }
            catch(Exception ex)
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
                Hotel hotel;

                if((hotel = _context.Hotels.Where(x => x.Id == id && !x.Deleted).FirstOrDefault()) == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                while(hotel.Images.Count > 0)
                {
                    ImageMaster.RemoveImage(hotel.Images[0]);
                    _context.SiteImages.Remove(hotel.Images[0]);
                }

                if(hotel.Orders != null && hotel.Orders.Count > 0)
                {
                    hotel.Deleted = true;
                }
                else
                {
                    _context.Hotels.Remove(hotel);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public ActionResult Apartments(int id)
        {
            try
            {
                Hotel hotel;

                if((hotel = _context.Hotels.Where(x => x.Id == id && !x.Deleted).FirstOrDefault()) == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                ApartmentEditModel model = new ApartmentEditModel(hotel);
                return View(model);

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveApartmentsChanges (ApartmentEditModel model)
        {
            try
            {
                Hotel hotel = null;

                if ((hotel = _context.Hotels.Where(x => x.Id == model.Hotel.Id && !x.Deleted).FirstOrDefault()) == null)
                {
                    throw new KeyNotFoundException();
                }

                hotel.UpdateApartments(model.Apartments);
                await _context.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                throw new Exception("Internal error");
            }
        }
    }
}