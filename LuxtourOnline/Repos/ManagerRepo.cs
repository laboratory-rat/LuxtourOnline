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
using Microsoft.AspNet.Identity.Owin;

namespace LuxtourOnline.Repos
{
    public class ManagerRepo : BaseRepo, IDisposable
    {

        public readonly string[] Extensions = { ".png", ".jpg", ".jpeg" };



        public ManagerRepo() { }
        public ManagerRepo(SiteDbContext context)
        {
            
        }

        #region Tour

        public List<ListTourModel> GetTourList (string lang, int count = 10, int page = 1, string title = "")
        {
            var tours = _context.Tours.OrderByDescending(t => t.CreateTime).ToList();

            if (!string.IsNullOrEmpty(title))
                tours = tours.Where(t => t.Descritions.Where(d => d.Lang == lang).FirstOrDefault().Title.ToLower().Contains(title.ToLower())).ToList();

            tours = tours.Skip((page - 1) * count).Take(count).ToList();

            

            return ListTourModel.CreateList(tours, lang);
        }

        public EditTourModel GetTourEditModel(int id)
        {
            var tour = _context.Tours.Where(x => x.Id == id).FirstOrDefault();
            if (tour == null)
                throw new ArgumentNullException($"No tour with id: {id}");

            EditTourModel model = new EditTourModel()
            {
                Id = tour.Id,
                Adult = tour.Adults,
                Child = tour.Child,
                Avalible = tour.Enable,
                Comment = "",
                CurrentImageUrl = tour.Image.Url,
                DaysCount = tour.DaysCount,
                Image = null,
                Price = tour.Price,
            };

            var dEn = tour.Descritions.Where(x => x.Lang == "en").First();
            model.DescriptionEn = dEn.Description;
            model.TitleEn = dEn.Title;

            var dUk = tour.Descritions.Where(x => x.Lang == "uk").First();
            model.DescriptionUk = dUk.Description;
            model.TitleUk = dUk.Title;

            var dRu = tour.Descritions.Where(x => x.Lang == "ru").First();
            model.DescriptionRu = dRu.Description;
            model.TitleRu = dRu.Title;

            return model;

            //throw new NotImplementedException();
        }

        public async Task EditTour(EditTourModel model)
        {
            Tour tour;

            if ((tour = _context.Tours.Where(x => x.Id == model.Id).FirstOrDefault()) != null)
            {
                while (tour.Descritions.Count > 0)
                {
                    var descr = tour.Descritions.FirstOrDefault();

                    _context.TourDescrptions.Attach(descr);
                    _context.TourDescrptions.Remove(descr);
                    await _context.SaveChangesAsync();
                }

                if (model.Image != null && tour.Image != null)
                    DeleteImage(tour.Image);
            }
            else
                tour = new Tour();

            var dEn = new TourDescription() { Lang = "en", ConnectedTour = tour, Description = model.DescriptionEn, Title = model.TitleEn };
            //_context.TourDescrptions.Add(dEn);

            var dUk = new TourDescription() { Lang = "uk", ConnectedTour = tour, Description = model.DescriptionUk, Title = model.TitleUk };
            //_context.TourDescrptions.Add(dUa);

            var dRu = new TourDescription() { Lang = "ru", ConnectedTour = tour, Description = model.DescriptionRu, Title = model.TitleRu };
            //_context.TourDescrptions.Add(dRu);

            tour.Descritions = new List<TourDescription>();
            tour.Descritions.Add(dEn);
            tour.Descritions.Add(dUk);
            tour.Descritions.Add(dRu);

            if (model.Image != null)
            {
                var image = CreateImage(model.Image);
                image.Tour = tour;

                _context.SiteImages.Add(image);

                tour.Image = image;
            }

            if (tour.CreateTime == DateTime.MinValue)
                tour.CreateTime = DateTime.Now;
            else
                tour.ModifyDate = DateTime.Now;

            var user = GetCurrentUser();

            tour.ModifiedBy = user;

            _context.Tours.Add(tour);

            await _context.SaveChangesAsync();
        }

