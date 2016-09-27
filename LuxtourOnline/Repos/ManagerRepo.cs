using LuxtourOnline.Models;
using LuxtourOnline.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Identity;
using NLog;

namespace LuxtourOnline.Repos
{
    public class ManagerRepo : IDisposable
    {

        public readonly string[] Extensions = { ".png", ".jpg", ".jpeg" };

        protected SiteDbContext _context;

        protected Logger _log = LogManager.GetCurrentClassLogger();

        protected string _basePath = HttpContext.Current.Request.PhysicalApplicationPath;

        public ManagerRepo() { }
        public ManagerRepo(SiteDbContext context)
        {
            _context = context;
        }

        public List<ListTourModel> GetTourList (string lang, int count = 10, int offcet = 0)
        {
            var tours = _context.Tours.OrderByDescending(t => t.CreateTime).Skip(offcet * count).Take(count).ToList();
            return ListTourModel.CreateList(tours, lang);
        }

        public List<ManagerHotelList> GetHotelList(string lang)
        {
            var list = new List<ManagerHotelList>();

            var hotels = _context.Hotels.OrderByDescending(h => h.CreatedTime).ToList();

            foreach (var hotel in hotels)
            {
                ManagerHotelList element = new ManagerHotelList()
                {
                    Avaliable = hotel.Avaliable,
                    CreationDate = hotel.CreatedTime,
                    ModifyUser = hotel.ModifyUser,
                    ModifyDate = hotel.ModifyDate,
                    Id = hotel.Id,
                    Rate = hotel.Rate,
                    Title = hotel.Title
                };



                var descr = hotel.Descriptions.Where(x => x.Lang == lang).FirstOrDefault();

                if (descr == null)
                {
                    _log.Log(LogLevel.Error, "Can't get description for hotel: " + hotel.Title + ", lang:" + lang);
                    element.Description = "Some errors...";
                }
                else
                {
                    element.Description = descr.Description;
                }

                list.Add(element);
            }

            return list;
        }

        public ManagerHotelRemove GetRemoveHotelModel(int id)
        {
            var hotel = _context.Hotels.Where(x => x.Id == id).FirstOrDefault();

            if (hotel == null)
                return null;

            ManagerHotelRemove model = new ManagerHotelRemove()
            {
                Id = hotel.Id,
                Title = hotel.Title,
                CreatedDate = hotel.CreatedTime,
                ModifyUser = hotel.ModifyUser,
                Rate = hotel.Rate
            };

            return model;
            //throw new NotImplementedException();
        }

        internal void RemoveHotel(int id)
        {
            var hotel = _context.Hotels.Where(x => x.Id == id).FirstOrDefault();

            if (hotel != null)
            {
                if(hotel.Descriptions.Count > 0)
                {
                    for(int i = 0; i < hotel.Descriptions.Count; i++)
                    {
                        var d = hotel.Descriptions[i];

                        for(int j = 0; j < d.Features.Count; j++)
                        {
                            var f = d.Features[j];

                            for (int x = 0; x < f.Free.Count; x++)
                            {
                                _context.HotelElements.Remove(f.Free[x]);
                            }

                            for (int x = 0; x < f.Paid.Count; x++)
                            {
                                _context.HotelElements.Remove(f.Paid[x]);
                            }

                            _context.Features.Remove(f);
                        }

                        _context.HotelDescriptions.Remove(d);
                    }

                }

                _context.Hotels.Remove(hotel);
            }

            //throw new NotImplementedException();
        }

        public void CreateHotel(ManagerHotel model)
        {
            Hotel hotel = new Hotel();

            hotel.Gallery = new List<SiteImage>();
            hotel.Descriptions = new List<HotelDescription>();

            hotel.Rate = model.Rate;
            hotel.Title = model.Title;
            hotel.Avaliable = model.Avaliable;
            
            if (model.Images != null && model.Images.Length > 0)
            {
                foreach (string path in model.Images)
                {
                    var i = MoveTmpImage(path);

                    if (i != null)
                    {
                        hotel.Gallery.Add(i);
                    }
                }
            }
            

            HotelDescription description = new HotelDescription("en");
            FillDescription(model.DescriptionEn, hotel, ref description);

            hotel.Descriptions.Add(description);

            description = new HotelDescription("uk");
            FillDescription(model.DescriptionUk, hotel, ref description);

            hotel.Descriptions.Add(description);

            description = new HotelDescription("ru");
            FillDescription(model.DescriptionRu, hotel, ref description);

            hotel.Descriptions.Add(description);

            hotel.CreatedTime = DateTime.Now;
            hotel.ModifyUser = GetCurrentUser();

            _context.Hotels.Add(hotel);

            _log.Debug<Hotel>(hotel);
        }

       

        protected void FillDescription(ManagerHotelDescription model, Hotel hotel, ref HotelDescription description)
        {
            description.Description = model.Description;
            description.Hotel = hotel;

            foreach (var f in model.Feautures)
            {


                var feature = new HotelFeature() { Description = f.Description, Glyph = f.Glyph, HotelDescription = description, Title = f.Title };

                foreach (var free in f.Free)
                {
                    var element = new HotelElement() { Feature = feature, Title = free.Title, Glyph = free.Glyph };
                    feature.Free.Add(element);
                }

                foreach (var paid in f.Paid)
                {
                    var element = new HotelElement() { Feature = feature, Title = paid.Title, Glyph = paid.Glyph };
                    feature.Paid.Add(element);
                }

                description.Features.Add(feature);
            }

        }

        public string UploadImage(HttpPostedFileBase image)
        {

            if (image.ContentLength <= 0)
            {
                throw new IOException("file size iz zero");
            }

            var ex = Path.GetExtension(image.FileName);

            if (!Extensions.Contains(ex))
                throw new FormatException("Bad file extension");

            var name = AppRandom.RandomString(15);

            var full = name + ex;

            var fullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\TmpImages\\", full);

            image.SaveAs(fullPath);

            return Path.Combine(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority),"/Content/TmpImages/",full);
            //throw new NotImplementedException();
        }


        public SiteImage MoveTmpImage(string url)
        {
            string path = Path.Combine(_basePath, url);
            string name = url.Split('/').LastOrDefault();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
                return null;

            string newPath = Path.Combine(_basePath, "Content/SystemImages/", name);

            if (File.Exists(path))
            {
                File.Move(path, newPath);
            }

            return new SiteImage { Alt = "", Path = newPath, Url = Path.Combine("/Content/SystemImages", name), Title = name };
        }


        public AppUser GetCurrentUser()
        {
            string name = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
            return user;
        }

        public async Task SaveAsync()
        {
            try
            {
                var result = _context.GetValidationErrors();

                if (result.Count() == 0)
                    await _context.SaveChangesAsync();
                else
                {
                    string e = result.ToString();

                    throw new Exception(string.Format("Can't save model: {0}", e));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}