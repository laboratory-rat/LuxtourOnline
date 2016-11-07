using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Models.Products
{
    public class SiteDocument
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string  Url { get; set; }
        public string Path { get; set; } = "";
        public string Extension { get; set; } = "";

        public virtual Order Order { get; set; }

        public SiteDocument()
        {

        }

        public SiteDocument(string name, string path, string url, string ext, Order order) : this()
        {
            Name = name;
            Path = path;
            Extension = ext;
            Url = url;
            Order = order;
        }
    }

    public class SiteDocumentDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }

        public SiteDocumentDisplayModel()
        {

        }

        public SiteDocumentDisplayModel(SiteDocument data) : this()
        {
            Id = data.Id;
            Name = data.Name;
            Extension = data.Extension;
            Path = data.Path;
            Url = data.Url;
        }
    }

}