using System;
using System.Web;
using System.Web.Optimization;

namespace FreightQuote.Web
{
    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/alertify.min.js",
                      "~/Scripts/CustomJs/Common.js"
                      ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
         "~/Scripts/kendo/kendo.all.min.js",
                // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
         "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/alertfy").Include(
                        "~/Scripts/alertify.min.js"));
            
            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
          "~/Content/kendo/kendo.common.min.css",
          "~/Content/kendo/kendo.silver.min.css"));

            bundles.Add(new StyleBundle("~/css/dashtheme").Include(
         "~/Content/css/bootstrap.css",
          "~/Content/css/font-awesome.min.css",
          "~/Content/css/ionicons.min.css",
         "~/Content/css/AdminLTE.css"));

            bundles.Add(new StyleBundle("~/Content/alerttheme").Include("~/Content/css/alertify.default.css",
                "~/Content/css/alertify.core.css"
                ));
        }
    }
}