﻿using System;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.RuntimeActions
{
    public interface IReportController
    {
        MyReportBase Print(Action<XtraReport> printAction);
    }
}
