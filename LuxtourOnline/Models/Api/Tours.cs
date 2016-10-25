using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Api
{
    public class ApiTourShort
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public string Url { get; set; }
    }

    public class ApiTour : ApiTourShort
    {
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int? DaysCount { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
    }
}