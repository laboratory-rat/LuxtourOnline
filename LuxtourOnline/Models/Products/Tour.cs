using LuxtourOnline.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public bool TravelAndFood { get; set; } = true;

        [Required]
        public decimal Price { get; set; }

        public int? DaysCount { get; set; } = null;
        public int? Adults { get; set; } = null;
        public int? Child { get; set; } = null;

        public virtual List<Review> Reviews { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<TourDescription> Descritions { get; set; }

        [Required]
        [Display(Name = "Created at ")]
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }

        [Display(Name = "Modified at ")]
        [DataType(DataType.DateTime)]
        public DateTime? ModifyDate { get; set; }

        [Required]
        [Display(Name = "Last modified by ")]
        public virtual AppUser ModifiedBy { get; set; }

        public virtual TopTour TopTour { get; set; } = null;

        [Required]
        public bool Deleted { get; set; } = false;

        [Required]
        public virtual List<Order> Orders { get; set; } = new List<Order>();


        public Tour()
        {
            
        }

        public Tour(SiteImage image) : this()
        {
            Image = image;
        }

        public static Tour Create(TourModifyModel model, SiteImage image, AppUser user)
        {
            var t = new Tour()
            {
                Image = image,
                Adults = model.Adult,
                Child = model.Child,
                DaysCount = model.Days,
                CreateTime = DateTime.Now,
                ModifiedBy = user,
                Deleted = false,
                Enable = model.Enable,
                ModifyDate = null,
                Orders = new List<Order>(),
                Price = model.Price,
                Reviews = new List<Review>(),
                Tags = new List<Tag>(),
                TopTour = null,
                TravelAndFood = model.TravelAndFood,
                Descritions = new List<TourDescription>(),
            };


            foreach (var d in model.Descriptions)
                t.Descritions.Add(TourDescription.Create(d, t));

            return t;
        }

        public void ModifyData (TourModifyModel model, AppUser user)
        {
            ModifyDate = DateTime.Now;
            ModifiedBy = user;

            Enable = model.Enable;
            TravelAndFood = model.TravelAndFood;

            Price = model.Price;
            Adults = model.Adult;
            Child = model.Child;
            DaysCount = model.Days;


            if (Descritions == null)
                Descritions = new List<TourDescription>();

            
            foreach (var d in model.Descriptions)
            {
                var dd = (from descriptions in Descritions where descriptions.Lang == d.Lang select descriptions).FirstOrDefault();

                if (dd != null)
                    dd.ModifyData(d);
                else
                    Descritions.Add(TourDescription.Create(d, this));
            }


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

        [Required]
        public virtual Tour ConnectedTour { get; set; }

        public TourDescription(Tour tour)
        {
            ConnectedTour = tour;
        }

        public TourDescription() { }
        public TourDescription(string lang) { Lang = lang; }

        public void ModifyData(TourModifyDescriptionModel model)
        {
            Title = model.Title;
            Description = model.Description;
        }

        public static TourDescription Create(TourModifyDescriptionModel model, Tour tour)
        {
            return new TourDescription()
            {
                Title = model.Title,
                Description = model.Description,
                ConnectedTour = tour,
                Lang = model.Lang,
            };

        }
    }

    public class TourModifyModel
    {
        public int Id { get; set; } = -1;

        public bool Enable { get; set; } = true;
        public bool TravelAndFood { get; set; } = true;

        public ImageEditModel Image { get; set; } = null;
        public decimal Price { get; set; } = 1000;
        public int Days { get; set; } = 10;
        public int Child { get; set; } = 0;
        public int Adult { get; set; } = 2;

        public List<TourModifyDescriptionModel> Descriptions { get; set; }


        public TourModifyModel()
        {
            Descriptions = new List<TourModifyDescriptionModel>();

            foreach (string l in Constants.AvaliableLangs)
                Descriptions.Add(new TourModifyDescriptionModel(l));
        }

        public TourModifyModel(Tour tour)
        {
            Id = tour.Id;
            Enable = tour.Enable;
            TravelAndFood = tour.TravelAndFood;
            Image = new ImageEditModel(tour.Image);
            Price = tour.Price;

            if (tour.DaysCount != null)
                Days = (int)tour.DaysCount;

            if (tour.Child != null)
                Child = (int)tour.Child;

            if (tour.Adults != null)
                Adult = (int)tour.Adults;


            Descriptions = new List<TourModifyDescriptionModel>();
            foreach (var d in tour.Descritions)
                Descriptions.Add(new TourModifyDescriptionModel(d));

        }

    }

    public class TourModifyDescriptionModel
    {
        public int Id { get; set; }
        public string Lang { get; set; } = "uk";

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public TourModifyDescriptionModel()
        {
        }

        public TourModifyDescriptionModel(string lang): this()
        {
            Lang = lang;
        }

        public TourModifyDescriptionModel( TourDescription model)
        {
            Id = model.Id;
            Lang = model.Lang;
            Title = model.Title;
            Description = model.Description;
        }

    }

    public class ImageEditModel
    {
        public int Id { get; set; } = -1;
        public string Url { get; set; } = "";
        public string Path { get; set; } = "";
        public int Order { get; set; } = 0;

        public string Name { get; set; } = "";
        public string Extension { get; set; } = "";

        public bool IsTmp { get; set; } = true;

        public ImageEditModel()
        {

        }

        public ImageEditModel(SiteImage image) : this()
        {
            Id = image.Id;
            Url = image.Url;
            Path = image.Path;
            Order = image.Order;

            IsTmp = image.IsTmp;

            Name = image.Name;
            Extension = image.Extension;
        }
    }

    public class TourDisplayModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Adult { get; set; } = 0;
        public int Child { get; set; } = 0;
        public int Days { get; set; } = 0;

        public bool Enable { get; set; }
        public bool TravelAndFood { get; set; }

        public ImageEditModel Image { get; set; } = null;

        public DateTime CreatedTime { get; set; }
        public AppUser ModifyBy { get; set; }

        public TourModifyDescriptionModel Description { get; set; } = null;

        public string Lang
        {
            get
            {
                if (Description == null)
                    return Constants.DefaultLanguage;

                return Description.Lang;
            }
        }

        public TourDisplayModel()
        {

        }

        public TourDisplayModel(Tour tour, string language) : this()
        {
            if (!Constants.AvaliableLangs.Contains(language))
                language = Constants.DefaultLanguage;

            Id = tour.Id;
            Price = tour.Price;

            if (tour.Adults != null)
                Adult = (int)tour.Adults;

            if (tour.Child != null)
                Child = (int)tour.Child;

            if (tour.DaysCount != null)
                Days = (int)tour.DaysCount;

            Enable = tour.Enable;
            TravelAndFood = tour.TravelAndFood;

            CreatedTime = tour.CreateTime;
            ModifyBy = tour.ModifiedBy;

            var desc = tour.Descritions.Where(x => x.Lang == language).FirstOrDefault();

            if (desc != null)
                Description = new TourModifyDescriptionModel (desc);

            if(tour.Image != null)
            {
                Image = new ImageEditModel(tour.Image);
            }
        }
    }
}