        internal void RemoveTour(RemoveTourModel model)
        {
            var tour = _context.Tours.Where(x => x.Id == model.Id).First();
            tour.ModifiedBy = null;

            while (tour.Descritions.Count > 0)
            {
                var descr = tour.Descritions.First();
                _context.TourDescrptions.Remove(descr);
            }

            _context.Tours.Remove(tour);
        }

        public RemoveTourModel GetRemoveTourModel(int id, string lang)
        {


            var tour = _context.Tours.Where(x => x.Id == id).First();
            var description = tour.Descritions.Where(x => x.Lang == lang).First();

            RemoveTourModel model = new RemoveTourModel()
            {
                Id = tour.Id,
                Avaliable = tour.Enable,
                CreatedTime = tour.CreateTime,
                Description = description.Description,
                Title = description.Title,
                ImageUrl = tour.Image.Url,
                ModifyTime = tour.ModifyDate,
                ModifyUser = tour.ModifiedBy,
                Price = tour.Price,
            };

            return model;
        }

        #endregion Tour

        #region Hotel
        public List<ManagerHotelList> GetHotelList(string lang)
        {
            var list = new List<ManagerHotelList>();

            var hotels = _context.Hotels.OrderByDescending(h => h.CreatedTime).ToList();

            
            

            foreach (var hotel in hotels)
            {
                string Url = "";
                if (hotel.Images.Count > 0 && hotel.Images[0] != null)
                    Url = hotel.Images[0].Url;


                ManagerHotelList element = new ManagerHotelList()
                {
                    Avaliable = hotel.Avaliable,
                    CreationDate = hotel.CreatedTime,
                    ModifyUser = hotel.ModifyUser,
                    UserName = "tmp_user", //hotel.ModifyUser.FullName,
                    ModifyDate = hotel.ModifyDate,
                    ImageUrl = Url,
                    Id = hotel.Id,
                    Rate = hotel.Rate,
                    Title = hotel.Title
                };



                var descr = hotel.Descriptions.Where(x => x.Lang == lang).FirstOrDefault();

                if (descr == null)
                {
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

        internal void RemoveHotel(ManagerHotelRemove model)
        {
            var hotel = _context.Hotels.Where(x => x.Id == model.Id).FirstOrDefault();

            if (hotel != null)
            {
                if (model.DeleteImages)
                    DeleteImage(hotel.Images.ToList());

                _context.Hotels.Remove(hotel);
            }
            else
            {
            }
            //throw new NotImplementedException();
        }

        public void CreateHotel(ManagerHotel model)
        {
            Hotel hotel = new Hotel();

            hotel.Images = new List<SiteImage>();
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

                        _context.SiteImages.Add(i);
                        hotel.Images.Add(i);
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

        }

        internal void EditHotel(ManagerHotelEdit model)
        {
            Hotel hotel = null;

            if (model.Id > 0 && _context.Hotels.Any(h => h.Id == model.Id))
            {
                hotel = _context.Hotels.Where(h => h.Id == model.Id).FirstOrDefault();

                while (hotel.Descriptions.Count > 0)
                {
                    _context.HotelDescriptions.Remove(hotel.Descriptions[0]);
                }
                //hotel.Descriptions.Clear();

                //_context.Entry(hotel).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }

            if (hotel == null)
                hotel = new Hotel();

            hotel.Rate = model.Rate;
            hotel.Title = model.Title;
            hotel.Avaliable = model.Avaliable;

            if (hotel.Images.Count > 0)
            {
                for (int ii = 0; ii < hotel.Images.Count; ii++)
                {
                    bool f = false;

                    foreach (var i in model.Images.Where(x => !x.New))
                    {
                        if (hotel.Images[ii].Id == i.Id)
                        {
                            f = true;
                            break;
                        }
                    }

                    if (!f)
                    {
                        DeleteImage(hotel.Images[ii]);
                    }
                }
            }

            foreach (var image in model.Images.Where(x => x.New))
            {
                var i = MoveTmpImage(image.Url);

                if (i != null)
                {
                    i.Hotel = hotel;
                    hotel.Images.Add(i);
                }
            }

            //Image order

            for (int i = 0; i < model.Images.Count; i++)
            {
                int id = model.Images[i].Id;

                foreach (var image in hotel.Images)
                {
                    if (image.Id == id)
                    {
                        image.Order = i;
                        break;
                    }
                }
            }

            // End Image order

            if (hotel.Descriptions.Count > 0)
            {
                hotel.Descriptions.Clear();
            }

            foreach (var descr in model.Descriptions)
            {
                HotelDescription description = new HotelDescription(descr.Lang);
                FillDescription(descr, hotel, ref description);

                hotel.Descriptions.Add(description);
            }

            if (hotel.CreatedTime == DateTime.MinValue)
                hotel.CreatedTime = DateTime.Now;
            else
                hotel.ModifyDate = DateTime.Now;

            hotel.ModifyUser = GetCurrentUser();

            if (model.Id <= 0)
            {
                _context.Hotels.Add(hotel);
            }
            else
            {
                _context.Entry(hotel).State = System.Data.Entity.EntityState.Modified;
            }

        }

        public ManagerHotelEdit GetHotelEditModel(int Id)
        {
            var hotel = _context.Hotels.Where(h => h.Id == Id).FirstOrDefault();

            //hotel.Gallery = hotel.Gallery.OrderBy(x => x.Order).ToList();

            ManagerHotelEdit model = null;

            if (hotel != null)
            {
                model = new ManagerHotelEdit(hotel);
            }

            return model;
        }

        public ManagerHotelEdit GetHotelEditModel()
        {
            ManagerHotelEdit model = new ManagerHotelEdit();

            model.Title = "New hotel";
            model.Rate = 5;
            model.Avaliable = false;

            foreach (var l in Constants.AvaliableLangs)
            {
                var desrc = new ManagerHotelDescription() { Lang = l };

                var feature = new ManagerHotelFeature();

                feature.Id = AppRandom.RandomString(10);

                feature.Paid.Add(new ManagerHotelElement() { Glyph = "ok", Id = -1, Title = "I am element" });
                feature.Free.Add(new ManagerHotelElement() { Glyph = "eye-open", Id = -1, Title = "I am element" });

                desrc.Features.Add(feature);

                model.Descriptions.Add(desrc);
            }

            return model;
        }

        protected void FillDescription(ManagerHotelDescription model, Hotel hotel, ref HotelDescription description)
        {
            description.Description = model.Description;
            description.Hotel = hotel;

            foreach (var f in model.Features)
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
        #endregion

        #region Images
        public HotelImage UploadImage(HttpPostedFileBase image)
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

            var path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\TmpImages\\", full);
            var url = Path.Combine(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "/Content/TmpImages/", full);


            image.SaveAs(path);

            return new HotelImage { New = true, Path = path, Url = url, Id = new Random().Next(0, 10000) };
            //throw new NotImplementedException();
        }

        public void DeleteImage(List<SiteImage> images)
        {
            for (int i = 0; i < images.Count; i++)
                DeleteImage(images[i]);
        }

        public void DeleteImage(SiteImage image)
        {
            var path = image.Path;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.SiteImages.Remove(image);
        }

        public SiteImage MoveTmpImage(string url)
        {
            string path = Path.Combine(_basePath, url.TrimStart('/').Replace('/', '\\'));
            string name = url.Split('/').LastOrDefault();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
                return null;

            string newPath = Path.Combine(_basePath, "Content\\SystemImages\\", name);

            if (File.Exists(path))
            {
                File.Move(path, newPath);
            }

            return new SiteImage { Alt = "", Path = newPath, Url = "/Content/SystemImages/" +  name, Title = name };
        }

        public SiteImage CreateImage(HttpPostedFileBase file)
        {
            if (file.ContentLength <= 0)
            {
                throw new IOException("file size iz zero");
            }

            var ex = Path.GetExtension(file.FileName);

            if (!Extensions.Contains(ex))
                throw new FormatException("Bad file extension");

            var name = AppRandom.RandomString(15);

            var full = name + ex;

            var path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\SystemImages\\", full);
            var url = Path.Combine(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "/Content/SystemImages/", full);


            file.SaveAs(path);

            return new SiteImage { Path = path, Url = url };
        }
        #endregion



        #region Users




        #endregion Users





    }
}