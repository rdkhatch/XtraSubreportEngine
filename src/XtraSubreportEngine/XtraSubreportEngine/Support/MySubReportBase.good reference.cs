using System;
using DevExpress.XtraReports.UI;
using System.ComponentModel;
using DevExpress.XtraReports;
using DevExpress.XtraReports.Localization;
using DevExpress.XtraReports.Design;
using DevExpress.Utils;

namespace XtraSubreportEngine.Support
{
    public class MySubReportBase : XRSubreport
    {

        XRDesignBinding _DataSource;
        IServiceProvider _provider;

        public void SetDataSource(IServiceProvider provider, object datasource, string datamember)
        {
            _provider = provider;
            _DataSource = new XRDesignBinding(datasource, datamember);
        }

        //[Editor("DevExpress.XtraReports.Design.ReportUrlEditor,DevExpress.XtraReports.v10.2.Extensions", typeof(UITypeEditor))]
        //[DXDisplayName(typeof(ResFinder), "PropertyNamesRes", "DevExpress.XtraReports.UI.XRSubreport.ReportSourceUrl")]
        //[DefaultValue("")]
        //[DXDisplayName(typeof(ResFinder), "PropertyNamesRes", "DevExpress.XtraReports.Design.DataBinding.Binding")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [SRCategory(ReportStringId.CatData)]
        public XRDesignBinding DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        public string DataSourcePath
        {
            get { return DataSource.GetDisplayName(_provider); }
        }

    }
}
