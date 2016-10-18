using LuxtourOnline.Utilites;
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

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Сonsideration;

        [Required]
        public virtual Tour Tour { get; set; }
        [Required]
        public virtual Hotel Hotel { get; set; }
        [Required]
        public virtual Apartment Apartment { get; set; }

        public decimal Cost
        {
            get { return Tour.Price; }
        }

        [Required]
        public string City { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Comments { get; set; }

        [Required]
        public virtual List<CustomerData> CustomersData { get; set; } = new List<CustomerData>();

        public Order()
        {
            Id = IdGenerator.GenerateId();
        }

        public Order (string id)
        {
            Id = id;
        }

    }

    public class CustomerData
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public bool IsChild { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string CountryFrom { get; set; }
        public string CountryLive { get; set; }

        public string PassportData { get; set; }
        public string PassportNumber { get; set; }
        public string PassportFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime PassportUntil { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public bool LoadPassportImages { get; set; }
        
        public virtual List<PassportImage> PassportImages { get; set; }
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
}