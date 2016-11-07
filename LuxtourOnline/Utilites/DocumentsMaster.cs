using LuxtourOnline.Models.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Utilites
{
    public class DocumentsMaster
    {
        public static string BasePath 
        {
            get {
                return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Docs");
            }
        }

        public static string BaseUrl
        {
            get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Docs/"; }
        }

        public static SiteDocument UploadDocument(HttpPostedFileBase doc, Order order)
        {
            if (doc == null || doc.ContentLength < 1)
                return null;

            string ex = doc.FileName.Split('.').LastOrDefault();

            if (string.IsNullOrEmpty(ex) || !Constants.IsPdf(ex))
                return null;

            string id = order.Id;

            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            string path = BasePath + "\\" + id;

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = path + "\\" + id + "\\" + doc.FileName;
            string fileUrl = BaseUrl + "id/" + doc.FileName;
            string name = doc.FileName;

            if (File.Exists(filePath))
                return null;

            doc.SaveAs(filePath);

            return new SiteDocument(name, filePath, fileUrl, ex, order);
        }

        public static void RemoveDocument(SiteDocument doc)
        {
            if(doc != null)
            {
                RemoveDocument(doc.Path);
            }
        }

        public static void RemoveDocument(SiteDocumentDisplayModel doc)
        {
            if(doc != null)
            {
                RemoveDocument(doc.Path);
            }
        }

        public static void RemoveDocument(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

    }
}