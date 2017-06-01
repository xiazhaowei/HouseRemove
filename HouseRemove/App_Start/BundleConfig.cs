using System.Web;
using System.Web.Optimization;

namespace MemberCenter
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {          

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/jquery-1.7.1.min.js",
                      "~/Scripts/jquery-ui-1.8.17.min.js",
                      "~/Scripts/styler.js",
                      "~/Scripts/jquery.tipTip.js",
                       "~/Scripts/colorpicker.js",
                       "~/Scripts/sticky.full.js",
                       "~/Scripts/forms/jquery.validate.min.js",
                       "~/Scripts/global.js"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                        "~/Content/reset.css",
                    "~/Content/grid.css",
                    "~/Content/tipTip.css",
                    "~/Content/jquery-ui.css",
                    "~/Content/ad.css",
                    "~/Content/sticky.full.css",
                    "~/Content/colorpicker.css",
                    "~/Content/demo.css",
                      "~/Content/tables.css",
                      "~/Content/icons.css",
                      "~/Content/site.css"));
        }
    }
}
