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

        public static readonly string[] PdfExtensions = new string[] { "pdf", "doc", "docx"};
        public static bool IsPdf(string ex)
        {
            ex = ex.ToLower();
            return PdfExtensions.Contains(ex);
        }

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

        #region Passport

        public static readonly string PassportFolder = "Content\\UserDocuments";

        public static readonly string PassportBasePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PassportFolder);

        public static readonly string PassportUlr = "Content/UserDocuments";

        public static readonly string FullBasePassportUrl =  $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{PassportUlr}";


        public static readonly string TmpPassportUlr = "Content/TmpDocs";

        public static readonly string TmpPassportFolder = "Content\\TmpDocs";

        public static readonly string TmpPassportBasePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, TmpPassportFolder);

        public static readonly string TmpBasePassportUrl = $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{TmpPassportUlr}";

        public static string GeneratePassportPath(string id, string name, string extension) { return GeneratePassportPath(id, name = extension); }
        public static string GeneratePassportPath(string id, string fullName)
        {

            string directory = Path.Combine(PassportBasePath, id);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Path.Combine(directory, fullName);
        }

        public static string GeneratePassportUrl (string id, string fullName)
        {
            return $"{FullBasePassportUrl}/{id}/{fullName}";
        }


        #endregion

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

        public static string PayLink(string id)
        {
            return "http://google.com/" + id;
        }

    }
}