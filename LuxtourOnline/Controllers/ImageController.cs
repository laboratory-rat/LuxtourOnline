using LuxtourOnline.Models;
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
        public async Task<ActionResult> SaveImageJson()
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
                    string imageName = $"{IdGenerator.GenerateId()}.{ext}";

                    imagePath = Constants.FullTmpImagePath + imageName;
                    imageUrl = Constants.FullTmpImageUrl + imageName;

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    image.SaveAs(imagePath);

                    SiteImage siteImage = new SiteImage()
                    {
                        Alt = "",
                        IsTmp = true,
                        Description = "",
                        Path = imagePath,
                        Title = "",
                        Url = imageUrl,
                        Name = imageName,
                        Extension = ext.ToLower(),
                    };

                    _context.SiteImages.Add(siteImage);
                    await _context.SaveChangesAsync();

                    output = new
                    {
                        Result = "success",
                        Data = new ImageEditModel(siteImage),
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


    }
}