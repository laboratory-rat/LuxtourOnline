using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LuxtourOnline
{
    public static class Constants
    {
        public static readonly string[] AvaliableLangs = new string[] { "en", "ru", "uk" };
        public static string DefaultLanguage { get { return AvaliableLangs[2]; } }



        #region Images

        public static readonly string[] AvliableImageExt = new string[] { "jpg", "png", "jpeg" };

        public const string ImageFolder = "Content\\SystemImages\\";

        public static string FullImageFolder
        {
            get { return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ImageFolder); }
        }

        public static string ImageUrl
        {
            get { return ImageFolder.Replace("\\", "/"); }
        }

        public static string FullImageUrl
        {
            get { return $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{ImageUrl}"; }
        }



        public const string ImageTmpFolder = "Content\\TmpImages\\";
        public static string ImageTmpUrl
        {
            get
            {
                return ImageTmpFolder.Replace("\\", "/");
            }
        }

        public static string FullTmpImagePath
        {
            get
            {
                return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ImageTmpFolder);
            }
        }

        public static string FullTmpImageUrl
        {
            get { return $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{ImageTmpUrl}"; }
        }


        public static string OutImageUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}{url}";
        }
        #endregion Images

        #region Tours

        public static string TourOutUrl(int id, string language)
        {
            return $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{language}/Home/Order/{id.ToString()}";
        }

        public static string TourOutUrl(int id)
        {
            return TourOutUrl(id, DefaultLanguage);
        }

        public readonly static string DefaultTourImageUrl = "~/Content/Resources/default_tour.jpg";

        #endregion


    }
}