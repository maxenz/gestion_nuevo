using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string formatZeroPrice(double input)
        {
            if (input == 0.00)
            {
                return "";
            }
            else
            {
                return input.ToString();
            }

        }

        public static MvcHtmlString getPageLink(string href, string icon, string title)
        {

            string hrefController = href.Remove(0, 1);
            string actual_controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            string statusPage = "";

            if (actual_controller.Contains(hrefController)) statusPage = "active";

            string res = String.Format("<li class=\"{0}\"><a href=\"{1}\"><i class=\"{2}\"></i><span>{3}</a></li>",statusPage,href,icon,title);

            return MvcHtmlString.Create(res);

        }
    }
}