using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.RuntimeActions
{
    public interface IRuntimeActionFacade
    {
        void AttemptActionsOnControl(XRControl control);
    }
}
