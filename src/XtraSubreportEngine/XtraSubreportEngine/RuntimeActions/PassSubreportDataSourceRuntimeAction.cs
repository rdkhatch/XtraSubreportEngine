using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Native;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.RuntimeActions
{
     public sealed class PassSubreportDataSourceRuntimeAction : ReportRuntimeActionBase<XRSubreport>
    {
         private readonly Action<XRSubreport, object> _nestedAction;

        public PassSubreportDataSourceRuntimeAction()
            : this(null)
        {
        }

        public PassSubreportDataSourceRuntimeAction(Action<XRSubreport, object> nestedAction)
        {
            _nestedAction = nestedAction;
        }

         protected override void PerformAction(XRSubreport control)
         {
             var ds = control.SetDataSourceOnSubreport();

             if (_nestedAction != null)
                 _nestedAction(control, ds);
         }
    }
}
