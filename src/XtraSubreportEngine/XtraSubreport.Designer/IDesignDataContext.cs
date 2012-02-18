using System.Collections.Generic;
using System.Linq;

namespace XtraSubreport.Design
{
    public interface IDesignDataContext
    {
        IDesignDataRepository DesignDataRepository { get; }
        IDesignReportMetadataAssociationRepository DesignDataDefinitionRepository { get; }
    }
}