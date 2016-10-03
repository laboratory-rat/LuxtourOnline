using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class Review
    {
        public int Id { get; set; }

        public virtual Hotel Hotel { get; set; } = null;
        public virtual Tour Tour { get; set; } = null;
    }
}