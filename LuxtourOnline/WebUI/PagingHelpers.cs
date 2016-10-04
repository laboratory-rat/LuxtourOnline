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

                tag.AddCssClass("mdl-button mdl-js-button");

                if (i == pagingInfo.CurrentPange)
                {
                    tag.AddCssClass("mdl-button--primary");
                }

                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString MdlValidation(string message)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder tag = new TagBuilder("span");
            tag.AddCssClass("mdl-textfield__error");
            tag.InnerHtml = message;
            result.Append(tag);

            return MvcHtmlString.Create(result.ToString());
        }
    }
}