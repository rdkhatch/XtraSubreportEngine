using System;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.Eventing
{
    public class XRLoadDesignTimeDatasourceMessage
    {
        public MyReportBase Report { get; private set; }
        public DesignTimeDataSourceDefinition DatasourceDefinition { get; private set; }

        public Action<object> SetDatasourceCallback { get; private set; }

        public XRLoadDesignTimeDatasourceMessage(MyReportBase report, DesignTimeDataSourceDefinition datasourceDefinition, Action<object> setDatasourceCallback)
        {
            Report = report;
            DatasourceDefinition = datasourceDefinition;
            SetDatasourceCallback = setDatasourceCallback;
        }
    }
}
