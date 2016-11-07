using LuxtourOnline.Utilites;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DateTime? PasswordChangedDate { get; set; } = null;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Сonsideration;

        public string QuickAccessNumber { get; set; } = "";
        public string UserPasswordHash { get; set; } = "";

        [Required]
        public virtual Tour Tour { get; set; } = null;
        [Required]
        public virtual Hotel Hotel { get; set; } = null;
        [Required]
        public virtual Apartment Apartment { get; set; } = null;

        public string FlyOutCity { get; set; } = "";

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Price { get; set; } = 0;
        public string Ip { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Comments { get; set; }

        [Required]
        public virtual List<CustomerData> CustomersData { get; set; } = new List<CustomerData>();

        [Required]
        public virtual List<SiteDocument> Documents { get; set; } = new List<SiteDocument>();

        protected int _quickAccessLength = 10;

        public Order()
        {
            Id = IdGenerator.GenerateId();
            OrderDate = DateTime.Now;
            Ip = LocationMaster.GetIp();
            QuickAccessNumber = IdGenerator.GeneratePin(_quickAccessLength);
        }

        public Order (string id) : this()
        {
            Id = id;
        }



        public bool GeneratePin(out string pin, int length = 8)
        {
            pin = "";

            if (!CanChangePassword())
                return false;

            pin = IdGenerator.GeneratePin(length);
            UserPasswordHash = IdGenerator.Hash(pin);

            PasswordChangedDate = DateTime.Now;
            return true;
        }

        public bool CanChangePassword()
        {
            if (PasswordChangedDate == null || DateTime.Now.Subtract((DateTime)PasswordChangedDate).Hours > 2)
                return true;

            return false;
        }

        public bool ComparePassword(string pass)
        {
            return IdGenerator.Compare(UserPasswordHash, pass);
        }
    }

    public class CustomerData
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public string FullName { get; set; } = "";
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

        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string City { get; set; } = "";
        public bool LoadPassportImages { get; set; } = false;

        public virtual List<PassportImage> PassportImages { get; set; } = new List<PassportImage>();
        public CustomerData(string id)
        {
            Id = id;
        }

        public CustomerData()
        {
            Id = IdGenerator.GenerateId();
        }
    }

    public class PassportImage
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public byte[] Data { get; set; }

        [Required]
        public virtual CustomerData CustomerData { get; set; }


        public PassportImage()
        {
            Id = IdGenerator.GenerateId();
        }

        public PassportImage(string id)
        {
            Id = id;
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
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

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

            City = data.City;
            Email = data.Email;
            Phone = data.Phone;

            Comments = data.Comments;

            FlyOutCity = data.FlyOutCity;

            Location = LocationMaster.GetLocation(data.Ip);

            Customers = new List<CustomerDisplayDataModel>();
            foreach(var c in data.CustomersData)
            {
                Customers.Add(new CustomerDisplayDataModel());
            }

        }
    }

    public class CustomerDisplayDataModel
    {
        public string Id { get; set; }
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

        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public bool LoadPassportImages { get; set; }

        public List<PassportDisplayModel> PassportImages { get; set; } = new List<PassportDisplayModel>();

        public CustomerDisplayDataModel()
        {

        }

        public CustomerDisplayDataModel(CustomerData data) : this()
        {
            Id = data.Id;
            IsChild = data.IsChild;
            Birthday = data.Birthday;
            FullName = data.FullName;
            CountryFrom = data.CountryFrom;
            CountryLive = data.CountryLive;
            PassportData = data.PassportData;
            PassportNumber = data.PassportNumber;
            PassportUntil = data.PassportUntil;
            PassportFrom = data.PassportFrom;

            Email = data.Email;
            Phone = data.Phone;
            City = data.City;

            PassportImages = new List<PassportDisplayModel>();
            foreach (var i in data.PassportImages)
                PassportImages.Add(new PassportDisplayModel(i));
        }
    }

    public class PassportDisplayModel
    {
        public string Id { get; set; }
        public byte[] Data { get; set; }

        public PassportDisplayModel()
        {

        }

        public PassportDisplayModel(PassportImage data)
        {
            Id = data.Id;
            Data = data.Data;
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
        public string Number { get; set; } = "";
        public string Code { get; set; } = "";

        public bool RememberMe { get; set; } = true;

        public InputQuickCode()
        {

        }
    }



}