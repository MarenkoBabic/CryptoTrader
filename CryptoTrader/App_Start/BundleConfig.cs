using System.Web;
using System.Web.Optimization;

namespace CryptoTrader
{
    public class BundleConfig
    {
        // Weitere Informationen zur Bündelung finden Sie unter https://go.microsoft.com/fwlink/?LinkId=301862.
        public static void RegisterBundles( BundleCollection bundles )
        {
            bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
                "~/Scripts/JsDefault/jquery-{version}.js" ) );

            bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
                "~/Scripts/JsDefault/jquery.validate*",
                "~/Scripts/JsDefault/jquery.avlidate.min.js") );

            bundles.Add( new ScriptBundle( "~/bundles/jqueryui" ).Include(
                "~/Scripts/JsDefault/jquery-ui-{version}.js",
                "~/Scripts/JsDefault/jquery-ui-{version}.min.js") );

            bundles.Add( new ScriptBundle( "~/bundles/toastr" ).Include(
                "~/Scripts/JsDefault/toastr.js",
                "~/Scripts/JsDefault/toastr.min.js") );



            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // bereit ist für die Produktion, verwenden Sie das Buildtool unter https://modernizr.com, um nur die benötigten Tests auszuwählen.
            bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
                "~/Scripts/JsDefault/modernizr-*") );

            bundles.Add( new ScriptBundle( "~/bundles/bootstrap" ).Include(
                "~/Scripts/JsDefault/bootstrap.js",
                "~/Scripts/JsDefault/respond.js") );

            bundles.Add( new StyleBundle( "~/Content/css" ).Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/toast/toastr.css" ) );
            bundles.Add( new ScriptBundle( "~/bundles/highstock" ).Include(
                     "~/Scripts/JsDefault/Highstock/exporting.js",
                     "~/Scripts/JsDefault/Highstock/highstock.js") );


        }
    }
}
