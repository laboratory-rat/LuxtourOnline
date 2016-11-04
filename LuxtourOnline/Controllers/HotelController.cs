using LuxtourOnline.Models;
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

        public ActionResult Display(int id)
        {
            throw new Exception();
        }

        public ActionResult Remove(int id)
        {
            throw new Exception();
        }

        public ActionResult RemoveConfirm(int id)
        {
            throw new Exception();
        }
    }
}