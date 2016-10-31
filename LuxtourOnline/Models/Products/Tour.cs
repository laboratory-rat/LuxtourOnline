using LuxtourOnline.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models
{
    public class Tour
    {
        public int Id { get; set; }

        [Required]
        public virtual SiteImage Image { get; set; }

        [Required]
        public bool Enable { get; set; } = true;

        [Required]
        public decimal Price { get; set; }

        public int? DaysCount { get; set; } = null;
        public int? Adults { get; set; } = null;
        public int? Child { get; set; } = null;

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<TourDescription> Descritions { get; set; }

        [Required]
        [Display(Name = "Created at ")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "Modified at ")]
        public DateTime? ModifyDate { get; set; }

        [Required]
        [Display(Name = "Last modified by ")]
        public virtual AppUser ModifiedBy { get; set; }

        public virtual TopTour TopTour { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        [Required]
        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public Tour(SiteImage image)
        {
            Image = image;
        }

        public Tour()
        {
            CreateTime = DateTime.Now;
        }

    }

    public class TourDescription
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Localization")]
        [StringLength(2, MinimumLength = 2)]
        public string Lang { get; set; }

        
        [Display(Name = "Title")]
        public string Title { get; set; }

        
        [Display(Name = "Description")]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Description { get; set; }

        public virtual Tour ConnectedTour { get; set; }

        public TourDescription(Tour tour)
        {
            ConnectedTour = tour;
        }

        public TourDescription() { }
        public TourDescription(string lang) { Lang = lang; }
    }
}