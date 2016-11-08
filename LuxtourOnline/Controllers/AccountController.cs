using LuxtourOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize]
    public class AccountController : BaseAppController, IDisposable
    {
        // GET: Account

        public ActionResult Index()
        {
            AccountDisplayModel model;

            try
            {
                var user = GetCurrentUser(_context);

                var list = _context.Orders.OrderByDescending(x => x.OrderDate).Where(x => x.User.Id == user.Id).ToList();

                model = new AccountDisplayModel(list, user);

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(model);
        }

        public ActionResult Tours()
        {
            return View();
        }
    }
}