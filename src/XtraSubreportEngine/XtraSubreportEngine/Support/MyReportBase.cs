using DevExpress.XtraReports;
using DevExpress.XtraReports.Localization;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.Support;

namespace XtraSubreportEngine.Support
{

    [RootClass]
    // RootClassAttribute is REQUIRED for subreports!
    // Otherwise, custom classes are NOT allowed as subreports.
    // http://devexpress.com/Support/Center/p/Q300888.aspx
    public class MyReportBase : XtraReportWithCustomPropertiesBase
    {

        [SRCategory(ReportStringId.CatData)]
        public XRSerializableCollection<DesignTimeDataSourceDefinition> DesignTimeDataSources { get; set; }

        public MyReportBase()
        {
            DesignTimeDataSources = new XRSerializableCollection<DesignTimeDataSourceDefinition>();
        }

        protected override void DeclareCustomProperties()
        {
            // Serialize DesignTimeDataSources collection into .REPX file
            DeclareCustomObjectProperty(() => DesignTimeDataSources);
        }

        protected override void OnBeforePrint(System.Drawing.Printing.PrintEventArgs e)
        {
            // IMPORTANT: Must use an aggregator for End-User Designer, because reports are serialized / CodeDom - events cannot be attached
            // Reports pass themselves into the aggregator
            var message = new XRBeforePrintMessage(this, e);
            EventAggregator.Singleton.Publish(message);

            base.OnBeforePrint(e);
        }

    }
}
