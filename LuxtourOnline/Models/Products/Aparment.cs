using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class Apartment
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Hotel Hotel { get; set; }

        public int? Adult { get; set; } = null;
        public int? Child { get; set; } = null;

        public bool Enabled { get; set; } = true;

        [Required]
        public bool Deleted { get; set; } = false;
    }
}