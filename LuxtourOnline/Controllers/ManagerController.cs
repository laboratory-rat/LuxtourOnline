using LuxtourOnline.Models;
using LuxtourOnline.Models.Account;
using LuxtourOnline.Models.Manager;
using LuxtourOnline.Repos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Net;
using LuxtourOnline.Utilites;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "manager, admin")]
    public class ManagerController : BaseAppController
    {
        protected ManagerRepo _repository { get { return new ManagerRepo(); } }

        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        #region Tour

        protected int _defaultCount = 10;

        public ActionResult TourList(int count = 10, int page = 0, int rate = -1, string title = "")
        {
            if (count < 1)
                count = _defaultCount;

            List<ListTourModel> list = new List<ListTourModel>();

            try
            {
                using (var repo = new ManagerRepo())
                {
                    list = repo.GetTourList(_lang, count, page);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("Index");
            }

            PageTourModel model = new PageTourModel(list, count, page);

            return View(model);
        }

        [HttpGet]
        public ActionResult EditTourNew (int? id)
        {
            try
            {
                TourModifyModel model;

                if (id != null && id > 0)
                {
                    Tour data = _context.Tours.Where(x => x.Id == (int)id).FirstOrDefault();

                    if (data != null)
                    {
                        model = new TourModifyModel(data);
                        return View(model);
                    }
                }

                model = new TourModifyModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveTourJson(TourModifyModel model)
        {
            if (!ModelState.IsValid)
            {
                string errors = "";

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        errors += error.ErrorMessage + "\n";
                }

                return Json(new { Result = "error", Data = errors }, JsonRequestBehavior.AllowGet);
            }

            Tour tour;

            if (model.Id < 0)
            {
                var image = ImageMaster.MoveTmpImage(model.Image);
                tour = Tour.Create(model, image, GetCurrentUser(_context));

                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();
            }
            else
            {
                tour = _context.Tours.Where(x => x.Id == model.Id).FirstOrDefault();
                var user = GetCurrentUser(_context);


                if (tour == null)
                    return null;


                if (model.Image.Id != tour.Image.Id)
                {
                    var image = ImageMaster.MoveTmpImage(model.Image);

                    image.Tour = tour;

                    ImageMaster.RemoveImage(tour.Image);

                    tour.Image.Copy(image);
                }

                tour.ModifyData(model, user);

                await _context.SaveChangesAsync();
            }

            return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult EditTour(int id = -1)
        {
            EditTourModel model;

            try
            {
                if (id < 0)
                    model = new EditTourModel();
                else
                {
                    using (var repo = _repository)
                    {
                        model = repo.GetTourEditModel(id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("TourList");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTour(EditTourModel model)
        {
            var files = Request.Files;

            if (model.Id < 0 && model.Image == null)
                ModelState.AddModelError("", "Image is required");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var repo = _repository)
                {
                    await repo.EditTour(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("TourList");
        }

        [HttpGet]
        public ActionResult RemoveTour(int id, string language = "")
        {

            Tour tour = _context.Tours.Where(x => x.Id == id).FirstOrDefault();

            if (tour != null)
            {
                if (string.IsNullOrEmpty(language))
                    language = _lang;

                TourDisplayModel model = new TourDisplayModel(tour, language);

                ViewBag.language = language;

                return View(model);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveTourConfirm(int id)
        {
            try
            {
                Tour tour = _context.Tours.Where(x => x.Id == id && !x.Deleted).FirstOrDefault();

                if (tour == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                if(tour.Orders != null && tour.Orders.Count > 0)
                {
                    tour.Enable = false;
                    tour.Deleted = true;
                }
                else
                {
                    if (tour.Image != null)
                    {
                        ImageMaster.RemoveImage(tour.Image);

                        _context.SiteImages.Remove(tour.Image);
                    }

                    _context.Tours.Remove(tour);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("TourList");
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult DisplayTour(int id, string language = "")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = Constants.DefaultLanguage;
            }

            try
            {
                Tour tour;

                if ((tour = _context.Tours.Where(x => x.Id == id && !x.Deleted).FirstOrDefault()) == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                TourDisplayModel model = new TourDisplayModel(tour, language);
                ViewBag.language = language;

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Hotels

        [HttpGet]
        public ActionResult HotelsList(string lang = "")
        {
            List<ManagerHotelList> model;

            if (!AppConsts.Langs.Contains(lang))
                lang = _lang;

            try
            {
                using (var repo = new ManagerRepo())
                {
                    model = repo.GetHotelList(lang);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveHotel(int id)
        {
            try
            {
                using (var repo = new ManagerRepo())
                {
                    ManagerHotelRemove model = repo.GetRemoveHotelModel(id);

                    if (model == null)
                        return RedirectToAction("HotelsList");

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("HotelsList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveHotel(ManagerHotelRemove model)
        {
            if (model == null)
            {
                return RedirectToAction("HotelsList");
            }

            try
            {
                using (var repo = new ManagerRepo())
                {
                    repo.RemoveHotel(model);
                    await repo.SaveAsync();

                    var user = repo.GetCurrentUser();

                    _logger.Info($"deleted hotel. title: {model.Title}, delete images = {model.DeleteImages}, User: {user.FullName}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return RedirectToAction("HotelsList");
        }

        [HttpGet]
        public ActionResult EditHotel(int id = -1)
        {
            if (id >= 0)
            {
                try
                {
                    using (var c = _context)
                    {
                        if (!c.Hotels.Any(h => h.Id == id))
                            return RedirectToAction("HotelsList");
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(ex);
                    return RedirectToAction("Index");
                }
            }


            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public ActionResult GetHotelEdit(int id)
        {
            ManagerHotelEdit model = null;

            try
            {
                using (var repo = new ManagerRepo())
                {
                    if (id > 0)
                        model = repo.GetHotelEditModel(id);
                    else
                        model = repo.GetHotelEditModel();
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("HotelsList");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditHotel(ManagerHotelEdit model)
        {

            try
            {
                using (var repo = new ManagerRepo())
                {
                    repo.EditHotel(model);
                    await repo.SaveAsync();

                    var user = repo.GetCurrentUser();
                    _logger.Info($"User {user.FullName} created new hotel");

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }


            return Json("success");
        }

        [HttpGet]
        public ActionResult EditHotelNew (int id)
        {
            ViewBag.HotelId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditHotelNew (EditHotelModel model)
        {
            dynamic output = null;

            if (!ModelState.IsValid)
            {
                output = new
                {
                    Result = "error",
                    Data = "Some errors in the form",
                };
            }
            else
            {
                try
                {
                    using (var c = _context)
                    {
                        var user = GetCurrentUser(c);

                        if (model.Id >= 0 )
                        {
                            Hotel hotel = c.Hotels.Where(x => x.Id == model.Id).First();

                            hotel.Title = model.Title;
                            hotel.Avaliable = model.Avaliable;
                            hotel.Rate = model.Rate;
                            hotel.ModifyUser = GetCurrentUser(c);
                            hotel.ModifyDate = DateTime.Now;
                            hotel.Deleted = false;

                            List<SiteImage> Images = new List<SiteImage>();

                            List<SiteImage> ImagesToDelete = new List<SiteImage>();

                            for (var i = 0; i < hotel.Images.Count; i++)
                            {
                                bool get = false;
                                int index = -1;

                                for(var j = 0; j < model.Images.Count; j++)
                                {
                                    if (hotel.Images[i].Title == model.Images[j].Title)
                                    {
                                        get = true;
                                        index = j;
                                        hotel.Images[i].Order = model.Images[j].Order;
                                        break;
                                    }
                                }

                                if (!get)
                                {
                                    ImagesToDelete.Add(hotel.Images[i]);
                                }
                                else
                                {
                                    model.Images.RemoveAt(index);
                                }
                            }

                            while(ImagesToDelete.Count > 0)
                            {
                                hotel.Images.Remove(ImagesToDelete[0]);
                            }

                            foreach(var img in model.Images)
                            {
                                string title = img.Title;
                                string path = img.Path;
                                int order = img.Order;

                                var NewImage = MoveTmpImage(path, title, order);

                                if (NewImage != null)
                                {
                                    NewImage.Hotel = hotel;
                                    hotel.Images.Add(NewImage);
                                }
                            }

                            while(hotel.Descriptions.Count > 0)
                            {
                                c.HotelDescriptions.Remove(hotel.Descriptions[0]);
                            }

                            hotel.Descriptions.Clear();

                            // Set descriptions

                            List<HotelDescription> descriptions = new List<HotelDescription>();

                            foreach (var descr in model.Descriptions)
                            {
                                string lang = descr.Language;
                                string description = descr.Description;

                                List<HotelFeature> Features = new List<HotelFeature>();

                                HotelDescription Description = new HotelDescription()
                                {
                                    Description = description,
                                    Lang = lang,
                                    Hotel = hotel,
                                    Features = Features,
                                };

                                if (descr.Features != null && descr.Features.Count > 0)
                                {
                                    foreach (var feature in descr.Features)
                                    {
                                        string title = feature.Title;
                                        int order = feature.Order;
                                        string featureDescription = feature.Description;
                                        string featureIco = feature.Ico;

                                        List<HotelElement> Free = new List<HotelElement>();
                                        List<HotelElement> Paid = new List<HotelElement>();

                                        HotelFeature NewFeature = new HotelFeature()
                                        {
                                            Title = title,
                                            Description = featureDescription,
                                            Order = order,
                                            Glyph = featureIco,
                                            HotelDescription = Description,
                                        };

                                        if (feature.Free != null && feature.Free.Count > 0)
                                        {
                                            foreach (var ff in feature.Free)
                                            {
                                                var n = new HotelElement()
                                                {
                                                    Title = ff.Title,
                                                    Glyph = ff.Ico,
                                                    Order = ff.Order,
                                                    Feature = NewFeature,
                                                };

                                                Free.Add(n);
                                            }
                                        }

                                        if (feature.Paid != null && feature.Paid.Count > 0)
                                        {
                                            foreach (var ff in feature.Paid)
                                            {
                                                var n = new HotelElement()
                                                {
                                                    Title = ff.Title,
                                                    Glyph = ff.Ico,
                                                    Order = ff.Order,
                                                    Feature = NewFeature,
                                                };

                                                Paid.Add(n);
                                            }
                                        }

                                        NewFeature.Free = Free;
                                        NewFeature.Paid = Paid;

                                        Features.Add(NewFeature);
                                    }
                                }

                                hotel.Descriptions.Add(Description);
                            }

                            c.Hotels.Attach(hotel);
                            c.Entry(hotel).State = System.Data.Entity.EntityState.Modified;
                            await c.SaveChangesAsync();

                            output = new
                            {
                                Result = "success",
                            };
                        }
                        else
                        {
                            Hotel hotel = new Hotel()
                            {
                                Title = model.Title,
                                Rate = model.Rate,
                                Deleted = false,
                                Apartmetns = new List<Apartment>(),
                                Descriptions = new List<HotelDescription>(),
                                Images = new List<SiteImage>(),
                                ModifyDate = null,
                                ModifyUser = user,
                                Orders = new List<Models.Products.Order>(),
                                Rewiews = new List<Review>(),
                                Tags = new List<Tag>(),
                                Avaliable = model.Avaliable,
                                CreatedTime = DateTime.Now,
                            };

                            List<SiteImage> images = new List<SiteImage>();

                            foreach(var img in model.Images)
                            {
                                string title = img.Title;
                                string path = img.Path;
                                int order = img.Order;

                                var NewImage = MoveTmpImage(path, title, order);

                                if (NewImage != null)
                                {
                                    NewImage.Hotel = hotel;
                                    //c.SiteImages.Add(NewImage);
                                    images.Add(NewImage);
                                }
                            }

                            hotel.Images = images;

                            List<HotelDescription> descriptions = new List<HotelDescription>();

                            foreach(var descr in model.Descriptions)
                            {
                                string lang = descr.Language;
                                string description = descr.Description;

                                List<HotelFeature> Features = new List<HotelFeature>();

                                HotelDescription Description = new HotelDescription()
                                {
                                    Description = description,
                                    Lang = lang,
                                    Hotel = hotel,
                                    Features = Features,
                                };

                                if (descr.Features != null && descr.Features.Count > 0)
                                {
                                    foreach(var feature in descr.Features)
                                    {
                                        string title = feature.Title;
                                        int order = feature.Order;
                                        string featureDescription = feature.Description;
                                        string featureIco = feature.Ico;
                                        
                                        List<HotelElement> Free = new List<HotelElement>();
                                        List<HotelElement> Paid = new List<HotelElement>();

                                        HotelFeature NewFeature = new HotelFeature()
                                        {
                                            Title = title,
                                            Description = featureDescription,
                                            Order = order,
                                            Glyph = featureIco,
                                            HotelDescription = Description,
                                        };

                                        if (feature.Free != null && feature.Free.Count > 0)
                                        {
                                            foreach (var ff in feature.Free)
                                            {
                                                var n = new HotelElement()
                                                {
                                                    Title = ff.Title,
                                                    Glyph = ff.Ico,
                                                    Order = ff.Order,
                                                    Feature = NewFeature,
                                                };

                                                Free.Add(n);
                                            }
                                        }

                                        if (feature.Paid != null && feature.Paid.Count > 0)
                                        {
                                            foreach (var ff in feature.Paid)
                                            {
                                                var n = new HotelElement()
                                                {
                                                    Title = ff.Title,
                                                    Glyph = ff.Ico,
                                                    Order = ff.Order,
                                                    Feature = NewFeature,
                                                };

                                                Paid.Add(n);
                                            }
                                        }

                                        NewFeature.Free = Free;
                                        NewFeature.Paid = Paid;

                                        Features.Add(NewFeature);
                                    }
                                }

                                hotel.Descriptions.Add(Description);
                            }

                            c.Hotels.Add(hotel);

                            await c.SaveChangesAsync();

                            output = new
                            {
                                Result = "success",
                            };

                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    output = null;
                }
            }

            if (output == null)
            {
                output = new
                {
                    Result = "error",
                    Data = "Unknow error",
                };

            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditHotelApiGet(int id)
        {
            dynamic output = null;
            if (id > 0)
            {
                try
                {
                    using (var c = _context)
                    {
                        var hotel = c.Hotels.Where(x => x.Id == id).First();

                        List<SiteImage> images = new List<SiteImage>();
                        if (hotel.Images != null && hotel.Images.Count > 0)
                            images = hotel.Images.OrderBy(x => x.Order).ToList();

                        List<dynamic> imagesJson = new List<dynamic>();

                        foreach(var image in images)
                            imagesJson.Add(new { Path = image.Path, Url = image.Url, IsNew = false, Title = image.Title});

                        List<dynamic> descrptions = new List<dynamic>();

                        foreach(var descr in hotel.Descriptions)
                        {
                            var Description = descr.Description;

                            var featuresList = descr.Features.OrderBy(x => x.Order).ToList();

                            List<dynamic> features = new List<dynamic>();

                            foreach(var f in featuresList)
                            {
                                var title = f.Title;
                                var description = f.Description;
                                var ico = f.Glyph;
                                int order = f.Order;

                                var freeList = f.Free.OrderBy(x => x.Order).ToList();
                                var paidList = f.Paid.OrderBy(x => x.Order).ToList();

                                List<dynamic> free = new List<dynamic>();
                                foreach (var ff in freeList)
                                    free.Add(new { Title = ff.Title, Ico = ff.Glyph, Order = ff.Order });

                                List<dynamic> paid = new List<dynamic>();
                                foreach (var ff in paidList)
                                    paid.Add(new { Title = ff.Title, Ico = ff.Glyph, Order = ff.Order });

                                features.Add(
                                    new{
                                        Title = title,
                                        Description = description,
                                        Ico = ico,
                                        Order = order,
                                        Free = free,
                                        Paid = paid,
                                });
                            }

                            descrptions.Add(
                                new {
                                    Description = Description,
                                    Features = features,
                                    Language = descr.Lang,
                                });
                        }

                        dynamic OutHotel = new
                        {
                            Images = imagesJson,
                            Title = hotel.Title,
                            Rate = hotel.Rate,
                            Avaliable = hotel.Avaliable,
                            Descriptions = descrptions,
                        };

                        output = new
                        {
                            Result = "success",
                            Data = OutHotel,
                        };

                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(ex);
                    output = null;
                }
            }

            if (output == null)
                output = new { Result = "error", Data = $"Bad id: {id}" };//.result = "error";

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditApartments(int id)
        {
            ManagerEditApartmentsModel model;

            try
            {
                using (var repo = _repository)
                {
                    model = repo.GetApartments(id);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(model);
        }

        public async Task<ActionResult> EditApartments(ManagerEditApartmentsModel model)
        {
            try
            {
                using (var repo = _repository)
                {
                    repo.EditApartments(model);
                    await repo.SaveAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return Json("success");
        }
        #endregion

        #region Utilites

        protected SiteImage MoveTmpImage(string path, string name, int order)
        {
            string regularPath = "", regularUrl = "";

            try
            {
                if (System.IO.File.Exists(path))
                {
                    regularPath = Path.Combine(Constants.FullImageFolder, name);
                    regularUrl = Constants.FullImageUrl + name;

                    SiteImage image = new SiteImage()
                    {
                        Order = order,
                        Path = regularPath,
                        Url = regularUrl,
                        Title = name,
                    };

                    if (!Directory.Exists(Constants.FullImageFolder))
                    {
                        Directory.CreateDirectory(Constants.FullImageFolder);
                    }

                    System.IO.File.Move(path, regularPath);

                    return image;
                }
                
            }
            catch(Exception ex)
            {
                _logger.Error(ex);

                if (regularPath != "" && System.IO.File.Exists(regularPath))
                {
                    System.IO.File.Delete(regularPath);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult UploadSingleImage()
        {
            var image = Request.Files[0];

            dynamic output = null;

            string imagePath = "", imageUrl = "";
            
            if (!Directory.Exists(Constants.FullTmpImagePath))
            {
                Directory.CreateDirectory(Constants.FullTmpImagePath);
            }

            if (image != null && image.ContentLength > 0)
            {
                string ext = image.FileName.Split('.').LastOrDefault();
                if (!string.IsNullOrEmpty(ext) && Constants.AvliableImageExt.Contains(ext))
                {
                    try
                    {


                        string imageName = $"{IdGenerator.GenerateId()}.{ext}";

                        imagePath = Constants.FullTmpImagePath + imageName;
                        imageUrl = Constants.FullTmpImageUrl + imageName;
                        bool IsNew = true;

                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        image.SaveAs(imagePath);

                        output = new
                        {
                            Result = "success",
                            Data = new
                            {
                                Title = imageName,
                                Url = imageUrl,
                                Path = imagePath,
                                IsNew = IsNew,
                                Order = 0,
                            }
                        };

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        output = null;

                        if (imagePath != "" && System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);
                    }
                }
            }

            if (output == null)
            {
                output = new
                {
                    Result = "error",
                    Data = "No Image or internal error",
                };
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveImage(string imagePath, bool isNew)
        {
            dynamic output = null;

            try
            {
                if (isNew)
                {
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                else
                {
                    using (var c = _context)
                    {
                        var image = c.SiteImages.Where(x => x.Path == imagePath).First();
                        
                        if(image.Path != "" && System.IO.File.Exists(image.Path))
                        {
                            System.IO.File.Delete(image.Path);
                        }

                        c.SiteImages.Remove(image);
                        await c.SaveChangesAsync();
                    }
                }

                output = new { result = "success" };

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                output = null;
            }

            if (output == null)
            {
                output = new
                {
                    result = "error",
                    data = "Internal server error",
                };
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

       

        #endregion Utilites





    }

    public class EditHotelModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public bool Avaliable { get; set; }

        public List<EditHotelImageModel> Images { get; set; } = new List<EditHotelImageModel>();

        [Required]
        public List<EditHotelDescriptionModel> Descriptions { get; set; }
    }

    public class EditHotelImageModel
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
    }

    public class EditHotelDescriptionModel
    {
        public string Description { get; set; }

        [Required]
        public string Language { get; set; }

        public List<EditHotelFeatureModel> Features { get; set; }
    }

    public class EditHotelFeatureModel
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Ico { get; set; }
        public List<EditHotelElementModel> Free { get; set; }
        public List<EditHotelElementModel> Paid { get; set; }

    }
    public class EditHotelElementModel
    {
        [Required]
        public int Order { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Ico { get; set; }

    }
}