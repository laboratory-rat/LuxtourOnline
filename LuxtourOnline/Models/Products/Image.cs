using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxtourOnline.Models
{
    public class SiteImage
    {
        
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Alt { get; set; } = "";

        public string Description { get; set; } = "";

        public string Url { get; set; } = "";

        public string Path { get; set; } = "";

        public string Name { get; set; } = "";
        public string Extension { get; set; } = "";

        public int Order { get; set; } = 0;

        public bool IsTmp { get; set; } = false;

        public virtual Tour Tour { get; set; } = null;

        public virtual Hotel Hotel { get; set; } = null;


        public SiteImage()
        {
               
        }

        public SiteImage(ImageEditModel model) : this()
        {
            Url = model.Url;
            IsTmp = model.IsTmp;
            Order = model.Order;
            Name = model.Name;
            Path = model.Path;
            Extension = model.Extension;
        }

        public void Copy(SiteImage model)
        {
            Url = model.Url;
            Path = model.Path;
            Alt = model.Alt;
            Tour = model.Tour;
            Hotel = model.Hotel;
            IsTmp = model.IsTmp;
            Order = model.Order;
            Description = model.Description;

            Name = model.Name;
            Extension = model.Extension;
        }

    }
}