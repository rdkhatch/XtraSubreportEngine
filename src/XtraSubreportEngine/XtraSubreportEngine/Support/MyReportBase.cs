using System;
using XtraSubreport.Engine.Support;
using DevExpress.XtraReports;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Extensions;
using DevExpress.XtraReports.Localization;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubreportEngine.Support
{

    [RootClass]
    // RootClassAttribute is REQUIRED for subreports!
    // Otherwise, custom classes are NOT allowed as subreports.
    // http://devexpress.com/Support/Center/p/Q300888.aspx
    public class MyReportBase : XtraReportWithCustomPropertiesBase
    {

        private DesignTimeDataSourceDefinition _SelectedDesignTimeDataSource;

        public MyReportBase() : base()
        {
            DesignTimeDataSources = new XRSerializableCollection<DesignTimeDataSourceDefinition>();
        }

        protected override void DeclareCustomProperties()
        {
            DeclareCustomObjectProperty(() => this.DesignTimeDataSources);
        }

        // Design-Time Datasources
        [SRCategory(ReportStringId.CatData)]
        public XRSerializableCollection<DesignTimeDataSourceDefinition> DesignTimeDataSources { get; set; }

        [SRCategory(ReportStringId.CatData)]
        public DesignTimeDataSourceDefinition SelectedDesignTimeDataSource
        {
            get { return _SelectedDesignTimeDataSource;  }
            set
            {
                _SelectedDesignTimeDataSource = value;

                object datasource = null;

                if (_SelectedDesignTimeDataSource != null)
                {
                    // Store datasource definition, so we can reuse it
                    AddDesignTimeDataSource(_SelectedDesignTimeDataSource);

                    // Fetch datasource
                    datasource = DataSourceProvider.GetObjectFromDataSourceDefinition(_SelectedDesignTimeDataSource);
                }

                this.SetReportDataSource(datasource);
            }
        }

        private void AddDesignTimeDataSource(DesignTimeDataSourceDefinition definition)
        {
            if (DesignTimeDataSources.Contains(definition))
                return;

            // Add item as first in list - So we know it was the last one used
            DesignTimeDataSources.Insert(0, definition);
        }

        protected override void OnBeforePrint(System.Drawing.Printing.PrintEventArgs e)
        {
            // IMPORTANT: Must use an aggregator for End-User Designer, because reports are serialized / CodeDom - events cannot be attached
            XRRuntimeVisitorAggregator.FireBeforePrintEvent(this, e);

            base.OnBeforePrint(e);
        }


    }
}
