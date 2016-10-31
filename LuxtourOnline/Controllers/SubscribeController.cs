using LuxtourOnline.Models;
using LuxtourOnline.Models.Products;
using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    public class SubscribeController : BaseAppController, IDisposable
    {
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult List(int? page, int perPage = 25)
        {
            if (page == null || page < 1)
                page = 1;

            if (perPage < 1)
                perPage = 25;

            ViewBag.PerPage = perPage;
            ViewBag.Page = (int)page;

            return View();
        }

        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult ListJson(int? page, int perPage = 25)
        {
            if (page == null)
            {
                page = 1;
            }

            if (perPage < 1)
                perPage = 25;

            ViewBag.EndOfList = false;
            ViewBag.PerPage = perPage;
            ViewBag.Page = (int)page;

            try
            {
                var sub = _context.Subscriptions.OrderByDescending(x => x.SubscribeDate).Skip(((int)page - 1) * perPage).Take(perPage).ToList();
                if (sub.Count < 1)
                {
                    ViewBag.EndOfList = true;
                }

                else
                {
                    PagingInfo info = new PagingInfo();
                    info.CurrentPange = (int)page;
                    info.ItemsPerPage = perPage;
                    info.TotalItems = _context.Subscriptions.Count();

                    SubscribeList model = new SubscribeList() { Subscribes = sub, Paging = info };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }

            return null;
        }

        [Authorize(Roles = "manager, admin")]
        [HttpPost]
        public async Task Toggle(int id)
        {
            try
            {
                var s = _context.Subscriptions.Where(x => x.Id == id).FirstOrDefault();

                if (s != null)
                {
                    s.IsActive = !s.IsActive;
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return;
            }
        }

        [Authorize(Roles = "manager, admin")]
        [HttpPost]
        public async Task Remove (int id)
        {
            try
            {
                if (_context.Subscriptions.Any(x => x.Id == id))
                {
                    var s = _context.Subscriptions.Where(x => x.Id == id).First();
                    _context.Subscriptions.Remove(s);

                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(SubscribeAddModel model)
        {
            if (!ModelState.IsValid)
                return null;

            try
            {

                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (_context.Subscriptions.Any(x => x.Email == model.Email) && !string.IsNullOrEmpty(model.Tel))
                    {
                        _context.Subscriptions.Where(x => x.Email == model.Email).First().Tel = model.Tel;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var sub = Subscribe.Create(model);
                        _context.Subscriptions.Add(sub);

                        

                        await _context.SaveChangesAsync();

                        SendRegisterEmail(sub);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Tel))
                {
                    if (!string.IsNullOrEmpty(model.Email) && _context.Subscriptions.Any(x => x.Tel == model.Tel))
                    {
                        _context.Subscriptions.Where(x => x.Tel == model.Tel).First().Email = model.Email;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Subscriptions.Add(Subscribe.Create(model));
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return null;
                }

                return Json(new { result = "succes" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        [HttpGet]
        public async Task<ActionResult> RemoveSubscription(string key = "")
        {
            ViewBag.success = false;
            ViewBag.badKey = false;

            if (string.IsNullOrEmpty(key))
            {
                return View();
            }
            else
            {
                try
                {
                    if (_context.Subscriptions.Any(x => x.RemoveSubscribeString == key))
                    {
                        _context.Subscriptions.Where(x => x.RemoveSubscribeString == key).First().RemoveSubscribe();
                        await _context.SaveChangesAsync();

                        ViewBag.success = true;
                    }
                    else
                    {
                        ViewBag.badKey = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            return View();
        }

        public ActionResult SubscriptionStatusJson(string email = "", string tel = "")
        {
            try
            {
                bool IsActive = false;

                if (!string.IsNullOrEmpty(email) && _context.Subscriptions.Any(x => x.Email == email))
                {
                    IsActive = _context.Subscriptions.Where(x => x.Email == email).First().IsActive;
                }
                else if (!string.IsNullOrEmpty(tel) && _context.Subscriptions.Any(x => x.Tel == tel))
                {
                    IsActive = _context.Subscriptions.Where(x => x.Tel == tel).First().IsActive;
                }

                return Json(new { status = IsActive }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public void SendRegisterEmail(Subscribe sub)
        {
            if (sub.Email != "" && sub.FullName != "")
            {

                string url = Url.Action("RemoveSubscription", new { key = sub.RemoveSubscribeString });
                string route = GetUrl() + url;

                string body = "Дякую за підписку на новини.<br> <br>"
                    + " Відписатися від новин можно перейшовши по" 
                    + " <a href='"+ route + "'>ссилці</a>";

                MailMaster.SendMail(sub.Email, "Підписка на новини", "Luxtour Online новини", body);
            }
        }
    }
}