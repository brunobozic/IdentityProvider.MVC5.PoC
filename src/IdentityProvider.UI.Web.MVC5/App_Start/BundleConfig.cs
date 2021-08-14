using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace IdentityProvider.UI.Web.MVC5
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Bundles/css")
                .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/bootstrap-select.css")
                .Include("~/Content/css/bootstrap-datepicker3.min.css")
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/icheck/blue.min.css", new CssRewriteUrlTransformAbsolute())
                //.Include("~/Content/css/AdminLTE.css" , new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/skins/skin-blue.css"));
            bundles.Add(new ScriptBundle("~/Bundles/js")
                .Include("~/Scripts/jquery-3.3.1.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/plugins/fastclick/fastclick.js")
                .Include("~/Scripts/plugins/slimscroll/jquery.slimscroll.js")
                .Include("~/Scripts/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/Scripts/plugins/moment/moment.js")
                .Include("~/Scripts/plugins/datepicker/bootstrap-datepicker.js")
                .Include("~/Scripts/plugins/icheck/icheck.js")
                .Include("~/Scripts/plugins/validator.js")
                .Include("~/Scripts/plugins/inputmask/jquery.inputmask.bundle.js")
                //.Include("~/Scripts/adminlte.js")
                .Include("~/Scripts/init.js"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}