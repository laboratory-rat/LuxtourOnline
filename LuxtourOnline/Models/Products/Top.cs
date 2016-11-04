using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Products
{
    public class TopHotel
    {
        public int Id { get; set; }
        [Required]
        public virtual Hotel Hotel { get; set; }
        public decimal Factor { get; set; }
        public DateTime SetDate { get; set; }

        public TopHotel()
        {

        }

        public TopHotel(Hotel h, decimal f)
        {
            Hotel = h;
            Factor = f;
            SetDate = DateTime.Now;
        }
    }

    public class TopTour
    {
        public int Id { get; set; }
        [Required]
        public virtual Tour Tour { get; set; }
        public decimal Factor { get; set; }
        public DateTime? SetDate { get; set; }

        public TopTour()
        {

        }

        public TopTour(Tour t, decimal f)
        {
            Tour = t;
            Factor = f;
            SetDate = DateTime.Now;
        }
    }
}