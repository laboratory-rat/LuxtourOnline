using LuxtourOnline.Models;
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Facebook;
using System.Dynamic;

namespace LuxtourOnline.Controllers
{
    //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "language", Duration = 60)]
    public class HomeController : BaseAppController, IDisposable
    {
        protected SiteDbContext _currentContext = null;

        protected string _fbToken = "access_token=1592303977741978|idYp70kTD_eA9FeofzGN6LrLkmo";
        protected string _fbHttps = "https://graph.facebook.com/v2.8";
        protected string _fbGroup = "luxtour.online/feed?fields=message,story,picture,link,likes,full_picture,actions";

        public HomeController(SiteDbContext newContext)
        {
            if (_currentContext == null)
                _currentContext = newContext;
        }

        public HomeController()
        {
            if (_currentContext == null)
                _currentContext = new SiteDbContext();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Tours()
        {
            return View();
        }

        protected int _defaultToursPerPage = 10;

        [HttpGet]
        public ActionResult GetTours(int? page, int? toursPerPage)
        {
            int currentPage = (page == null || (int)page < 1) ? 1 : (int)page;
            int perPage = (toursPerPage == null || (int)toursPerPage < 1) ? _defaultToursPerPage : (int)toursPerPage;



            List<Tour> tours;
            PagingInfo paging = new PagingInfo();

            _currentContext = _context;

            tours = _currentContext.Tours.OrderByDescending(x => x.CreateTime).Skip((currentPage - 1) * perPage).Take(perPage).ToList();

            List<dynamic> resultTures = new List<dynamic>();

            foreach(var tour in tours)
            {
                var descr = tour.Descritions.Where(x => x.Lang == _lang).FirstOrDefault();

                if (descr == null)
                    continue;

                resultTures.Add(new
                {
                    id = tour.Id,
                    child = tour.Child,
                    adult = tour.Adults,
                    price = tour.Price,
                    image = tour.Image.Url,
                    title = descr.Title,
                    description = descr.Description,
                    count = tour.DaysCount,
                });
            }

            paging.CurrentPange = currentPage;
            paging.ItemsPerPage = perPage;
            paging.TotalItems = tours.Count;

            return Json(
                new{
                    tours = resultTures,
                    lang = _lang,
                    paging = paging,
            }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Order(int id)
        {
            ViewBag.id = id;

            return View();
        }

        int _moreTours = 2;

        [HttpGet]
        public ActionResult OrderTourJson(int id)
        {
            dynamic unsver;

            var _c = _context;

            var tour = _c.Tours.Where(x => x.Id == id && x.Enable == true).FirstOrDefault();

            if (tour == null)
                return Json(new { result = "Error", data = "" }, JsonRequestBehavior.AllowGet);

            var descr = tour.Descritions.Where(x => x.Lang == _lang).FirstOrDefault();

            var hotels = _c.Hotels.Where(x => x.Avaliable == true && x.Deleted == false).ToList();

            List<dynamic> hots = new List<dynamic>();

            foreach (var h in hotels)
            {
                List<dynamic> apartments = new List<dynamic>();

                foreach(var a in h.Apartmetns)
                {
                    apartments.Add(new {
                        id = a.Id,
                        title = a.Title,
                        adult = a.Adult,
                        child = a.Child,
                    });
                }


                hots.Add(new { id = h.Id, title = h.Title, rate = h.Rate, apartments = apartments, avaliable = h.Avaliable});
            }


            unsver = new
            {
                title = descr.Title,
                description = descr.Description,
                price = tour.Price,
                child = tour.Child,
                adults = tour.Adults,
                count = tour.DaysCount,
                id = id,
                image = tour.Image.Url,
                hotels = hots,
            };



            return Json(new { result = "success", data = unsver }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult OrderHotelJson(int id)
        {
            var hotel = _currentContext.Hotels.Where(x => x.Id == id).FirstOrDefault();

            if (hotel == null)
                return Json(new { result = "error", data = "No such hotel" }, JsonRequestBehavior.AllowGet);

            var desc = hotel.Descriptions.Where(x => x.Lang == _lang).FirstOrDefault();

            List<dynamic> features = new List<dynamic>();


            foreach(var f in desc.Features)
            {
                List<dynamic> free = new List<dynamic>();
                List<dynamic> paid = new List<dynamic>();

                foreach(var ff in f.Free)
                    free.Add(new { title = ff.Title, ico = ff.Glyph});

                foreach (var ff in f.Paid)
                    paid.Add(new { title = ff.Title, ico = ff.Glyph });

                features.Add(new
                {
                    title = f.Title,
                    description = f.Description,
                    ico = f.Glyph,
                    free = free,
                    paid = paid,
                });
            }

            List<dynamic> images = new List<dynamic>();

            foreach(var image in hotel.Images)
                images.Add(new { image = image.Url });

            dynamic result = new
            {
                id = id,
                title = hotel.Title,
                rate = hotel.Rate,
                
                description = desc.Description,
                features = features,
                images = images,
            };

            return Json(new { result = "success", data = result }, JsonRequestBehavior.AllowGet);
        }

        protected int _newsCount = 10;

        [HttpGet]
        public ActionResult News(int? page, int? count)
        {
            if (page == null || page < 1)
                page = 1;

            if (count == null || count < 1)
                count = _newsCount;


            ViewBag.Page = (int)page;
            ViewBag.Count = (int)count;

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
        public async Task<ActionResult> GetNews(int page, int count)
        {
            if (page < 1)
                page = 1;

            if (count < 1)
                count = _newsCount;

            string st;
            string url = $"{_fbHttps}/{_fbGroup}&offset={(page - 1) * count}&limit={count}&{_fbToken}";

            using (WebClient client = new WebClient())
            {

                st = await client.DownloadStringTaskAsync(url);
            }

            dynamic result = new JavaScriptSerializer().DeserializeObject(st);

            return Json(result["data"], JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (_currentContext != null)
                _currentContext.Dispose();

            base.Dispose(disposing);

        }
    }

    public class TourViewModel
    {
        public List<Tour> Tours { get; set; }

        public string lang { get; set; }

        public PagingInfo Paging { get; set; }
    }

}