using System.Web;
using System.Web.Optimization;

namespace WMSv2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/common_js").Include(
                "~/Content/assets/js/core/popper.min.js",
                "~/Content/assets/js/core/jquery.min.js",
                "~/Content/assets/js/plugins/moment.min.js",
                "~/Content/assets/demo/demo.js",
                "~/Content/assets/js/plugins/jquery.bootstrap-wizard.js",
                "~/Content/assets/js/plugins/jquery.dataTables.min.js",
                "~/Content/assets/js/import/Buttons-1.5.6/js/dataTables.buttons.js",
                "~/Content/assets/js/import/Buttons-1.5.6/js/dataTables.buttons.min.js",
                "~/Content/assets/js/import/Buttons-1.5.6/js/buttons.bootstrap4.js",
                "~/Content/assets/js/import/Buttons-1.5.6/js/buttons.bootstrap4.min.js",
                "~/Content/assets/js/core/bootstrap.min.js",
                "~/Content/assets/js/plugins/bootstrap-datetimepicker.js",
                "~/Content/assets/js/plugins/bootstrap-notify.js",
                "~/Content/assets/js/plugins/bootstrap-selectpicker.js",
                "~/Content/assets/js/plugins/bootstrap-switch.js",
                "~/Content/assets/js/plugins/bootstrap-tagsinput.js",
                "~/Content/assets/js/plugins/chartjs.min.js",
                "~/Content/assets/js/plugins/fullcalendar.min.js",
                "~/Content/assets/js/plugins/jasny-bootstrap.min.js",
                "~/Content/assets/js/plugins/nouislider.min.js",
                "~/Content/assets/js/plugins/perfect-scrollbar.jquery.min.js",
                "~/Content/assets/js/plugins/sweetalert2.min.js",
                "~/Content/assets/js/paper-dashboard.min790f.js",
              
                
                "~/Scripts/helper.js",
                "~/Scripts/sidebar.js",
                "~/Content/dist/js/select2.min.js"

                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/common_css").Include(
                "~/Content/assets/css/bootstrap.min.css",
                "~/Content/assets/css/paper-dashboard.min790f.css",
                "~/Content/assets/demo/demo.css",
                "~/Content/assets/demo/docs.min.css",
                "~/Content/assets/js/import/Buttons-1.5.6/css/buttons.bootstrap4.css",
                "~/Content/dist/css/select2.min.css",
                "~/Content/assets/css/customhere.css"
                ));
        }
    }
}
