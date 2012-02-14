using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraReports.Extensions;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using GeniusCode.Framework.Extensions;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Designer;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Designer
{

    public class DesignerContext : IDesignerContext
    {
        private readonly List<IReportRuntimeAction> _additionalReportActions;
        private XRReportController _controller;

        private string _reportBasePath;
        private string _datasourceBasePath;

        public DataSourceLocator DataSourceLocator { get; private set; }
        public XRDesignForm DesignForm { get; private set; }



        public DesignerContext(List<IReportRuntimeAction> additionalReportActions, string relativeReportBasePath, string relativeDatasourceBasePath)
        {
            _additionalReportActions = additionalReportActions;
            // Design-Time MEF Datasources
            DataSourceLocator = new DataSourceLocator(relativeDatasourceBasePath);

            // Designer
            CreateDesigner(relativeReportBasePath);
        }

        public IReportController GetController(XtraReport report)
        {
            var runtimeActions = GetRuntimeActions(_additionalReportActions);
            var facade = new XRRuntimeActionFacade(runtimeActions);

            return new XRReportController(report, facade);
        }

        public string ProjectRootPath { get; set; }

        private void CreateDesigner(string relativeReportBasePath)
        {
            DesignForm = CreateDesignForm();
            var controller = DesignForm.DesignMdiController;

            // Hide Scripting & HTML Preview
            controller.SetCommandVisibility(ReportCommand.ShowScriptsTab, CommandVisibility.None);
            controller.SetCommandVisibility(ReportCommand.ShowHTMLViewTab, CommandVisibility.None);

            // Use ReportController during PrintPreview
            this.UseReportControllerDuringPrintPreview();

            // Pass Datasource to Subreports
            this.SetupDesignTimeSubreportDatasourcePassing();

            // Add toolbar button to select Datasource
            AddToolbarButton_SelectDataSource(DesignForm);

            // Relative Path ReportSourceURL (Sad this is static/global.  Bad Design, DevExpress.)
            var relativePathStorage = new RelativePathReportStorage(relativeReportBasePath);
            ReportStorageExtension.RegisterExtensionGlobal(relativePathStorage);
        }

        private IReportRuntimeAction[] GetRuntimeActions(List<IReportRuntimeAction> additionalActions)
        {
            // Runtime Actions replace code-behind in Reports
            // Can be applied to ANY control in a report's heirarchy, including n-level subreports.
            var runtimeActions = new List<IReportRuntimeAction>()
            {
                // Will pass Datasource to Subreports at Runtime
                new PassSubreportDataSourceRuntimeAction(),
            };

            // Add custom actions
            runtimeActions.AddRange(additionalActions);

            return runtimeActions.ToArray();
        }

        private static XRDesignForm CreateDesignForm()
        {
            // Create Runtime Report Designer
            XRDesignForm form = new XRDesignForm();

            form.Text = "Ryan & Jer's Report Designer";

            // Report Factory Service (which uses custom MyReportBase class)
            var service = new ReportFactory();
            form.DesignMdiController.AddService(typeof(ReportTypeService), service);

            return form;
        }


        #region Select Datasource Dialog

        private void AddToolbarButton_SelectDataSource(XRDesignForm form)
        {
            var item = new BarButtonItem(form.DesignBarManager, "Select Datasource...");

            // Click Handler
            item.ItemClick += (s, e) =>
            {
                if (form.DesignMdiController.ActiveDesignPanel == null)
                {
                    MessageBox.Show("Please create/open a report.");
                    return;
                }

                var report = form.DesignMdiController.ActiveDesignPanel.Report;
                report.TryAs<MyReportBase>(myReport => PromptSelectDatasource(form, myReport));
            };

            // Add Datasource Button
            form.DesignBarManager.Toolbar.AddItem(item);
        }

        private void PromptSelectDatasource(XRDesignForm form, MyReportBase report)
        {
            Form dialog = null;

            // Datasource Selected Callback
            Action<DesignTimeDataSourceDefinition> callback = (definition) =>
            {
                dialog.Close();

                // Change Report Datasource
                report.ChangeDesignTimeDatasource(definition, this);
            };

            // Create Select Datasource Dialog
            dialog = new SelectDesignTimeDataSourceForm(this, report, callback);
            dialog.BringToFront();
            dialog.ShowDialog();
        }

        #endregion

        bool IsReportInDesigner(MyReportBase report)
        {
            // TODO: Implement, in case there are multiple DesignerContext's
            return true;
        }

    }
}
