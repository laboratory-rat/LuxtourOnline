using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models.Manager
{
    public class ManagerHotelEdit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Avaliable { get; set; }
        public int  Rate { get; set; }

        public List<HotelImage> Images { get; set; } = new List<HotelImage>();
        public List<ManagerHotelDescription> Descriptions { get; set; } = new List<ManagerHotelDescription>();

        public DateTime CreatedTime { get; set; }
        public AppUser ModifyUser { get; set; }
        public DateTime? ModifyTime { get; set; }

        public ManagerHotelEdit()
        {

        }

        public ManagerHotelEdit(Hotel hotel)
        {
            Id = hotel.Id;
            Title = hotel.Title;
            Avaliable = hotel.Avaliable;
            Rate = hotel.Rate;

            CreatedTime = hotel.CreatedTime;
            ModifyTime = hotel.ModifyDate;
            ModifyUser = hotel.ModifyUser;

            Images = new List<HotelImage>();

            foreach(var image in hotel.Gallery)
            {
                Images.Add(new HotelImage(image));
            }

            foreach (var d in hotel.Descriptions)
                Descriptions.Add(new ManagerHotelDescription(d));
        }
    }

    public class HotelImage
    {
        public int Id { get; set; } = -1;
        public string  Url { get; set; }

        public string Path { get; set; }

        public HotelImage()
        {
        }

        public HotelImage(SiteImage image)
        {
            Id = image.Id;
            Url = image.Url;
            Path = image.Path;
        }
    }

    public class ManagerHotel
    {
        public int Id { get; set; } = -1;
        public string Title { get; set; }
        public string[] Images { get; set; }
        public bool Avaliable { get; set; }
        public int Rate { get; set; }
        public ManagerHotelDescription DescriptionEn { get; set; }
        public ManagerHotelDescription DescriptionUk { get; set; }
        public ManagerHotelDescription DescriptionRu { get; set; }

    }

    public class ManagerHotelDescription
    {
        public int Id { get; set; } = -1;
        public string Lang { get; set; } = "";
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public List<ManagerHotelFeature> Features { get; set; } = new List<ManagerHotelFeature>();

        public ManagerHotelDescription()
        {

        }

        public ManagerHotelDescription(HotelDescription desc)
        {
            Id = desc.Id;
            Lang = desc.Lang;
            Description = desc.Description;

            foreach (var f in desc.Features)
                Features.Add(new ManagerHotelFeature(f));
        }
    }

    public class ManagerHotelFeature
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Glyph { get; set; }

        public List<ManagerHotelElement> Free { get; set; } = new List<ManagerHotelElement>();
        public List<ManagerHotelElement> Paid { get; set; } = new List<ManagerHotelElement>();

        public ManagerHotelFeature()
        {

        }

        public ManagerHotelFeature(HotelFeature feat)
        {
            Id = feat.Id.ToString();
            Title = feat.Title;
            Description = feat.Description;
            Glyph = feat.Glyph;

            foreach (var f in feat.Free)
                Free.Add(new ManagerHotelElement(f));

            foreach (var f in feat.Paid)
                Paid.Add(new ManagerHotelElement(f));


        }
    }

    public class ManagerHotelElement
    {
        public int Id { get; set; } = -1;
        public string Title { get; set; }
        public string Glyph { get; set; }

        public ManagerHotelElement()
        {

        }

        public ManagerHotelElement(HotelElement el)
        {
            Id = el.Id;
            Title = el.Title;
            Glyph = el.Glyph;
        }
    }

    public class ManagerHotelList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Avaliable { get; set; }
        public int Rate { get; set; }
        public DateTime CreationDate { get; set; }
        public AppUser ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }

    public class ManagerHotelRemove
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppUser ModifyUser { get; set; }

        [Required]
        public bool DeleteImages { get; set; } = true;

        public ManagerHotelRemove()
        {

        }
    }
}