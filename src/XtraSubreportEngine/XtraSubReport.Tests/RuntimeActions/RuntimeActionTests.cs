using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine.RuntimeActions;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine;
using System.IO;
using DevExpress.XtraPrinting;
using System.Drawing;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class RuntimeActionTests
    {
        [TestMethod]
        public void change_all_label_text()
        {
            var transformText = "Jeremiah";
            var action = new ReportRuntimeActionBase<XRLabel>((l) => true, (l) => l.Text = transformText);

            var label = new XRLabel() { Text = string.Empty };

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(label);

            var visitor = new XRRuntimeVisitor(action);
            visitor.AttachTo(report);

            TestHelper.RunReport(report);

            Assert.AreEqual(transformText, label.Text);
        }

        [TestMethod]
        public void predicate_prevents_applying_action()
        {
            var transformText = "Jeremiah";
            var action = new ReportRuntimeActionBase<XRLabel>((l) => false, (l) => l.Text = transformText);

            var label = new XRLabel() { Text = string.Empty };

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(label);

            var visitor = new XRRuntimeVisitor(action);
            visitor.AttachTo(report);

            TestHelper.RunReport(report);

            Assert.AreNotEqual(transformText, label.Text);
        }

        [TestMethod]
        public void applies_to_entire_report()
        {
            var color = Color.Green;
            var action = new ReportRuntimeActionBase<XtraReport>((r) => true, (r) => r.BackColor = color);

            var report = new ReportFactory().GetNewReport();

            var visitor = new XRRuntimeVisitor(action);
            visitor.AttachTo(report);

            TestHelper.RunReport(report);

            Assert.AreEqual(color, report.BackColor);
        }

        [TestMethod]
        public void applies_to_subreports()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            var subreportContainer = tuple.Item2;
            var subreport = tuple.Item3;

            var color = Color.Green;
            var action = new ReportRuntimeActionBase<XtraReport>((r) => true, (r) => r.BackColor = color);

            var visitor = new XRRuntimeVisitor(action);
            visitor.AttachTo(parentReport);

            TestHelper.RunReport(parentReport);

            Assert.AreEqual(color, subreport.BackColor);
        }

        [TestMethod]
        public void applies_to_tables()
        {
            var transformColor = Color.Blue;
            var action = new ReportRuntimeActionBase<XRControl>(c => true, c => c.BackColor = transformColor);

            var table = new XRTable();
            var row = new XRTableRow();
            var cell = new XRTableCell();
            row.Cells.Add(cell);
            table.Rows.Add(row);

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(table);

            var visitor = new XRRuntimeVisitor(action);
            visitor.AttachTo(report);

            TestHelper.RunReport(report);

            Assert.AreEqual(transformColor, cell.BackColor);
        }

    }
}
