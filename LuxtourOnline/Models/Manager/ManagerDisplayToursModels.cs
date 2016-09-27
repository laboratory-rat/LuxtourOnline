using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Manager
{
    public class ManagerDisplayTourModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Avalible { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Display (Name = "Created at")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last modify date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified by")]
        public string ModifiedUser { get; set; }
    }
}