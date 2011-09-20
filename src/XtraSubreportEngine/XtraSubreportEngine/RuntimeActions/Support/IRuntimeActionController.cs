using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.RuntimeActions
{
    public interface IRuntimeActionController
    {
        void AttemptActionsOnControl(XRControl control);
    }
}
