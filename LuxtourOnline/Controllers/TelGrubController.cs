using LuxtourOnline.Models;
using LuxtourOnline.Models.TelGrub;
using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class TelGrubController : BaseAppController, IDisposable
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddTel(TelGrubNewModel model)
        {
            try
            {
                var grub = TelGrubModel.Create(model, _lang);

                if (grub != null)
                {
                    _context.TelGrubs.Add(grub);
                    await _context.SaveChangesAsync();

                    var operators = _context.Users.Where(x => x.AllowTelGrub).ToList();//_context.TelOperators.ToList();

                    if (operators.Count > 0)
                    {
                        string name = grub.FullName;
                        string tel = grub.TelNumber.Substring(0, 6) + "****";

                        string link = $"{GetUrl()}{Url.Action("Take", new { key = grub.GrubKey})}";

                        var position = LocationMaster.GetLocation(grub.Ip);

                        //string mail = $"Замовлений дзвінок від користувача: '{name}'<br> Номер: {tel} <br> Мова: {_lang}<br><br> <a href='{link}'>Прийняти дзвінок</a><br><br>{link}";

                        string mail = "";

                        mail += "Замовлений дзвінок від користувача <br><br>";
                        mail += $"Ім'я: {name}<br> Tel: {tel}<br><br>";

                        if (position != null)
                        {
                            mail += $"Country code: {position.CountryCode} <br> Country name: {position.CountryName} <br> City: {position.City} <br> Region name: {position.RegionName} <br> Time zone: {position.TimeZone}";
                        }

                        mail += $"<br><br> <a href='{link}'>Прийняти замовлення</a>";

                        var operList = (from o in operators select o.Email).ToList();

                        await MailMaster.SendMailAsync(operList, "Запрос дзвінка", "Luxtour bot", mail);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return null;
            }

            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Take(string key, string Email)
        {
            TelGrubModelDisplay model = null;

            try
            {
                TelGrubModel data = null;
                var user = GetCurrentUser(_context);

                if ((data = _context.TelGrubs.Where(x => x.GrubKey == key && x.Status == TelGrubStatus.New).FirstOrDefault()) != null && user.AllowTelGrub)
                {
                    model = new TelGrubModelDisplay(data);

                    data.Grub(user);

                    await _context.SaveChangesAsync();
                    return View(model);
                }
                
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return View("Take_failed");
        }

        [HttpGet]
        public async Task<ActionResult> TakeResult (int id, string success = "", string comment = "")
        {
            try
            {
                bool res = (success == "on");

                var user = GetCurrentUser(_context);

                var model = _context.TelGrubs.Where(x => x.Id == id && x.Operator.Id == user.Id).FirstOrDefault();

                if (model != null)
                {
                    model.Result(res, comment);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("index", "Manager");
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListJson(int? page = 1, int? perPage = 50)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            if(perPage == null || perPage < 1)
            {
                perPage = 50;
            }

            var res = _context.TelGrubs.OrderByDescending(x => x.CreatedTime).Skip(((int)page - 1) * (int)perPage).Take((int)perPage).ToList();

            if (res.Count < 1)
                return null;

            var r = new TelGrubListModel(res, (int)page, (int)perPage);

            return Json(r, JsonRequestBehavior.AllowGet);
        }


        
    }
}