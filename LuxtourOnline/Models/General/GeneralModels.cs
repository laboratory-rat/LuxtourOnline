using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class GeneralModels
    {
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPange { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public PagingInfo()
        {

        }
        
    }
}