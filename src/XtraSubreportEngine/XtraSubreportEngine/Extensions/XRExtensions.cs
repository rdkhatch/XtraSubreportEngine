using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Browsing;
using DevExpress.XtraReports.Native.Data;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using GeniusCode.Framework.Extensions;

namespace XtraSubreport.Engine
{
    public static class XRExtensions
    {

        #region Get DataSource

        /// <summary>
        /// Obtains DataSource for this Band.  Works at Runtime and Design-Time. Very important method.
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        public static object GetDataSource(this Band band)
        {
            object result = null;

            // Single
            band.TryAs<DetailBand>(detailBand =>
                {
                    var browser = detailBand.Report.GetListBrowser();
                    result = browser.Current;
                });

            // Collection
            if (result == null)
            {
                var browser = band.Report.GetListBrowser();
                return browser.List;
            }

            return result;
        }

        public static ReportDataContext GetReportDataContext(this XtraReportBase report)
        {
            VerifyListBrowserAndDataSourceAreCreated(report);

            var field = typeof(XtraReportBase).GetField("fDataContext", BindingFlags.NonPublic | BindingFlags.Instance);

            return (ReportDataContext)field.GetValue(report);
        }

        public static ListBrowser GetListBrowser(this XtraReportBase report)
        {
            VerifyListBrowserAndDataSourceAreCreated(report);

            var field = typeof(XtraReportBase).GetField("dataBrowser", BindingFlags.NonPublic | BindingFlags.Instance);

            return (ListBrowser)field.GetValue(report);
        }

        private static void VerifyListBrowserAndDataSourceAreCreated(XtraReportBase report)
        {
            // Force ListBrowser to be created, along with DataContext
            // Very important.
            report.GetCurrentRow();
        }

        // Another method of passing Subreport it's parent datasource
        // This method did not work for me.
        //private static void ChangeSubreportParentForDataContext(this XRSubreport subreport)
        //{
        //    var report = subreport.ReportSource;

        //    //var dataContextContainer = subreport.Report;
        //    var dataContextContainer = subreport.RootReport;

        //    //container.Bands[BandKind.Detail].Controls.Add(report);

        //    var field = typeof(XtraReportBase).GetField("fParent", BindingFlags.NonPublic | BindingFlags.Instance);
        //    field.SetValue(report, dataContextContainer);
        //}

        #endregion

        #region Set DataSource

        public static void SetReportDataSource(this XtraReportBase report, object datasource)
        {
            if (report == null) return;

            if (datasource == null)
                report.DataSource = null;

            // Set Datasource, Must be a Collection
            else if (datasource is IEnumerable)
                report.DataSource = datasource;
            else
                report.DataSource = new List<object> { datasource };
        }

        #endregion

        public static void OnDesignPanelActivated(this XRDesignMdiController controller, Action<XRDesignPanel> handler)
        {
            controller.DesignPanelLoaded += (sender1, designLoadedArgs) =>
            {
                var designPanel = (XRDesignPanel)sender1;

                EventHandler activated = null;
                activated = (sender2, emptyArgs) =>
                {
                    designLoadedArgs.DesignerHost.Activated -= activated;
                    handler.Invoke(designPanel);
                };

                designLoadedArgs.DesignerHost.Activated += activated;
            };
        }

        public static IEnumerable<TreeListNode> GetAllNodes(this TreeList tree)
        {
            var accumulator = new TreeListOperationAccumulateNodes();
            tree.NodesIterator.DoOperation(accumulator);

            return accumulator.Nodes.Cast<TreeListNode>();
        }

        public static IEnumerable<XRControl> GetAllControls(this XRControl control)
        {
            var myControls = control.Controls.Cast<XRControl>();

            // Recursive
            var bandChildControls = from band in myControls.OfType<Band>()
                                    from bandChildControl in GetAllControls(band)
                                    select bandChildControl;

            return Enumerable.Concat(myControls, bandChildControls);
        }

        public static IEnumerable<XRControl> GetAllControls(this Band band)
        {
            var myControls = band.Controls.Cast<XRControl>();

            // Recursive
            var childControls = from control in myControls
                                from childControl in GetAllControls(control)
                                select childControl;

            return Enumerable.Concat(myControls, childControls);
        }


        public static IEnumerable<XRSubreport> FindAllSubreports(this XtraReport report)
        {
            var q = from band in report.Bands.Cast<Band>()
                    from subreport in FindAllSubreports(band)
                    select subreport;

            return q;
        }

        public static IEnumerable<XRSubreport> FindAllSubreports(this Band band)
        {
            var mySubreports = band.Controls.OfType<XRSubreport>();

            // Recursive
            var childBandSubreports = from childBand in band.Controls.OfType<Band>()
                                      from subreport in FindAllSubreports(childBand)
                                      select subreport;

            return Enumerable.Concat(mySubreports, childBandSubreports);
        }

    }

}
