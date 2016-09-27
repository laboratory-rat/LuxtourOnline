using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;

namespace LuxtourOnline.Models
{
    public class SiteImage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Title { get; set; }

        public string Alt { get; set; } = "";

        public string Description { get; set; } = "";

        public string Url { get; set; } = "";

        public string Path { get; set; } = "";

       
        public virtual ICollection<Tour> Tours { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Aparment> Aparments { get; set; }

    }
}