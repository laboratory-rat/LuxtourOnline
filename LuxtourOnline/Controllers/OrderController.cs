using LuxtourOnline.Models;
using LuxtourOnline.Models.Products;
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

        protected List<Order> _testOrderList = new List<Order>()
        {
            new Order() {Id = "123", Apartment = new Apartment() {Id = 123 }, City = "Lviv", Comments = "asd", Ip="192.100.100.150", FlyOutCity = "Lviv", Status = OrderStatus.Сonsideration, CustomersData = new List<CustomerData>(), Date = DateTime.Now, Email = "asd@asd.com", Hotel = new Hotel() {Id = 123, Title="TestHotel"}, OrderDate = DateTime.Now, Phone = "000000", Price = 1000, Tour = new Tour() {Id = 123, Descritions = new List<TourDescription>() { new TourDescription() { Id = 123, Lang = "en", Title = "123" } } } },
            new Order() {Id = "321", Apartment = new Apartment() {Id = 123 }, City = "Lviv", Comments = "asd", Ip="192.100.100.150", FlyOutCity = "Lviv", Status = OrderStatus.Сonsideration, CustomersData = new List<CustomerData>(), Date = DateTime.Now, Email = "asd@asd.com", Hotel = new Hotel() {Id = 123, Title="TestHotel"}, OrderDate = DateTime.Now, Phone = "000000", Price = 1000, Tour = new Tour() {Id = 123, Descritions = new List<TourDescription>() { new TourDescription() { Id = 123, Lang = "en", Title = "123" } } } },
            new Order() {Id = "321", Apartment = new Apartment() {Id = 123 }, City = "Lviv", Comments = "asd", Ip="192.100.100.150", FlyOutCity = "Lviv", Status = OrderStatus.Сonsideration, CustomersData = new List<CustomerData>(), Date = DateTime.Now, Email = "asd@asd.com", Hotel = new Hotel() {Id = 123, Title="TestHotel"}, OrderDate = DateTime.Now, Phone = "000000", Price = 1000, Tour = new Tour() {Id = 123, Descritions = new List<TourDescription>() { new TourDescription() { Id = 123, Lang = "en", Title = "123" } } } },
        };

        // GET: Order
        [HttpGet]
        public ActionResult Index(int? page)
        {
            if (page == null || (int)page < 1)
                page = 1;

            try
            {

                //var orders = _context.Orders.OrderByDescending(x => x.OrderDate).Skip(((int)page - 1) * _perPage).Take(_perPage).ToList();
                //var total = _context.Orders.Count();

                var orders = _testOrderList;
                var total = _testOrderList.Count();

                OrderDisplayListModel model = new OrderDisplayListModel(orders, (int)page, _perPage, total, _lang);

                return View(model);

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Show()
        {
            string number = "";
            string code = "";

            if(!string.IsNullOrEmpty((string)Session["number"]) && !string.IsNullOrEmpty((string)Session["code"]))
            {
                number = (string)Session["number"];
                code = (string)Session["code"];
            }
            else
            {
                HttpCookieCollection collection = Request.Cookies;
                HttpCookie ck = collection.Get("QuickEnter");

                if(ck != null)
                {
                    number = ck["number"];
                    code = ck["code"];
                }
            }

            InputQuickCode model = new InputQuickCode() { Number = number, Code = code, RememberMe = true };

            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(code))
            {
                return View(model);
            }

            return Show(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Show(InputQuickCode model)
        {
            if (model.Code == "")
            {
                return View(model);
            }

            try
            {
                var order = _context.Orders.Where(x => x.QuickAccessNumber == model.Number && x.Status != OrderStatus.Finished).FirstOrDefault();

                if (order == null || !order.ComparePassword(model.Code))
                {
                    model.Code = "";

                    ModelState.AddModelError("", "Нівірний номер замовлення");

                    return View(model);
                }

                if(model.RememberMe)
                {
                    HttpCookie ck = new HttpCookie("QuickEnter");
                    ck["number"] = model.Number;
                    ck["code"] = model.Code;
                    ck.Expires = DateTime.Now.AddDays(3d);
                    Response.Cookies.Add(ck);
                }

                Session["number"] = model.Number;
                Session["code"] = model.Code;

                return View("ShowOrderData", new OrderDisplayModel(order, _lang));

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index", "Home", null);
            }
                

        }
    }
}