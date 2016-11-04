using LuxtourOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LuxtourOnline.Utilites
{
    public class ImageMaster
    {
        public static SiteImage MoveTmpImage(ImageEditModel model)
        {
            SiteImage image = new SiteImage(model);

            if (!System.IO.File.Exists(image.Path))
                return null;



            if (image.Name == "")
                image.Name = image.Path.Split('\\').Last();

            if (image.Extension == "")
                image.Extension = image.Path.Split('.').Last().ToLower();

            string path = Path.Combine(Constants.FullImageFolder, image.Name);
            string url = Constants.FullImageUrl + image.Name;

            System.IO.File.Move(image.Path, path);

            image.Path = path;
            image.Url = url;

            return image;
        }

        public static void RemoveImage(SiteImage image)
        {
            if (image != null && File.Exists(image.Path))
            {
                File.Delete(image.Path);
            }
        }

        public static void RemoveImage(ImageEditModel image)
        {
            if (image != null && File.Exists(image.Path))
                File.Delete(image.Path);
        }
    }
}