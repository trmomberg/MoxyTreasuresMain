using System.Web;
using System.Web.Optimization;

namespace MoxyTreasures
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/customscripts").Include(
						//"~/Scripts/ajax.js"
						"~/Scripts/app-menu.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/app-menu.css",
					  "~/Content/site.css"));
		}
	}
}
