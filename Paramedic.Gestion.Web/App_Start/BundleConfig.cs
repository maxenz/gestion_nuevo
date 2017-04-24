using System.Web;
using System.Web.Optimization;

namespace Gestion
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/AdminJs").Include(
                  "~/Scripts/sbadmin/jquery-2.0.3.min.js",
                  "~/Scripts/sbadmin/bootstrap.js",
                  "~/Scripts/sbadmin/tablesorter/jquery.tablesorter.js",
                  "~/Scripts/sbadmin/tables.js",
                  "~/Scripts/sbadmin/bootbox.min.js",
                  "~/Scripts/sbadmin/bootstrap-datepicker.js",
                  "~/Scripts/sbadmin/jquery.fancybox.pack.js",
                  "~/Scripts/sbadmin/bootstrap-switch.js",
                  "~/Scripts/sbadmin/bootstrap-select.min.js",
                  "~/Scripts/sbadmin/messenger.min.js",
                  "~/Scripts/sbadmin/messenger-theme-future.min.js",
                  "~/Scripts/sbadmin/general.js"

             ));

            bundles.Add(new ScriptBundle("~/bundles/libraries_js").Include(
                "~/Scripts/template_scripts/js/jquery-1.7.2.min.js",
                "~/Scripts/template_scripts/js/excanvas.min.js",
                "~/Scripts/template_scripts/js/bootstrap.js",
                "~/Scripts/sbadmin/jquery.fancybox.pack.js",
                "~/Scripts/sbadmin/bootbox.min.js",
                "~/Scripts/sbadmin/bootstrap-datepicker.js",
                "~/Scripts/sbadmin/bootstrap-switch.js",
                "~/Scripts/sbadmin/bootstrap-select.min.js",
                "~/Scripts/bootstrap-colorpicker.js",
                "~/Scripts/sbadmin/messenger.min.js",
                "~/Scripts/sbadmin/messenger-theme-future.min.js",
                "~/Scripts/template_scripts/js/base.js",
                "~/Scripts/template_scripts/js/jquery.blockUI.js"

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
                    //"~/Content/template_resources/css/bootstrap-responsive.min.css",
                    //"~/Content/template_resources/css/font-awesome.css",
                    "~/Content/template_resources/css/tabs/*.css",
                    "~/Content/sbadmin/jquery.fancybox.css",
                    "~/Content/sbadmin/datepicker.css",
                    "~/Content/sbadmin/messenger.min.css",
                    "~/Content/sbadmin/messenger-theme-future.min.css",
                    "~/Content/sbadmin/bootstrap-switch.css",
                    "~/Content/bootstrap-colorpicker.css",
                    "~/Content/template_resources/css/style.css",
                    "~/Content/template_resources/css/ct-navbar.css"

            ));

            bundles.Add(new StyleBundle("~/bundles/signin_css").Include(
                    "~/Content/template_resources/css/pages/signin.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/mapa").Include(
                        "~/Scripts/sbadmin/map.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/AdminCss").Include(
                   "~/Content/sbadmin/bootstrap.css",
                   "~/Content/font-awesome/css/font-awesome.css",
                   "~/Content/sbadmin/datepicker.css",
                   "~/Content/sbadmin/bootstrap-select.min.css",
                   "~/Content/sbadmin/bootstrap-switch.css",
                   "~/Content/sbadmin/jquery.fancybox.css",
                   "~/Content/sbadmin/animate.min.css",
                   "~/Content/sbadmin/messenger.min.css",
                    "~/Content/sbadmin/messenger-theme-future.min.css",
                   "~/Content/sbadmin/sb-admin.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/bootbox.min.js",
                        "~/Scripts/bootstrap-switch.js",
                        "~/Scripts/flatui-checkbox.js",
                        "~/Scripts/flatui-radio.js",
                        "~/Scripts/tagsinput.js",
                        "~/Scripts/jquery.placeholder.js",
                        "~/Scripts/gestion.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/base").Include(
                        "~/Content/styles/flat-ui.css"));
        }
    }
}