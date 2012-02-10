﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Native;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.RuntimeActions
{
     public class PassSubreportDataSourceRuntimeAction : ReportRuntimeAction<XRSubreport>
    {
        private static readonly Func<XRSubreport, bool> Predicate = s => true;

        public PassSubreportDataSourceRuntimeAction()
            : this(null)
        {
        }

        private static void OnPerformAction(XRSubreport subReport, Action<XRSubreport, object> nestedAction)
        {
            var ds = RunTimeHelper.SetDataSourceOnSubreport(subReport);

            if (nestedAction != null)
                nestedAction(subReport,ds);
        }

        public PassSubreportDataSourceRuntimeAction(Action<XRSubreport, object> nestedAction)
            : base( Predicate,r=> OnPerformAction(r, nestedAction))
                                  
        {
        }

    }
}
