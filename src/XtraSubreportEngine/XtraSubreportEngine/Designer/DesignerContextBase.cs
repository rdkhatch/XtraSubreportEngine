using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreportEngine;

namespace XtraSubreport.Engine.Designer
{
    public abstract class DesignerContextBase : IDesignerContext, IDesignerContextInternal
    {
        protected abstract DataSourceLocator GetDataSourceLocator();
        protected abstract XRDesignForm GetDesignForm();

        #region IDesignerContext

        DataSourceLocator IDesignerContext.DataSourceLocator
        {
            get { return GetDataSourceLocator(); }
        }

        XRDesignForm IDesignerContext.DesignForm
        {
            get { return GetDesignForm(); }
        }

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
