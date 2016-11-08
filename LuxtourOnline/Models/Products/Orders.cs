using LuxtourOnline.Utilites;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Products
{

    public enum OrderStatus { Null = 0, InProgress, Canceled, Finished, Сonsideration }
    public class Order
    {
        [Key]
        public string Id { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Сonsideration;

        [Required]
        public virtual Tour Tour { get; set; } = null;
        [Required]
        public virtual Hotel Hotel { get; set; } = null;
        [Required]
        public virtual Apartment Apartment { get; set; } = null;

        public bool IsActive { get; set; } = true;

        public string FlyOutCity { get; set; } = "";

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Price { get; set; } = 0;

        public string Comments { get; set; }

        public string Language { get; set; }

        public virtual List<CustomerData> CustomersData { get; set; } = new List<CustomerData>();

        public virtual List<SiteDocument> Documents { get; set; } = new List<SiteDocument>();

        [Required]
        public virtual AppUser User { get; set; }

        public Order()
        {
            Id = IdGenerator.GenerateId();
            OrderDate = DateTime.Now;

            Documents = new List<SiteDocument>();
        }

        public Order(AppUser user) : this()
        {
            User = user;
        }

        public Order (string id) : this()
        {
            Id = id;
        }

        public Order(CreateOrderModel model, Tour tour, Hotel hotel, Apartment apartment, AppUser user, string language) : this(user)
        {
            Date = model.FlyOutDate;
            FlyOutCity = model.FlyOutCity;
            Status = OrderStatus.Сonsideration;
            Comments = model.Comments;

            Tour = tour;
            Hotel = hotel;
            Apartment = apartment;
            Price = Tour.Price;

            Language = language;

            User = user;

            CustomersData = new List<CustomerData>();
            if(model.Customers != null && model.Customers.Count > 0)
            {
                foreach(var c in model.Customers)
                {
                    CustomersData.Add(new CustomerData(c, this));
                }
            }

            
        }
    }

    public class CustomerData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;

        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public bool IsChild { get; set; } = false;

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; } = null;
        public string CountryFrom { get; set; } = "";
        public string CountryLive { get; set; } = "";

        public string PassportData { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string PassportFrom { get; set; } = "";
        [DataType(DataType.Date)]
        public DateTime? PassportUntil { get; set; } = null;

        public bool LoadPassportImages { get; set; } = false;

        public virtual List<PassportImage> PassportImages { get; set; } = new List<PassportImage>();

        public CustomerData(int id)
        {
            Id = id;
        }

        public CustomerData()
        {

        }

        public CustomerData(CustomerDisplayDataModel model, Order order)
        {
            IsChild = model.IsChild;
            Birthday = model.Birthday;
            CountryFrom = model.CountryFrom;
            CountryLive = model.CountryLive;
            PassportData = model.PassportData;
            PassportNumber = model.PassportNumber;
            PassportFrom = model.PassportFrom;
            PassportUntil = model.PassportUntil;

            LoadPassportImages = model.LoadPassportImages;

            Order = order;

            if(model.PassportImages != null && model.PassportImages.Count > 0)
            {
                foreach (var i in model.PassportImages)
                {
                    PassportImages.Add(new PassportImage(i, this, order.User));
                }
            }

        }
    }

    public class PassportImage
    {

        public int  Id { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }

        public virtual CustomerData Customer { get; set; } = null;

        public PassportImage()
        {

        }

        public PassportImage(string name, string ex, string path, string url, CustomerData data) : this()
        {
            Name = name;
            Extension = ex;
            Path = path;
            Url = url;


            Customer = data;
        }

        public PassportImage(PassportImageDisplayModel model, CustomerData data, AppUser user) : this()
        {
            if(model.IsNew)
            {
                string currentPath = model.Path;
                string newPath = Constants.GeneratePassportPath(user.Id, model.Name + model.Extension);

                File.Move(currentPath, newPath);

                string url = Constants.GeneratePassportUrl(user.Id, model.Name + model.Extension);

                model.Path = newPath;
                model.Url = url;
            }

            Name = model.Name;
            Extension = model.Extension;
            Path = model.Path;
            Url = model.Url;

            Customer  = data;
        }

    }
    public class PassportImageDisplayModel
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public bool IsNew { get; set; } = false;

        public PassportImageDisplayModel()
        {

        }

        public PassportImageDisplayModel(string name, string ext, string url, string path) : this()
        {
            Name = name;
            Extension = ext;
            Url = url;
            Path = path;
            Id = -1;
        }

        public PassportImageDisplayModel(PassportImage data)
        {
            Name = data.Name;
            Extension = data.Extension;
            Url = data.Url;
            Path = data.Path;
            Id = data.Id;
        }
    }


    
    public class OrderDisplayModel
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }
        public TourDisplayModel Tour { get; set; }
        public HotelDisplayModel Hotel { get; set; }
        public ApartmentDisplayModel Apartment { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public DateTime OrderDate { get; set; }

        public AppUser User { get; set; }

        public string FlyOutCity { get; set; }


        public LocationModel Location { get; set; }

        public string Comments { get; set; }

        public string Agent { get; set; } = "";

        public List<CustomerDisplayDataModel> Customers { get; set; } = new List<CustomerDisplayDataModel>();

        


        public OrderDisplayModel()
        {

        }

        public OrderDisplayModel(Order data, string language) : this()
        {
            Id = data.Id;
            Status = data.Status;
            Date = data.Date;
            OrderDate = data.OrderDate;

            Tour = new TourDisplayModel(data.Tour, language);
            Hotel = new HotelDisplayModel(data.Hotel);
            Apartment = new ApartmentDisplayModel(data.Apartment);

            Price = data.Price;

            Comments = data.Comments;

            FlyOutCity = data.FlyOutCity;

            User = data.User;

            Customers = new List<CustomerDisplayDataModel>();
            foreach(var c in data.CustomersData)
            {
                Customers.Add(new CustomerDisplayDataModel());
            }

        }
    }

    public class CustomerDisplayDataModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsChild { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        public string CountryFrom { get; set; } = "";
        public string CountryLive { get; set; } = "";

        public string PassportData { get; set; }
        public string PassportNumber { get; set; }
        public string PassportFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PassportUntil { get; set; }

        public bool LoadPassportImages { get; set; }

        public List<PassportImageDisplayModel> PassportImages { get; set; } = new List<PassportImageDisplayModel>();

        public CustomerDisplayDataModel()
        {

        }

        public CustomerDisplayDataModel(CustomerData data) : this()
        {
            Id = data.Id;
            IsChild = data.IsChild;
            Birthday = data.Birthday;
            CountryFrom = data.CountryFrom;
            CountryLive = data.CountryLive;
            PassportData = data.PassportData;
            PassportNumber = data.PassportNumber;
            PassportUntil = data.PassportUntil;
            PassportFrom = data.PassportFrom;

            PassportImages = new List<PassportImageDisplayModel>();
            foreach (var i in data.PassportImages)
                PassportImages.Add(new PassportImageDisplayModel(i));
        }
    }

    public class OrderDisplayListModel
    {
        public List<OrderDisplayModel> Orders { get; set; } = new List<OrderDisplayModel>();
        public PagingInfo Paging { get; set; } = null;

        public OrderDisplayListModel()
        {

        }

        public OrderDisplayListModel(List<Order> data, int page, int perPage, int total, string language) : this()
        {
            foreach(var d in data)
            {
                Orders.Add(new OrderDisplayModel(d, language));
            }

            Paging = new PagingInfo() { CurrentPange = page, ItemsPerPage = perPage, TotalItems = total };
        }
    }

    public class InputQuickCode
    {
        [Required]
        public string Number { get; set; } = "";
        [Required]
        public string Code { get; set; } = "";

        public bool RememberMe { get; set; } = true;

        public InputQuickCode()
        {

        }
    }

    public class OrderUserDisplayModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string PayLink
        {
            get { return "http://google.com/" + Id; }
        }

        public List<CustomerDisplayDataModel> Customers { get; set; } = new List<CustomerDisplayDataModel>();
        public List<SiteDocumentDisplayModel> Documents { get; set; } = new List<SiteDocumentDisplayModel>();

        public string Email { get; set; }
        public string Phone { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; } = OrderStatus.Null;
        public string FlyOutCity { get; set; } = "";
        public TourDisplayModel Tour { get; set; }
        public HotelDisplayModel Hotel { get; set; }
        public ApartmentDisplayModel Aparment { get; set; }

        public decimal Price { get; set; } = 0M;

        public OrderUserDisplayModel()
        {

        }

        public OrderUserDisplayModel(Order data) : this()
        {
            Id = data.Id;

            if(data.CustomersData != null && data.CustomersData.Count > 0)
            {
                foreach (var c in data.CustomersData)
                    Customers.Add(new CustomerDisplayDataModel(c));
            }

            if(data.Documents != null && data.CustomersData.Count > 0)
            {
                foreach(var d in data.Documents)
                {
                    Documents.Add(new SiteDocumentDisplayModel(d));
                }
            }
        }

    }

    public class CreateOrderModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Comments { get; set; }
        public int TourId { get; set; }
        public int HotelId { get; set; }
        public int ApartmentId { get; set; }
        public List<CustomerDisplayDataModel> Customers { get; set; }
        public DateTime FlyOutDate { get; set; }
        public string FlyOutCity { get; set; }

        public CreateOrderModel()
        {

        }
    }

}