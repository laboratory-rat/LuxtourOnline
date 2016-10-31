using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Products
{
    public class Subscribe
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime SubscribeDate { get; set; }

        public string Language { get; set; }

        public string RemoveSubscribeString { get; set; } = string.Empty;

        protected int RemoveSubscribeLength = 20;

        public Subscribe()
        {
            if (string.IsNullOrEmpty(RemoveSubscribeString))
                RemoveSubscribeString = Utilites.IdGenerator.GenerateId().Substring(0, RemoveSubscribeLength).ToLower();

            SubscribeDate = DateTime.Now;
        }

        public Subscribe(string fullName, string email, string phone, string lang) : this()
        {
            if (!Constants.AvaliableLangs.Contains(lang))
                lang = Constants.DefaultLanguage;

            FullName = fullName;
            Email = email;
            Tel = phone;
            Language = lang;
        }

        public static Subscribe Create(SubscribeAddModel model)
        {
            return new Subscribe(model.FullName, model.Email, model.Tel, model.Language);
        }

        public void RemoveSubscribe()
        {
            IsActive = false;
        }
    }

    public class SubscribeAddModel
    {
        public string FullName { get; set; }
        public string Email { get; set; } = "";
        public string Tel { get; set; } = "";
        public string Language { get; set; }
    }

    public class SubscribeList
    {
        public List<Subscribe> Subscribes { get; set; }
        public PagingInfo Paging { get; set; }
    }
}