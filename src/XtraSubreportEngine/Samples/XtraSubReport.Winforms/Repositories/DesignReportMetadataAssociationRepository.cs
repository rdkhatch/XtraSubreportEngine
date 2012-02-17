using System.Collections.Generic;
using System.Linq;
using GeniusCode.Framework.Extensions;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreport.Design;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Repositories
{
    public class DesignReportMetadataAssociationRepository : IDesignReportMetadataAssociationRepository
    {
        private readonly Dictionary<int, IReportDatasourceMetadataWithTraversal> _currentlySelectedDictionary;
        private readonly Dictionary<int, HashSet<IReportDatasourceMetadataWithTraversal>> _allItemsDictionary;

        public DesignReportMetadataAssociationRepository()
        {
            _currentlySelectedDictionary = new Dictionary<int, IReportDatasourceMetadataWithTraversal>();
            _allItemsDictionary  = new Dictionary<int, HashSet<IReportDatasourceMetadataWithTraversal>>();
        }

        public IEnumerable<IReportDatasourceMetadataWithTraversal> GetAssociationsForReport(MyReportBase report)
        {
            return _allItemsDictionary.CreateOrGetValue(report.GetHashCode(),
                                                        () => new HashSet<IReportDatasourceMetadataWithTraversal>());           
        }

        public IReportDatasourceMetadataWithTraversal GetCurrentAssociationForReport(MyReportBase report)
        {
            return _currentlySelectedDictionary.GetIfExists(report.GetHashCode(), null);
        }

        public void AssociateWithReport(IReportDatasourceMetadataWithTraversal definition, MyReportBase report)
        {
            var hashset = _allItemsDictionary.CreateOrGetValue(report.GetHashCode(),
                                                               () => new HashSet<IReportDatasourceMetadataWithTraversal>());

            hashset.AddIfUnique(definition);
        }

        public void AssociateWithReportAsCurrent(IReportDatasourceMetadataWithTraversal definition, MyReportBase report)
        {
            _currentlySelectedDictionary.AddIfUniqueOrReplace(definition, ds => report.GetHashCode());
        }
    }
}