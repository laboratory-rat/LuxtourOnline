using LuxtourOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.WebUI
{
    public static class ExtensionMethods
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for(int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                tag.AddCssClass("btn");

                if (i == pagingInfo.CurrentPange)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                else
                    tag.AddCssClass("btn-default");

                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}