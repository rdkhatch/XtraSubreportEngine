using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using GeniusCode.Framework.Extensions;
using XtraSubReport.Winforms.Prototypes;
using XtraSubreport.Design;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Support
{
    public class ActionMessageHandler : IHandle<DataSourceSelectedForReportMessage>, IHandle<ReportActivatedMessage>, IHandle<ReportActivatedBySubreportMessage>, IHandle<DesignPanelPrintPreviewMessage>
    {
        private readonly IDataSourceSetter _dataSourceSetter;
        private readonly IDesignDataRepository _designDataRepository;
        private readonly IDesignReportMetadataAssociationRepository _metadataAssociationRepository;
        private readonly IReportControllerFactory _reportControllerFactory;

        public ActionMessageHandler(IDataSourceSetter dataSourceSetter, IEventAggregator aggregator, IDesignDataRepository designDataRepository, IDesignReportMetadataAssociationRepository metadataAssociationRepository, IReportControllerFactory reportControllerFactory)
        {
            _dataSourceSetter = dataSourceSetter;
            _designDataRepository = designDataRepository;
            _metadataAssociationRepository = metadataAssociationRepository;
            _reportControllerFactory = reportControllerFactory;
            aggregator.Subscribe(this);
        }

        public void Handle(ReportActivatedMessage message)
        {
            //TODO: add xml later
        }


        /// <summary>
        /// Concatenates all nested DataMember paths to create the Full DataMember Path
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        private string GetTraversalPath(Band band)
        {
            var path = String.Empty;

            if (band == null)
                return path;

            band.TryAs<XtraReportBase>(report => path = GetFullDataMemberPathForXtraReportBase(report));
                                           

            if (band is XtraReportBase == false)
            {
                var report = band.Report;

                // Recurse into Report, which has the DataMember for the Collection
                // Prevent infinite loop, when a report is it's own parent
                if (band != report)
                    path = GetTraversalPath(report);
            }

            band.TryAs<DetailBand>(detail =>
                                       {
                                           // Detail band is a single item
                                           // By Default, Use first element for design-time
                                           path += "[0]";
                                       });

            return path;
        }

        private string GetFullDataMemberPathForXtraReportBase(XtraReportBase report)
        {
            var context = report.GetReportDataContext();
            var path = context.GetDataMemberDisplayName(report.DataSource, report.DataMember);

            var isRootReport = report == report.Report;

            // Keep calling parent reports, until there is no DataMember
            if (!isRootReport && path != String.Empty)
            {
                // Go deeper
                string parentPath = GetTraversalPath(report.Report);
                path = "{0}.{1}".FormatString(parentPath, path);

                // Prevent a period at the beginning
                if (path.StartsWith("."))
                    path = path.Remove(0, 1);
            }
            else
            {
                // If we are at the top, add starting Relation Path from datasource
                var asMyReportBase = report as MyReportBase;
                if (asMyReportBase != null)
                {
                    var selectedDatasourceDefinition =
                        _metadataAssociationRepository.GetCurrentAssociationForReport(asMyReportBase);

                        if (selectedDatasourceDefinition != null)
                        {
                            // Append parent report
                            var startingReportPath = _metadataAssociationRepository.GetCurrentAssociationForReport(asMyReportBase);
                            path = startingReportPath.TraversalPath;
                        }
                }
                
            }

            return path;
        }

        public void Handle(ReportActivatedBySubreportMessage message)
        {         
            // go to parent
            var parentReport = message.SelectedSubreport.NavigateToMyReportBase();
            // get datasource metadata from parent
            var parentDataSourceDefinition = _metadataAssociationRepository.GetCurrentAssociationForReport(parentReport);
            // get traversal path
            var path = GetTraversalPath(message.SelectedSubreport.Band);
            // set datasource on new report
            _dataSourceSetter.SetReportDatasource(message.NewReport,parentDataSourceDefinition,path);
        }

        public void Handle(DesignPanelPrintPreviewMessage message)
        {
            var previewReport = message.DesignPanel.Report.CloneLayoutAsMyReportBase();
            var reportController = _reportControllerFactory.GetController(previewReport);
            // Print Preview!
            reportController.Print(r => r.ShowPreviewDialog(message.DesignPanel.LookAndFeel));
        }

        public void Handle(DataSourceSelectedForReportMessage message)
        {
            _dataSourceSetter.SetReportDatasource(message.Report,message.ReportDatasourceMetadataWithTraversal,message.ReportDatasourceMetadataWithTraversal.TraversalPath);
        }
    }
}