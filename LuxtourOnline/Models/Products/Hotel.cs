using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool Avaliable { get; set; } = false;

        public virtual List<HotelDescription> Descriptions { get; set; } = new List<HotelDescription>();

        public virtual List<SiteImage> Images { get; set; } = new List<SiteImage>();

        public virtual List<Tag> Tags { get; set; }

        public virtual List<Apartment> Apartmetns { get; set; }

        public virtual List<Review> Rewiews { get; set; }

        [Required]
        [Range(1d, 5d, ErrorMessage = "Must be 1 - 5")]
        public int Rate { get; set; }

        public AppUser ModifyUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifyDate { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public Hotel()
        {

        }
    }


    public class HotelDescription
    {
        public int Id { get; set; }
        public string Lang { get; set; }

        [Required]
        public virtual Hotel Hotel { get; set; }
        public string Description { get; set; }

        public virtual List<HotelFeature> Features { get; set; } = new List<HotelFeature>();
        public HotelDescription(string lang)
        {
            Lang = lang;
        }

        public HotelDescription()
        {

        }

        public static List<HotelDescription> CreateList()
        {
            return new List<HotelDescription>() { new HotelDescription("en"), new HotelDescription("uk"), new HotelDescription("ru") };
        }
    }

    public class HotelFeature
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Glyph { get; set; }

        public virtual List<HotelElement> Free { get; set; } = new List<HotelElement>();
        public virtual List<HotelElement> Paid { get; set; } = new List<HotelElement>();

        [Required]
        public virtual HotelDescription HotelDescription { get; set; }

        public HotelFeature()
        {

        }
    }

    public class HotelElement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Glyph { get; set; }

        [Required]
        public virtual HotelFeature Feature { get; set; }

        public HotelElement()
        {

        }
    }
}
