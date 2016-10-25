using LuxtourOnline.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LuxtourOnline.Controllers.Api
{
    public class TourController : AppApiController, IDisposable
    {
        protected int _perPage = 10;

        public List<ApiTour> Get(int count, int offset, string language)
        {
            if (count < 1)
                count = _perPage;

            if (offset < 0)
                offset = 0;

            if (!Constants.AvaliableLangs.Contains(language))
                language = Constants.DefaultLanguage;

            try
            {
                var tours = _context.Tours.OrderByDescending(x => x.CreateTime).Skip(offset).Take(count).ToList();

                if (tours.Count == 0)
                    return null;

                List<ApiTour> output = new List<ApiTour>();

                foreach(var t in tours)
                {
                    var descr = t.Descritions.Where(x => x.Lang == language).FirstOrDefault();

                    if (descr == null)
                        continue;

                    string url = t.Image != null ? t.Image.Url : Constants.DefaultTourImageUrl;

                    output.Add(new ApiTour()
                    {
                        Id = t.Id,
                        Price = t.Price,
                        Adult = t.Adults,
                        Child = t.Child,
                        DaysCount = t.DaysCount,
                        Description = descr.Description,
                        Title = descr.Title,
                        Url = Constants.TourOutUrl(t.Id, language),
                        Image = url,
                    });
                }

                if (output.Count == 0)
                    return null;

                return output;

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }
    }
}
