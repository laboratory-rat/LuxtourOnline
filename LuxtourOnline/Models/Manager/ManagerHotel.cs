using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models.Manager
{
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
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public List<ManagerHotelFeature> Feautures { get; set; }
    }

    public class ManagerHotelFeature
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Glyph { get; set; }

        public List<ManagerHotelElement> Free { get; set; }
        public List<ManagerHotelElement> Paid { get; set; }
    }

    public class ManagerHotelElement
    {
        public int Id { get; set; } = -1;
        public string Title { get; set; }
        public string Glyph { get; set; }
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

        public ManagerHotelRemove()
        {

        }
    }
}