using System;
using System.Linq;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Design.Traversals
{
    public class TraversedDatasourceResult
    {
        public object RootDataSource { get; private set; }
        public object TraversedDataSource { get; private set; }

        public Type TraversedDataType {get { return Succeeded ? TraversedDataSource.GetType() : typeof (object); }}

        public bool Succeeded
        {
            get { return RootDataSource != null && TraversedDataSource != null; }
        }

        public TraversedDatasourceResult(object rootDataSource, object traversedDataSource)
        {
            RootDataSource = rootDataSource;
            TraversedDataSource = traversedDataSource;
        }
    }
}