using XtraSubreportEngine.Support;

namespace XtraSubreport.Designer
{
    public class TraversedDatasourceResult
    {
        public DesignTimeDataSourceDefinition Definition { get; private set; }
        public object RootDataSource { get; private set; }
        public object TraversedDataSource { get; private set; }

        public bool Succeeded
        {
            get { return RootDataSource != null && TraversedDataSource != null; }
        }

        public TraversedDatasourceResult(DesignTimeDataSourceDefinition definition, object rootDataSource, object traversedDataSource)
        {
            Definition = definition;
            RootDataSource = rootDataSource;
            TraversedDataSource = traversedDataSource;
        }
    }
}