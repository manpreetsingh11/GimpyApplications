using System.Web;
using System.Web.Optimization;

namespace GimpySoftwareNewTemplate
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/assets/plugins/jquery/jquery-1.11.1.min.js",
                        "~/Content/assets/plugins/modernizr.custom.js",
                        "~/Content/assets/plugins/jquery-ui/jquery-ui.min.js",
                        "~/Content/assets/plugins/boostrapv3/js/bootstrap.min.js",
                        "~/Content/assets/plugins/jquery/jquery-easy.js",
                        "~/Content/assets/plugins/jquery-unveil/jquery.unveil.min.js",
                        "~/Content/assets/plugins/jquery-bez/jquery.bez.min.js",
                        "~/Content/assets/plugins/jquery-ios-list/jquery.ioslist.min.js",
                        "~/Content/assets/plugins/jquery-actual/jquery.actual.min.js",
                        "~/Content/assets/plugins/jquery-scrollbar/jquery.scrollbar.min.js",
                        "~/Content/assets/plugins/bootstrap-select2/select2.min.js",
                        "~/Content/assets/plugins/classie/classie.js",
                        "~/Content/assets/plugins/switchery/js/switchery.min.js",
                        "~/Content/assets/plugins/nvd3/lib/d3.v3.js",
                        "~/Content/assets/plugins/nvd3/nv.d3.min.js",
                        "~/Content/assets/plugins/nvd3/src/utils.js",
                        "~/Content/assets/plugins/nvd3/src/tooltip.js",
                        "~/Content/assets/plugins/nvd3/src/interactiveLayer.js",
                        "~/Content/assets/plugins/mapplic/js/hammer.js",
                        "~/Content/assets/plugins/mapplic/js/jquery.mousewheel.js",
                        "~/Content/assets/plugins/mapplic/js/mapplic.js",
                        "~/Content/assets/plugins/rickshaw/rickshaw.min.js",
                        "~/Content/assets/plugins/jquery-metrojs/MetroJs.min.js",
                        "~/Content/assets/plugins/jquery-sparkline/jquery.sparkline.min.js",
                        "~/Content/assets/plugins/skycons/skycons.js",
                        "~/Content/pages/js/pages.min.js",
                        "~/Content/assets/js/notifications.js"));


            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                        "~/Content/assets/js/dashboard.js",
                        "~/Content/assets/js/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Content/assets/plugins/jquery/jquery-1.11.1.min.js",
                        "~/Content/assets/plugins/modernizr.custom.js",
                        "~/Content/assets/plugins/jquery-ui/jquery-ui.min.js",
                        "~/Content/assets/plugins/boostrapv3/js/bootstrap.min.js",
                        "~/Content/assets/plugins/jquery/jquery-easy.js",
                        "~/Content/assets/plugins/jquery-unveil/jquery.unveil.min.js",
                        "~/Content/assets/plugins/jquery-bez/jquery.bez.min.js",
                        "~/Content/assets/plugins/jquery-ios-list/jquery.ioslist.min.js",
                        "~/Content/assets/plugins/jquery-actual/jquery.actual.min.js",
                        "~/Content/assets/plugins/jquery-scrollbar/jquery.scrollbar.min.js",
                        "~/Content/assets/plugins/bootstrap-select2/select2.min.js",
                        "~/Content/assets/plugins/classie/classie.js",
                        "~/Content/assets/plugins/jquery-validation/js/jquery.validate.min.js",
                        "~/Content/pages/js/pages.min.js",
                        "~/Content/assets/js/notifications.js"));
    


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                    "~/Content/assets/plugins/pace/pace-theme-flash.css",
                    "~/Content/assets/plugins/boostrapv3/css/bootstrap.min.css",
                    "~/Content/assets/plugins/font-awesome/css/font-awesome.css",
                    "~/Content/assets/plugins/jquery-scrollbar/jquery.scrollbar.css",
                    "~/Content/assets/plugins/bootstrap-select2/select2.css",
                    "~/Content/assets/plugins/switchery/css/switchery.min.css",
                    "~/Content/pages/css/pages-icons.css",
                    "~/Content/pages/css/pages.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/assets/plugins/pace/pace-theme-flash.css",
                    "~/Content/assets/plugins/simple-line-icons/simple-line-icons.css",
                    "~/Content/assets/plugins/boostrapv3/css/bootstrap.min.css",
                    "~/Content/assets/plugins/font-awesome/css/font-awesome.css",
                    "~/Content/assets/plugins/jquery-scrollbar/jquery.scrollbar.css",
                    "~/Content/assets/plugins/bootstrap-select2/select2.css",
                    "~/Content/assets/plugins/switchery/css/switchery.min.css",
                    "~/Content/assets/plugins/nvd3/nv.d3.min.css",
                    "~/Content/assets/plugins/mapplic/css/mapplic.css",
                    "~/Content/assets/plugins/rickshaw/rickshaw.min.css",
                    "~/Content/assets/plugins/bootstrap-datepicker/css/datepicker3.css",
                    "~/Content/assets/plugins/jquery-metrojs/MetroJs.css",
                    "~/Content/pages/css/pages-icons.css",
                    "~/Content/pages/css/pages.css"));

        }
    }
}
