using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using CimenaCityProject.Models;
using CimenaCityProject.ViewModels;

namespace CimenaCityProject.CustomHtmlHelper
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlString Image(this HtmlHelper helper, string src, string alt)
        {
            if (src == null)
            {
                src = "~/Image/TempImage.png";
                alt = "Temp";
               
            }
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("alt", alt);
           

            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

        
    }
}