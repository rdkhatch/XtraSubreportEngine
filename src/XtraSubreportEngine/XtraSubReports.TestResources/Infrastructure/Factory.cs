using System.Collections.Generic;
using System.Linq;
using XtraSubReport.Winforms;
using XtraSubReport.Winforms.Repositories;
using XtraSubReport.Winforms.Support;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;

namespace XtraSubReports.TestResources.Infrastructure
{
    public static class Factory
    {
        public static IDesignDataContext CreateForDogTime(out IDataSourceSetter setter)
        {
            var providers = new List<IReportDatasourceProvider> { new DogTimeReportDatasourceProvider() };
            var dataDefRep = new DesignReportMetadataAssociationRepository();
            var datarep = new DesignDataRepository(providers);
            setter = new DataSourceSetter(datarep, dataDefRep, new ObjectGraphPathTraverser());
            return new DesignDataContext2(dataDefRep,datarep);
        }
    }
}