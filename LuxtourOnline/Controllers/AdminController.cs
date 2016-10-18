using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuxtourOnline;
using LuxtourOnline.Models;
using LuxtourOnline.Utilites;
using Microsoft.AspNet.Identity.Owin;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "admin")]
    [HandleErrorExtension]
    public class AdminController : BaseAppController, IDisposable
    {
        protected SiteDbContext _currentContext = null;

        public AdminController()
        {
            _currentContext = new SiteDbContext();
        }

        public AdminController(SiteDbContext context)
        {
            _currentContext = context;
        }

        public ActionResult Index()
        {
            return View();
        }


        protected int _defaultCount = 30;
        public ActionResult Logs(int page = 1, int count = 30, string errorMessage = "")
        {
            if (page < 1)
                page = 1;

            if (count < 0 || count > 500)
                count = _defaultCount;

            LogListModel model = new LogListModel();

            List<Log> list;
            PagingInfo pagin = new PagingInfo();

            if (errorMessage != "")
            {
                list = _currentContext.Logs.OrderByDescending(x => x.EventDateTime).Where(x => x.EventMessage.Contains(errorMessage.ToLower())).Skip((page - 1) * count).Take(count).ToList();
                pagin.TotalItems = _currentContext.Logs.Where(x => x.EventMessage.Contains(errorMessage.ToLower())).Count();
            }
            else
            { 
                list = _currentContext.Logs.OrderByDescending(x => x.EventDateTime).Skip((page - 1) * count).Take(count).ToList();
                pagin.TotalItems = _currentContext.Logs.Count();
            }

            pagin.ItemsPerPage = count;
            pagin.CurrentPange = page;

            model.Logs = list;
            model.Pagin = pagin;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearLogs()
        {
            try
            {
                using (var c = _currentContext)
                {
                    if (c.Logs.Count() != 0)
                    {  
                        var list = c.Logs.ToList();
                        c.Logs.RemoveRange(list);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error(e);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (_currentContext != null)
                _currentContext.Dispose();

            base.Dispose(disposing);
        }
    }

   

    public class LogListModel
    {
        public List<Log> Logs { get; set; }
        public PagingInfo Pagin { get; set; }

        public LogListModel()
        {

        }

        public LogListModel(List<Log> list, PagingInfo paging)
        {
            Logs = list;
            Pagin = paging;
        }
    }
}