using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;

namespace XtraSubreport.Contracts.DataSources
{

    public interface IReportDatasource
    {
        object DataSource { get; }
    }

}
