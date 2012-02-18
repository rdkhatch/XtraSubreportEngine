using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Design;

namespace XtraSubReport.Winforms
{
    public class DesignDataContext2 : IDesignDataContext
    {
        public DesignDataContext2(IDesignReportMetadataAssociationRepository designDataDefinitionRepository, IDesignDataRepository designDataRepository)
        {
            DesignDataRepository = designDataRepository;
            DesignDataDefinitionRepository = designDataDefinitionRepository;
        }

        public IDesignDataRepository DesignDataRepository { get; private set; }
        public IDesignReportMetadataAssociationRepository DesignDataDefinitionRepository { get; private set; }
    }
}