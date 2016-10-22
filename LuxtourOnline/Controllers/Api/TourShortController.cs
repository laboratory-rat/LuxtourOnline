using LuxtourOnline.Models;
using LuxtourOnline.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LuxtourOnline.Controllers.Api
{
    public class TourShortController : AppApiController, IDisposable
    {

        public ApiTourShort Get(int id, string language = "")
        {
            if (!Constants.AvaliableLangs.Contains(language))
                language = Constants.DefaultLanguage;

            ApiTourShort output = null;

            try
            {
                var tour = _context.Tours.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();
                if (tour == null)
                    return null;

                var descr = tour.Descritions.Where(x => x.Lang == language).First();

                output = new ApiTourShort()
                {
                    Id = tour.Id,
                    Title = descr.Title,
                    Image = Constants.OutImageUrl(tour.Image.Url),
                    Url = Constants.TourOutUrl(tour.Id),
                };

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return output;
        }


        public enum TourOrder { Null = 0, Date, Hot}
        public List<ApiTourShort> Get(int count = 10, TourOrder order = TourOrder.Null, string language = "uk")
        {
            List<ApiTourShort> output = new List<ApiTourShort>();

            if (!Constants.AvaliableLangs.Contains(language))
                language = Constants.DefaultLanguage;

            if (count < 1)
                count = 10;

            try
            {
                List<Tour> tours;

                if (order == TourOrder.Null)
                    tours = _context.Tours.Where(x => !x.Deleted && x.Enable).ToList();
                else if(order == TourOrder.Date)
                    tours = _context.Tours.Where(x => !x.Deleted && x.Enable).OrderByDescending(x => x.ModifyDate).ToList();
                else
                    tours = _context.Tours.Where(x => !x.Deleted && x.Enable).OrderByDescending(x => x.ModifyDate).ToList();

                count = Math.Min(count, tours.Count);

                

                for (int i = 0; i < count; i++)
                {
                    var descr = tours[i].Descritions.Where(x => x.Lang == language).FirstOrDefault();

                    output.Add(new ApiTourShort()
                    {
                        Id = tours[i].Id,
                        Title = descr == null ? "" : descr.Title,
                        Image = tours[i].Image == null ? "" : Constants.OutImageUrl(tours[i].Image.Url),
                        Url = Constants.TourOutUrl(tours[i].Id),
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }

            return output;
        }

        [Route("all")]
        public List<ApiTourShort>Get(TourOrder order = TourOrder.Null, string language = "uk")
        {
            int count = _context.Tours.Count();
            return Get(count, order, language);
        }
    }
}
