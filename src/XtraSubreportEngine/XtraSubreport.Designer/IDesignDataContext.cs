using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DesignTime;

namespace XtraSubreport.Design
{
    public interface IDesignDataContext
    {
        IDesignDataRepository DesignDataRepository { get; }
        IDesignReportMetadataAssociationRepository DesignDataDefinitionRepository { get; }
    }
}