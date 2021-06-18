using System.Web;
using System.Web.Optimization;

namespace WORKFLOW_APP
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/Scripts/assets/app_js").Include(
                        //"~/Scripts/assets/js/jquery-2.1.4.min.js",
                        "~/Scripts/assets/js/jquery-ui.min.js",
                        "~/Scripts/assets/js/bootstrap.min.js",
                        "~/Scripts/assets/js/jquery-ui.custom.min.js",
                        "~/Scripts/assets/js/jquery.ui.touch-punch.min.js",
                        "~/Scripts/assets/js/jquery.easypiechart.min.js",
                        "~/Scripts/assets/js/jquery.sparkline.index.min.js",
                        "~/Scripts/assets/js/jquery.flot.min.js",
                        "~/Scripts/assets/js/jquery.flot.pie.min.js",
                        "~/Scripts/assets/js/jquery.flot.resize.min.js",
                        "~/Scripts/assets/js/ace-elements.min.js",
                        "~/Scripts/assets/js/ace-extra.min.js",
                         "~/Scripts/assets/js/ace.min.js"));




            //jqx scripts

            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/Scripts/jqwidgets/jqx").Include(
                //"~/Scripts/assets/js/jquery-2.1.4.min.js",
                        "~/Scripts/jqwidgets/jqxcore.js",
            "~/Scripts/jqwidgets/jqxbuttons.js",
                "~/Scripts/jqwidgets/jqxscrollbar.js",
     "~/Scripts/jqwidgets/jqxdatatable.js",
        "~/Scripts/jqwidgets/jqxlistbox.js",
             "~/Scripts/jqwidgets/jqxdropdownlist.js",
              "~/Scripts/jqwidgets/jqxComboBox.js", 
                "~/Scripts/jqwidgets/jqxdata.js",
            "~/Scripts/jqwidgets/jqxtooltip.js",
                "~/Scripts/jqwidgets/jqxinput.js",
            "~/Scripts/jqwidgets/jqxradiobutton.js",
             "~/Scripts/jqwidgets/jqxcheckbox.js",
                  "~/Scripts/jqwidgets/jqxdatetimeinput.js",
             "~/Scripts/jqwidgets/jqxtabs.js",
                   "~/Scripts/jqwidgets/jqxwindow.js",
                   "~/Scripts/jqwidgets/jqxtabs.js",
                   "~/Scripts/jqwidgets/jqxpanel.js",
                   "~/Scripts/jqwidgets/jqxnumberinput.js",
                   "~/Scripts/jqwidgets/jqxdropdownlist.js",
                   "~/Scripts/jqwidgets/jqxdropdownbutton.js",
                   "~/Scripts/jqwidgets/jqxcalendar.js",
                   "~/Scripts/jqwidgets/jqxexpander.js", 
                   "~/Scripts/jqwidgets/jqxmenu.js",
                   "~/Scripts/jqwidgets/jqxgrid.js",
                   "~/Scripts/jqwidgets/jqxgrid.aggregates.js",
                    "~/Scripts/jqwidgets/jqxgrid.columnsreorder.js",
                     "~/Scripts/jqwidgets/jqxgrid.columnsresize.js",
                      "~/Scripts/jqwidgets/jqxgrid.edit.js",
                       "~/Scripts/jqwidgets/jqxgrid.export.js",
                        "~/Scripts/jqwidgets/jqxgrid.filter.js",
                         "~/Scripts/jqwidgets/jqxgrid.grouping.js",
                          "~/Scripts/jqwidgets/jqxgrid.pager.js",
                           "~/Scripts/jqwidgets/jqxgrid.selection.js",
                            "~/Scripts/jqwidgets/jqxgrid.sort.js",
                             
                             "~/Scripts/jqwidgets/jqxgrid.storage.js",
                              "~/Scripts/js/highcharts.js",
                              "~/Scripts/jqwidgets/jqxloader.js",
                               "~/Scripts/js/drilldown.js",
                                "~/Scripts/js/sweetalert.min.js",
                                 "~/Scripts/js/typed.js"
                               //     "~/App_Start/global.js"
          ));


            //jqx_css
            bundles.Add(new StyleBundle("~/Scripts/jqwidgets/jqx/css").Include(
                        "~/Scripts/jqwidgets/styles/jqx.base.css",
                         "~/Scripts/css/highcharts.css",
                          "~/Scripts/jqwidgets/styles/jqx.ui-redmond.css",
                           "~/Scripts/jqwidgets/styles/jqx.orange.css",
                            "~/Scripts/jqwidgets/styles/jqx.light.css",
                             "~/Scripts/jqwidgets/styles/jqx.darkblue.css",
                              "~/Scripts/jqwidgets/styles/jqx.blackberry.css",
                               "~/Scripts/css/sweetalert.css"
           ));
            //scripts for app
            bundles.Add(new ScriptBundle("~/Scripts/APP_SCRIPTS/Groups").Include(
           "~/Scripts/APP_SCRIPTS/web_service.js",
                "~/App_Start/global.js",
             "~/Scripts/APP_SCRIPTS/groups.js"
            ));


            bundles.Add(new ScriptBundle("~/Scripts/APP_SCRIPTS/Vacation").Include(
           "~/Scripts/APP_SCRIPTS/web_service.js",
                "~/App_Start/global.js",
             "~/Scripts/APP_SCRIPTS/Vacation.js"
            ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(

                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/Scripts/assets/app_css").Include(
                "~/Scripts/assets/font-awesome/4.5.0/css/font-awesome.min.css",
                        "~/Scripts/assets/css/bootstrap.min.css",
                         "~/Scripts/assets/css/jquery-ui.min.css",
                        "~/Scripts/assets/font-awesome/4.5.0/css/font-awesome.min.css",
                        "~/Scripts/assets/css/fonts.googleapis.com.css",
                        "~/Scripts/assets/css/ace.min.css",
                        "~/Scripts/assets/css/ace-skins.min.css",
                        "~/Scripts/assets/css/ace-rtl.min.css",
                         "~/Scripts/assets/css/hover-min.css",
                        "~/Scripts/APP_CSS/jqx.fresh.css",
                        "~/Scripts/APP_CSS/animate.css"));


            bundles.Add(new StyleBundle("~/Scripts/assets/app_css/GROUPS").Include("~/Scripts/APP_CSS/Groups.css"));


        }
    }
}