﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;
using System.Web.Mvc;

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

        public int Order { get; set; } = 100;

        public virtual Tour Tour { get; set; } = null;
        public virtual Hotel Hotel { get; set; } = null;
        public virtual Aparment Apartment { get; set; } = null;

    }
}