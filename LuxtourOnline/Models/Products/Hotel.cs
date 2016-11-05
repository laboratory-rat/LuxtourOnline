using LuxtourOnline.Models.Products;
using LuxtourOnline.Utilites;
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

        public virtual List<Tag> Tags { get; set; } = new List<Tag>();

        public virtual List<Apartment> Apartments { get; set; } = new List<Apartment>();

        public virtual List<Review> Rewiews { get; set; } = new List<Review>();

        public virtual List<Order> Orders { get; set; } = new List<Order>();

        [Required]
        [Range(1d, 5d, ErrorMessage = "Must be 1 - 5")]
        public int Rate { get; set; }

        public AppUser ModifyUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifyDate { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public virtual TopHotel TopHotel { get; set; }

        public Hotel()
        {

        }

        public Hotel (HotelDisplayModel model, AppUser user)
        {
            Rate = model.Rate;
            Title = model.Title;
            Avaliable = model.Avaliable;
            CreatedTime = DateTime.Now;
            ModifyUser = user;
            Deleted = false;

            ModifyDate = null;

            if (model.Descriptions != null && model.Descriptions.Count > 0)
            {
                foreach (var descr in model.Descriptions)
                    Descriptions.Add(new HotelDescription(descr, this));
            }

            foreach(var image in model.Images)
            {
                Images.Add(ImageMaster.MoveTmpImage(image));
            }
        }

        public void Modify(HotelDisplayModel model, AppUser user)
        {
            Title = model.Title;
            Rate = model.Rate;
            ModifyDate = DateTime.Now;
            ModifyUser = user;
            Avaliable = model.Avaliable;

            if(model.Descriptions != null)
            {
                List<HotelDescription> toRemove = new List<HotelDescription>(Descriptions);

                foreach (var d in model.Descriptions)
                {
                    var selfD = Descriptions.Where(x => x.Id == d.Id).FirstOrDefault();

                    if(selfD == null)
                    {
                        selfD = new HotelDescription(d, this);
                    }
                    else
                    {
                        selfD.Modify(d);
                        toRemove.Remove(selfD);
                    }
                }

                while(toRemove.Count > 0)
                {
                    Descriptions.Remove(toRemove[0]);
                    toRemove.RemoveAt(0);
                }

            }
            else if (Descriptions.Count > 0)
            {
                Descriptions.Clear();
            }

            List<SiteImage> imagesToRemove = new List<SiteImage>(Images);

            foreach(var image in model.Images)
            {
                var self = Images.Where(x => x.Id == image.Id).FirstOrDefault();

                if(self == null)
                {
                    Images.Add(ImageMaster.MoveTmpImage(image));
                }
                else
                {
                    self.Order = image.Order;

                    imagesToRemove.Remove(self);
                }
            }

            while(imagesToRemove.Count > 0)
            {
                Images.Remove(imagesToRemove[0]);
                imagesToRemove.RemoveAt(0);
            }

        }

        internal void UpdateApartments(List<ApartmentDisplayModel> apartments)
        {
            if (Apartments == null)
                Apartments = new List<Apartment>();

            List<Apartment> toRemove = new List<Apartment>(Apartments);

            foreach(var ap in apartments)
            {
                var self = Apartments.Where(x => x.Id == ap.Id).FirstOrDefault();
                {
                    if(self == null)
                    {
                        Apartments.Add(new Apartment(ap, this));
                    }
                    else
                    {
                        self.Modify(ap);
                        toRemove.Remove(self);
                    }
                }
            }

            while(toRemove.Count > 0)
            {
                Apartments.Remove(toRemove[0]);
                toRemove.RemoveAt(0);
            }
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

        public HotelDescription(HotelDescriptionModel model, Hotel hotel)
        {
            Lang = model.Language;
            Description = model.Description;
            Hotel = hotel;

            if (model.Features != null && model.Features.Count > 0)
            {
                foreach (var f in model.Features)
                    Features.Add(new HotelFeature(f, this));
            }
        }

        public void Modify(HotelDescriptionModel model)
        {
            Lang = model.Language;
            Description = model.Description;

            

            if(model.Features != null)
            {
                List<HotelFeature> toRemove = new List<HotelFeature>(Features);

                foreach (var f in model.Features)
                {
                    var selfF = Features.Where(x => x.Id == f.Id).FirstOrDefault();

                    if (selfF == null)
                    {
                        Features.Add(new HotelFeature(f, this));
                    }
                    else
                    {
                        selfF.Modify(f);
                        toRemove.Remove(selfF);
                    }

                }

                while(toRemove.Count > 0)
                {
                    Features.Remove(toRemove[0]);
                    toRemove.RemoveAt(0);
                }
            }
            else if (Features.Count > 0)
            {
                Features.Clear();
            }
        }
    }

    public class HotelFeature
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Glyph { get; set; }

        public int Order { get; set; } = 0;

        public virtual List<HotelElement> Free { get; set; } = new List<HotelElement>();
        public virtual List<HotelElement> Paid { get; set; } = new List<HotelElement>();

        [Required]
        public virtual HotelDescription HotelDescription { get; set; }

        public HotelFeature()
        {

        }

        public HotelFeature(HotelDescriptionFeatureModel model, HotelDescription descrption)
        {
            Title = model.Title;
            Glyph = model.Ico;
            Description = model.Description;
            HotelDescription = descrption;
            Order = model.Order;

            if (model.Free != null && model.Free.Count > 0)
            {
                foreach (var ff in model.Free)
                    Free.Add(new HotelElement(ff, this));
            }

            if (model.Paid != null && model.Paid.Count > 0)
            {
                foreach (var ff in model.Paid)
                    Paid.Add(new HotelElement(ff, this));
            }
        }

        public void Modify(HotelDescriptionFeatureModel model)
        {
            Title = model.Title;
            Glyph = model.Ico;
            Description = model.Description;
            Order = model.Order;

            

            if(model.Free != null)
            {
                List<HotelElement> toRemoveFree = new List<HotelElement>(Free);

                foreach (var ff in model.Free)
                {
                    var self = Free.Where(x => x.Id == ff.Id).FirstOrDefault();

                    if (self == null)
                    {
                        Free.Add(new HotelElement(ff, this));
                    }
                    else
                    {
                        self.Modify(ff);
                        toRemoveFree.Remove(self);
                    }
                }

                while(toRemoveFree.Count > 0)
                {
                    Free.Remove(toRemoveFree[0]);
                    toRemoveFree.RemoveAt(0);
                }

            }
            else if (Free.Count > 0)
            {
                Free.Clear();
            }

            // Paid functions

            if (model.Free != null)
            {
                List<HotelElement> toRemovePaid = new List<HotelElement>(Paid);

                foreach (var ff in model.Paid)
                {
                    var self = Paid.Where(x => x.Id == ff.Id).FirstOrDefault();

                    if (self == null)
                    {
                        Paid.Add(new HotelElement(ff, this));
                    }
                    else
                    {
                        self.Modify(ff);
                        toRemovePaid.Remove(self);
                    }
                }

                while (toRemovePaid.Count > 0)
                {
                    Paid.Remove(toRemovePaid[0]);
                    toRemovePaid.RemoveAt(0);
                }

            }
            else if (Paid.Count > 0)
            {
                Paid.Clear();
            }
        }
    }

    public class HotelElement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Glyph { get; set; }

        public int Order { get; set; } = 0;

        [Required]
        public virtual HotelFeature Feature { get; set; }

        public HotelElement()
        {

        }

        public HotelElement(HotelDescriptionFeatureElementModel model, HotelFeature feature)
        {
            Title = model.Title;
            Glyph = model.Ico;
            Order = model.Order;
            Feature = feature;
        }

        public void Modify(HotelDescriptionFeatureElementModel model)
        {
            Title = model.Title;
            Glyph = model.Ico;
            Order = model.Order;
        }
    }

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
        public int Order { get; set; } = 0;

        [Required]
        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public Apartment()
        {
            Deleted = false;
            Orders = new List<Products.Order>();
        }

        public Apartment(ApartmentDisplayModel model, Hotel hotel) : this()
        {
            Title = model.Title;
            Hotel = hotel;
            Adult = model.Adult;
            Child = model.Child;
            Enabled = model.Enabled;
            
            Order = model.Order;
        }

        public void Modify(ApartmentDisplayModel model)
        {
            Title = model.Title;
            Adult = model.Adult;
            Child = model.Child;
            Enabled = model.Enabled;
            Deleted = model.Deleted;
            Order = model.Order;
        }
    }

    public class ApartmentDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public bool Enabled { get; set; }
        public bool Deleted { get; set; }

        public ApartmentDisplayModel()
        {

        }

        public ApartmentDisplayModel(Apartment data) : this()
        {
            Id = data.Id;
            Title = data.Title;
            Order = data.Order;
            Adult = data.Adult == null ? 0 : (int)data.Adult;
            Child = data.Child == null ? 0 : (int)data.Child;

            Deleted = data.Deleted;

            Enabled = data.Enabled;
        }
    }

    public class HotelDisplayModel
    {
        public int Id { get; set; } = -1;
        public string Title { get; set; } = "";
        public bool Avaliable { get; set; } = true;
        public List<ImageEditModel> Images { get; set; } = new List<ImageEditModel>();
        public int Rate { get; set; } = 5;
        public DateTime CreatedData { get; set; } = DateTime.Now;
        public AppUser ModifyBy { get; set; } = null;
        public DateTime? ModifyDate { get; set; } = null;

        public List<HotelDescriptionModel> Descriptions { get; set; } = new List<HotelDescriptionModel>();

        public HotelDisplayModel()
        {

        }

        public HotelDisplayModel(Hotel data) : this()
        {
            Id = data.Id;
            Title = data.Title;
            Avaliable = data.Avaliable;
            Rate = data.Rate;
            CreatedData = data.CreatedTime;
            ModifyBy = data.ModifyUser;

            if (data.ModifyDate != null)
                ModifyDate = data.ModifyDate;

            if(data.Images != null && data.Images.Count > 0)
            {
                foreach(var image in data.Images)
                {
                    Images.Add(new ImageEditModel(image));
                }
            }

            if(data.Descriptions != null && data.Descriptions.Count > 0)
            {
                foreach(var d in data.Descriptions)
                {
                    Descriptions.Add(new HotelDescriptionModel(d));
                }
            }
            
        }

        public static HotelDisplayModel Create()
        {
            HotelDisplayModel model = new HotelDisplayModel();

            foreach(var l in Constants.AvaliableLangs)
            {
                model.Descriptions.Add(HotelDescriptionModel.Create(l));
            }

            return model;
        }
    }

    public class HotelRemoveModel : HotelDisplayModel
    {
        public HotelDescriptionModel CurrentDescription { get; set; }
        public string Language { get; set; }

        public HotelRemoveModel() : base()
        {

        }

        public HotelRemoveModel(Hotel hotel, string language) : base(hotel)
        {
            language = language.ToLower();
            if (!Constants.AvaliableLangs.Contains(language))
                language = Constants.DefaultLanguage;

            CurrentDescription = Descriptions.Where(x => x.Language == language).FirstOrDefault();
            Language = language;
        }
    }

    public class HotelDescriptionModel
    {
        public int Id { get; set; } = -1;
        public string Description { get; set; } = "";
        public string Language { get; set; } = "";

        public List<HotelDescriptionFeatureModel> Features { get; set; } = new List<HotelDescriptionFeatureModel>();

        public HotelDescriptionModel()
        {

        }

        public HotelDescriptionModel(HotelDescription data) : this()
        {
            Id = data.Id;
            Description = data.Description;
            Language = data.Lang;

            Features = new List<HotelDescriptionFeatureModel>();

            if(data.Features != null && data.Features.Count > 0)
            {
                foreach(var f in data.Features)
                {
                    Features.Add(new HotelDescriptionFeatureModel(f));
                }
            }
        }
        public static HotelDescriptionModel Create (string language)
        {
            HotelDescriptionModel model = new HotelDescriptionModel() { Language = language };
            model.Features.Add(HotelDescriptionFeatureModel.Create());

            return model;
        }
    }

    public class HotelDescriptionFeatureModel
    {
        public int Id { get; set; } = -1;
        public string Ico { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Order { get; set; } = 0;
        public List<HotelDescriptionFeatureElementModel> Free { get; set; } = new List<HotelDescriptionFeatureElementModel>();
        public List<HotelDescriptionFeatureElementModel> Paid { get; set; } = new List<HotelDescriptionFeatureElementModel>();

        public HotelDescriptionFeatureModel()
        {

        }

        public HotelDescriptionFeatureModel(HotelFeature data) : this()
        {
            Id = data.Id;
            Ico = data.Glyph;
            Title = data.Title;
            Description = data.Description;

            Order = data.Order;

            Free = new List<HotelDescriptionFeatureElementModel>();

            if (data.Free != null && data.Free.Count > 0)
            {
                foreach (var el in data.Free)
                {
                    Free.Add(new HotelDescriptionFeatureElementModel(el));
                }
            }

            Paid = new List<HotelDescriptionFeatureElementModel>();

            if (data.Paid != null && data.Paid.Count > 0)
            {
                foreach (var el in data.Paid)
                {
                    Paid.Add(new HotelDescriptionFeatureElementModel(el));
                }
            }
        }

        public static HotelDescriptionFeatureModel Create()
        {
            HotelDescriptionFeatureModel model = new HotelDescriptionFeatureModel();
            model.Free.Add(new HotelDescriptionFeatureElementModel());
            model.Paid.Add(new HotelDescriptionFeatureElementModel());

            return model;
        }

    }

    public class HotelDescriptionFeatureElementModel
    {
        public int Id { get; set; } = -1;
        public string Ico { get; set; } = "";
        public string Title { get; set; } = "";

        public int Order { get; set; } = 0;

        public HotelDescriptionFeatureElementModel()
        {

        }

        public HotelDescriptionFeatureElementModel(HotelElement data) : this()
        {
            Id = data.Id;
            Ico = data.Glyph;
            Order = data.Order;
            Title = data.Title;
        }
    }

    public class HotelDisplayList
    {
        public List<HotelDisplayShort> Hotels = new List<HotelDisplayShort>();
        public PagingInfo Paging { get; set; }

        public HotelDisplayList()
        {

        }

        public HotelDisplayList(List<Hotel> hotels, int page, int perPage)
        {
            foreach(var data in hotels)
            {
                Hotels.Add(new HotelDisplayShort(data));
            }

            Paging = new PagingInfo() { CurrentPange = page, ItemsPerPage = perPage, TotalItems = Hotels.Count };
        }
    }

    public class HotelDisplayShort
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedTime { get; set; }
        public AppUser ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Title { get; set; }
        public bool Avaliable { get; set; }

        public ImageEditModel Image { get; set; }

        public HotelDisplayShort()
        {

        }

        public HotelDisplayShort(Hotel data) : this()
        {
            Id = data.Id;
            Title = data.Title;
            Rate = data.Rate;
            Avaliable = data.Avaliable;
            CreatedTime = data.CreatedTime;
            ModifyBy = data.ModifyUser;
            ModifyDate = data.ModifyDate;

            if(data.Images != null && data.Images.Count > 0)
            {
                Image = new ImageEditModel(data.Images[0]);
            }
            else
            {
                Image = new ImageEditModel() { Url = "http://placehold.it/500x500" };
            }
        }
    }

    public class ApartmentEditModel
    {
        public HotelDisplayShort Hotel { get; set; }
        public List<ApartmentDisplayModel> Apartments { get; set; } = new List<ApartmentDisplayModel>();

        public ApartmentEditModel()
        {

        }

        public ApartmentEditModel(Hotel hotel) : this()
        {
            Hotel = new HotelDisplayShort(hotel);

            Apartments = new List<ApartmentDisplayModel>();

            foreach(var ap in hotel.Apartments)
            {
                Apartments.Add(new ApartmentDisplayModel(ap));
            }
        }
    }
}
