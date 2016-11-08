using LuxtourOnline.Models;
using LuxtourOnline.Models.Products;
using LuxtourOnline.Utilites;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class OrderController : BaseAppController, IDisposable
    {

        protected int _perPage = 50; // { get; set; }

        // GET: Order
        [HttpGet]
        public ActionResult Index(int? page)
        {
            if (page == null || (int)page < 1)
                page = 1;

            try
            {

                var orders = _context.Orders.OrderByDescending(x => x.OrderDate).Skip(((int)page - 1) * _perPage).Take(_perPage).ToList();
                var total = _context.Orders.Count();

                OrderDisplayListModel model = new OrderDisplayListModel(orders, (int)page, _perPage, total, _lang);

                return View(model);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        protected int _passwordLength = 8;
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreateOrderModel model)
        {

            try
            {
                Tour tour = null;
                Hotel hotel = null;
                Apartment apartment = null;

                tour = _context.Tours.Where(x => x.Id == model.TourId).FirstOrDefault();
                hotel = _context.Hotels.Where(x => x.Id == model.HotelId).FirstOrDefault();
                apartment = _context.Apartents.Where(x => x.Id == model.ApartmentId).FirstOrDefault();

                if (tour == null || hotel == null || apartment == null)
                {
                    throw new Exception("No tour hotel or apartment");
                }

                AppUser user = null;
                bool newUser = false;
                string password = "";

                if (User.Identity.IsAuthenticated)
                {
                    user = GetCurrentUser(_context);
                }
                else
                {
                    string email = model.Email;
                    if (email != "" && _context.Users.Where(x => x.Email == model.Email.ToLower()).Any())
                    {
                        user = _context.Users.Where(x => x.Email == model.Email).First();
                    }
                    else if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Phone) && !string.IsNullOrEmpty(model.City))
                    {
                        user = new AppUser(model.Email, model.FullName, LocationMaster.GetIp(), model.Phone, model.City);
                        password = IdGenerator.RandomString(_passwordLength);

                        var manager = new AppUserManager(new UserStore<AppUser>(_context));

                        await manager.CreateAsync(user, password);
                        await manager.AddToRoleAsync(user.Id, "user");
                        newUser = true;
                    }
                    else
                    {
                        throw new Exception("Bad model");
                    }
                }

                Order order = new Order(model, tour, hotel, apartment, user, _lang);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();



                string mail = "<center><h2>Вітаємо</h2><center>";
                mail += $"Ваше замовлення: {tour.Descritions.Where(x => x.Lang == _lang).First().Title} : {hotel.Title} : {apartment.Title}<br>";
                mail += $"До оплати: {order.Price}$ <br>";
                mail += $"Платіжна система: <a href='{Constants.PayLink(order.Id)}'>тут</a> <br>";

                if (newUser)
                {
                    string url = GetUrl() + "/Account";

                    mail += "<br><br>";
                    mail += $"Для вас створений особистй аккаунт, де ви маєте моливість побачити дані вашого замовелення<br>";
                    mail += $"<br><br>Login: {user.Email} <br>Password: {password}";
                    mail += $"<br><br> <a href='{url}'>Acount</a>";
                }

                await MailMaster.SendMailAsync(user.Email, "LuxtourOnline", "Order #" + order.Id.Substring(0, 10), mail);

                string t = "";
                t = $"{order.Id} / {order.Price}";

                await MailMaster.SendMailAsync(MailMaster.OrderMail, "New order", "Luxtour new order", t);
#if !DEBUG
#endif

                return Json(Constants.PayLink(order.Id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                throw new Exception();
            }

            

        }
    }



}