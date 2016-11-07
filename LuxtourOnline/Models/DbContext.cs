using LuxtourOnline.Models.Products;
using LuxtourOnline.Models.TelGrub;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models
{
    public class SiteDbContext : IdentityDbContext<AppUser>
    {
        public SiteDbContext() : base("SmartAsp")
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourDescription> TourDescrptions { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelDescription> HotelDescriptions { get; set; }
        public DbSet<HotelFeature> Features { get; set; }
        public DbSet<HotelElement> HotelElements { get; set; }
        public DbSet<Apartment> Apartents { get; set; }
        public DbSet<SiteImage> SiteImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Log> Logs { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomerData> CustomerData { get; set; }
        public DbSet<PassportImage>  PassportImages { get; set; }

        public DbSet<Subscribe> Subscriptions { get; set; }
        public DbSet<TopTour> TopTours { get; set; }
        public DbSet<TopHotel> TopHotels { get; set; }

        public DbSet<TelGrubModel> TelGrubs { get; set; }

        public DbSet<SiteDocument> Documents { get; set; }
    }

    


}