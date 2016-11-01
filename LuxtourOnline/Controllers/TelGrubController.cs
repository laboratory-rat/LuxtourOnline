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
                var grub = TelGrubModel.Create(model);

                if (grub != null)
                {
                    _context.TelGrubs.Add(grub);
                    await _context.SaveChangesAsync();

                    var operators = _context.TelOperators.ToList();

                    if (operators.Count > 0)
                    {
                        string name = grub.FullName;
                        string tel = grub.TelNumber.Substring(0, 6) + "****";

                        string link = $"{GetUrl()}{Url.Action("Take", new { key = grub.GrubKey})}";

                        string mail = $"Замовлений дзвінок від користувача: '{name}'<br> Номер: {tel} <br><br> <a href='{link}'>Прийняти дзвінок</a>";



                        var operList = (from o in operators select o.OperatorEmail).ToList();

                        await MailMaster.SendMailAsync(operList, "Запрос дзвінка", "Class from site", mail);
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
            TelGrubNewModel model = new TelGrubNewModel();

            try
            {
                TelGrubModel data = null;
                var user = GetCurrentUser();

                if ((data = _context.TelGrubs.Where(x => x.GrubKey == key && x.Status == TelGrubStatus.New).FirstOrDefault()) != null && user.AllowTelGrub)
                {
                    model.TelNumber = data.TelNumber;
                    model.FullName = data.FullName;

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

        public ActionResult OperatorsList()
        {
            return View();
        }

        public ActionResult OperatorsListJson(int page, int perPage)
        {
            if (page < 1)
                page = 1;

            if (perPage < 1)
                perPage = 25;

            try
            {
                var list = _context.TelOperators.OrderByDescending(x => x.Id).Skip(((int)page - 1) * (int)perPage).Take((int)perPage).ToList();
                if (list.Count > 0)
                {
                    var res = new TelGrubOperatorsList(list, page, perPage);

                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }


            return null;
        }

        [HttpPost]
        public async Task<ActionResult> AddOperator(string email, string fullName)
        {
            TelGrubOperators oper = null;

            if ((oper = _context.TelOperators.Where(x => x.OperatorEmail == email).FirstOrDefault()) == null)
            {
                oper = new TelGrubOperators(fullName, email);
                _context.TelOperators.Add(oper);

                await _context.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> RemoveOperator(int id)
        {
            TelGrubOperators oper = null;

            if((oper = _context.TelOperators.Where(x => x.Id == id).FirstOrDefault()) != null)
            {
                _context.TelOperators.Remove(oper);
                await _context.SaveChangesAsync();
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }
        
    }
}