using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Designer;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubreport.Engine.Designer
{
    public abstract class DesignerContextBase : IDesignerContext, IDesignerContextInternal, IReportControllerFactory
    {
        protected abstract IDataSourceLocator GetDataSourceLocator();
        protected abstract XRDesignForm GetDesignForm();
        public string ProjectRootPath { get; private set; }

        public DesignerContextBase(string projectRootPath)
        {
            ProjectRootPath = projectRootPath;
        }

        #region IDesignerContext

        IDataSourceLocator IDesignerContext.DataSourceLocator
        {
            get { return GetDataSourceLocator(); }
        }

        XRDesignForm IDesignerContext.DesignForm
        {
            get { return GetDesignForm(); }
        }

        public abstract IReportController GetController(XtraReport report);

        #endregion

        #region IDesignerContextInternal

        protected SubreportBase _selectedSubreport;
        SubreportBase IDesignerContextInternal.SelectedSubreport
        {
            get
            {
                return _selectedSubreport;
            }
            set
            {
                _selectedSubreport = value;
            }
        }

        #endregion
    }
}
