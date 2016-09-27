using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class Review
    {
        public int Id { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}