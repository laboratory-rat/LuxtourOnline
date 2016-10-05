using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models.Manager
{
    #region Tours
    public class EditTourModel
    {
        public int Id { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; } = 1000;

        [Display(Name = "Adults count")]
        public int? Adult { get; set; }

        public int? Child { get; set; }

        public int? DaysCount { get; set; }

        [Required]
        public bool Avalible { get; set; } = false;

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; } = "I'm comment";

        [Required]
        public string TitleEn { get; set; } = "New hotel";

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string DescriptionEn { get; set; } = "Some description";

        [Required]
        public string TitleUk { get; set; } = "Я зоголовок";

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string DescriptionUk { get; set; } = "Трохи опису";

        [Required]
        public string TitleRu { get; set; } = "Я заголовок";

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string DescriptionRu { get; set; } = "Немного описания";

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [DataType(DataType.ImageUrl)]
        public string CurrentImageUrl { get; set; }

        public EditTourModel()
        {

        }

    }

    public class RemoveTourModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public AppUser ModifyUser { get; set; }

        public string ImageUrl { get; set; }

        public bool Avaliable { get; set; }
        public decimal Price { get; set; }

        public RemoveTourModel()
        {

        }

        public RemoveTourModel(Tour tour)
        {
            Id = tour.Id;

        }

    }
    public class DisplayTourModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
        public int? DaysCount { get; set; }
        public bool Avalible { get; set; }
        public string ImageUrl { get; set; }

        public AppUser ModifyUser { get; set; }
        public DateTime? ModifyData { get; set; }
        public DateTime CreateData { get; set; }

        public DisplayTourModel()
        {

        }

        public DisplayTourModel(Tour tour, string lang)
        {
            Id = tour.Id;
            Price = tour.Price;
            Adult = tour.Adults;
            Child = tour.Child;
            DaysCount = tour.DaysCount;

            Avalible = tour.Enable;

            Title = tour.Descritions.Where(d => d.Lang == lang).FirstOrDefault().Title;
            Title = tour.Descritions.Where(d => d.Lang == lang).FirstOrDefault().Description;

            ImageUrl = tour.Image.Url;

            ModifyUser = tour.ModifiedBy;
            ModifyData = tour.ModifyDate;
            CreateData = tour.CreateTime;
        }
    }

    public class PageTourModel
    {
        public List<ListTourModel> Tours { get; set; }
        public PagingInfo Paging { get; set; }

        public PageTourModel()
        {

        }

        public PageTourModel(List<ListTourModel> list, int elements, int page)
        {
            Tours = list;

            Paging = new PagingInfo() { CurrentPange = page, ItemsPerPage = elements, TotalItems = list.Count };
        }
    }

    public class ListTourModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
        public int? DaysCount { get; set; }

        public DateTime? ModifyData { get; set; }
        public AppUser ModifyUser { get; set; }
        public DateTime CreateData { get; set; }


        public ListTourModel(Tour tour, string lang)
        {
            Id = tour.Id;
            Price = tour.Price;
            Adult = tour.Adults;
            Child = tour.Child;
            DaysCount = tour.DaysCount;

            if (tour.Descritions.Count > 0)
                Title = tour.Descritions.Where(d => d.Lang == lang).FirstOrDefault().Title;

            ImageUrl = tour.Image.Url;

            ModifyUser = tour.ModifiedBy;
            ModifyData = tour.ModifyDate;
            CreateData = tour.CreateTime;
        }

        public static List<ListTourModel> CreateList(List<Tour> tours, string lang)
        {
            List<ListTourModel> list = new List<ListTourModel>();
            foreach (var t in tours)
            {
                list.Add(new ListTourModel(t, lang));
            }

            return list;
        }
    }
    #endregion

    #region Hotels

    #endregion

    #region Apartments

    #endregion

    #region Etc

    #endregion



    public class ManagerEditApartmentsModel
    {
        public int Hotel { get; set; }
        public List<EditApartment> Apartments { get; set; }

        public string Url { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Title { get; set; }

        public ManagerEditApartmentsModel()
        {

        }

        public ManagerEditApartmentsModel(Hotel hotel, List<Apartment> apartments)
        {
            Hotel = hotel.Id;
            Apartments = EditApartment.List(apartments);
        }

        public ManagerEditApartmentsModel(Hotel hotel)
        {
            Hotel = hotel.Id;
            Apartments = EditApartment.List(hotel.Apartmetns.ToList());
        }

    }
    
    public class EditApartment
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int? Adults { get; set; }
        public int? Child { get; set; }
        public bool Enabled { get; set; } = false;

        public EditApartment()
        {

        }

        public static List<EditApartment> List(List<Apartment> aparts)
        {
            List<EditApartment> result = new List<EditApartment>();

            if (aparts != null)
            {
                foreach (var apart in aparts)
                {
                    result.Add(new EditApartment()
                    {
                        Id = apart.Id,
                        Title = apart.Title,
                        Adults = apart.Adult,
                        Child = apart.Child,
                        Enabled = apart.Enabled,
                    });
                }
            }

            return result;
        }
    }

    public class SimpleFeature
    {
        public string Lang { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        public List<SimpleFElement> FreeElements { get; set; } = new List<SimpleFElement>();
        public List<SimpleFElement> PaidElements { get; set; } = new List<SimpleFElement>();

        public SimpleFeature()
        {

        }

        public SimpleFeature(string lang)
        {
            Lang = lang;
        }
    }

    public class SimpleFElement
    {
        public string Title { get; set; }
        public string Image { get; set; }
    }


   



}