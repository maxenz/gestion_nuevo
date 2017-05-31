using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Paramedic.Gestion.Web.HtmlHelpers
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

            string res = String.Format("<li class=\"{0}\"><a href=\"{1}\"><i class=\"{2}\"></i><span>{3}</a></li>", statusPage, href, icon, title);

            return MvcHtmlString.Create(res);

        }
        public static MvcHtmlString DropDownList(this System.Web.Mvc.HtmlHelper html, string name, SelectList values, object htmlAttributes, bool canEdit)
        {
            if (canEdit)
            {
                return html.DropDownList(name, values, htmlAttributes);
            }

            return html.DropDownList(name, values, new { @class="form-control col-xs-12", disabled = "disabled" });
        }
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, object htmlAttributes, bool canEdit)
        {

            if (canEdit)
            {
                return htmlHelper.EnumDropDownListFor(
                                       expression,
                                       optionLabel,
                                       htmlAttributes);
            }

            return htmlHelper.EnumDropDownListFor(expression, optionLabel, new { @class = "form-control", disabled = "disabled" });

        }
		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType().GetMember(enumValue.ToString())
							  .First()
							  .GetCustomAttribute<DisplayAttribute>()
							  .Name;
		}

	}
}