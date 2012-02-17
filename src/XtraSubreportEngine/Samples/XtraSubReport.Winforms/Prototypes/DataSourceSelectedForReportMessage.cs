using System.Linq;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Winforms.Prototypes
{
    public class DataSourceSelectedForReportMessage
    {
        public ReportDatasourceMetadataWithTraversal ReportDatasourceMetadataWithTraversal { get; set; }
        public MyReportBase Report { get; set; }

        public DataSourceSelectedForReportMessage(ReportDatasourceMetadataWithTraversal reportDatasourceMetadataWithTraversal, MyReportBase report)
        {
            ReportDatasourceMetadataWithTraversal = reportDatasourceMetadataWithTraversal;
            Report = report;
        }
    }
}