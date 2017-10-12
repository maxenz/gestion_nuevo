using System.Web;
using System.Web.Optimization;

namespace Gestion
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/libraries_js").Include(
				"~/Scripts/template_scripts/js/jquery-1.7.2.min.js",
				"~/Scripts/template_scripts/js/excanvas.min.js",
				"~/Scripts/template_scripts/js/bootstrap.js",
				"~/Scripts/sbadmin/jquery.fancybox.pack.js",
				"~/Scripts/sbadmin/bootbox.min.js",
				"~/Scripts/sbadmin/bootstrap-select.min.js",
				"~/Scripts/bootstrap-colorpicker.js",
				"~/Scripts/sbadmin/messenger.min.js",
				"~/Scripts/sbadmin/messenger-theme-future.min.js",
				"~/Scripts/template_scripts/js/base.js",
				"~/Scripts/template_scripts/js/jquery.blockUI.js",
				"~/Scripts/jquery-ui.js",
				"~/Scripts/datepicker-es.js",
				"~/Scripts/toastr.js"


			));

			bundles.Add(new ScriptBundle("~/bundles/tabs").Include(
				"~/Scripts/tabs/modernizr.custom.js",
				"~/Scripts/tabs/cbpFWTabs.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/login_js").Include(
					"~/Scripts/template_scripts/js/login.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/general_js").Include(
					"~/Scripts/template_scripts/js/general.js"
			));

			bundles.Add(new StyleBundle("~/bundles/gestion_css").Include(
					"~/Content/template_resources/css/bootstrap.css",
					"~/Content/template_resources/css/tabs/*.css",
					"~/Content/sbadmin/jquery.fancybox.css",
					"~/Content/jquery-ui.css",
					"~/Content/sbadmin/messenger.min.css",
					"~/Content/sbadmin/messenger-theme-future.min.css",
					"~/Content/bootstrap-colorpicker.css",
					"~/Content/template_resources/css/style.css",
					"~/Content/template_resources/css/ct-navbar.css",
					"~/Content/toastr.css"

			));

			bundles.Add(new StyleBundle("~/bundles/signin_css").Include(
					"~/Content/template_resources/css/pages/signin.css"
			));

			bundles.Add(new ScriptBundle("~/bundles/mapa").Include(
						"~/Scripts/sbadmin/map.js"
						));

			bundles.Add(new StyleBundle("~/Content/bootstrap/base").Include(
						"~/Content/styles/flat-ui.css"));
		}
	}
}