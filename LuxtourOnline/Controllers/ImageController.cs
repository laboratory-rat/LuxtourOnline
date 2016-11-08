using LuxtourOnline.Models;
using LuxtourOnline.Models.Products;
using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class ImageController : BaseAppController, IDisposable
    {
        // GET: Image
        public ActionResult SaveImageJson()
        {
            if (Request.Files == null || Request.Files.Count == 0 || Request.Files[0] == null || Request.Files[0].ContentLength < 1)
                return null;

            var image = Request.Files[0];

            dynamic output = null;

            string imagePath = "", imageUrl = "";

            if (!Directory.Exists(Constants.FullTmpImagePath))
            {
                Directory.CreateDirectory(Constants.FullTmpImagePath);
            }


            string ext = image.FileName.Split('.').LastOrDefault();

            if (!string.IsNullOrEmpty(ext) && Constants.AvliableImageExt.Contains(ext.ToLower()))
            {
                try
                {

                    string name = IdGenerator.GenerateId();
                    string imageName = $"{name}.{ext}";
                    


                    imagePath = Constants.FullTmpImagePath + imageName;
                    imageUrl = Constants.FullTmpImageUrl + imageName;

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    image.SaveAs(imagePath);

                    var i = new ImageEditModel()
                    {
                        Extension = ext,
                        IsTmp = true,
                        Name = imageName,
                        Order = 0,
                        Path = imagePath,
                        Url = imageUrl,
                    };

                    output = new
                    {
                        Result = "success",
                        Data = i,
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

        public async Task<ActionResult> RemoveImageJson(int id)
        {
            try
            {
                SiteImage image = _context.SiteImages.Where(x => x.Id == id).FirstOrDefault();

                if (image == null)
                    return null;

                if (System.IO.File.Exists(image.Path))
                {
                    System.IO.File.Delete(image.Path);
                }

                _context.SiteImages.Remove(image);
                await _context.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return null;
            }

        }

        [HttpPost]
        public ActionResult RemoveImageJson(ImageEditModel image)
        {
            ImageMaster.RemoveImage(image);

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddPassportJson()
        {
            if (Request.Files == null || Request.Files.Count == 0 || Request.Files[0] == null || Request.Files[0].ContentLength < 1)
                return null;

            var image = Request.Files[0];
            string path = "";

            if (!Directory.Exists(Constants.TmpPassportBasePath))
            {
                Directory.CreateDirectory(Constants.TmpPassportBasePath);
            }


            string ext = image.FileName.Split('.').LastOrDefault();

            if (!string.IsNullOrEmpty(ext) && Constants.AvliableImageExt.Contains(ext.ToLower()))
            {
                try
                {

                    string name = IdGenerator.GenerateId();
                    string imageName = $"{name}{ext}";
                    path = Constants.TmpPassportBasePath + imageName;
                    string url = Constants.TmpBasePassportUrl + imageName;



                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    image.SaveAs(path);

                    var result = new PassportImageDisplayModel(name, ext, url, path);
                    result.IsNew = true;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);

                    if (path != "" && System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RemovePassportJson(PassportImageDisplayModel model)
        {
            if(model.IsNew == true)
            {
                if (!string.IsNullOrEmpty(model.Path) && model.Path.Contains(Constants.TmpPassportFolder))
                {
                    if (System.IO.File.Exists(model.Path))
                        System.IO.File.Delete(model.Path);
                }
            }
            else if(User.Identity.IsAuthenticated && model.Id > -1)
            {
                var pass = _context.PassportImages.Where(x => x.Id == model.Id).FirstOrDefault();

                if(pass != null && pass.Customer.Order.User.Id == GetCurrentUser(_context).Id)
                {
                    _context.PassportImages.Remove(pass);

                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(model.Path))
                        System.IO.File.Delete(model.Path);
                }
            }
            else
            {
                return null;
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

    }
}