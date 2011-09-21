using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using GeniusCode.Framework.Extensions;
using NLog;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms
{
    static class Program
    {
        // NLog - Helpful to diagnose if a DLL cannot be found, etc.
        private static Logger logger;

        // Runtime Action Visitor, able to apply actions to Reports @ Runtime
        private static XRRuntimeSubscriber subscriber;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            XRDesignForm form = CreateDesignForm();
            var controller = form.DesignMdiController;

            SetupNLog();

            // Pass Datasource to Subreports
            controller.SetupDesignTimeSubreportDatasourcePassing();

            // Runtime Actions
            SetupRuntimeActions();

            // Design-Time Datasources
            SetupDesignTimeDataSources();

            // Add toolbar button to select Datasource
            AddToolbarButton_SelectDataSource(form);

            Application.Run(form);
        }


        private static void SetupRuntimeActions()
        {
            // Runtime Actions replace code-behind in Reports
            // Can be applied to ANY control in a report's heirarchy, including n-level subreports.
            var runtimeActions = new List<IReportRuntimeAction>()
            {
                // Will pass Datasource to Subreports at Runtime
                new PassSubreportDataSourceRuntimeAction(),

                // ColorReplaceAction, which can change report colors at runtime
                //new ColorReplacerAction()
                new ReportRuntimeActionBase<XRLabel>(label => label.Name.Contains("MakeMeGold"), label => label.BackColor = Color.Gold)
            };

            var controller = new XRRuntimeActionController(runtimeActions.ToArray());
            subscriber = new XRRuntimeSubscriber(controller);
        }





        private static XRDesignForm CreateDesignForm()
        {
            // Create Runtime Report Designer
            XRDesignForm form = new XRDesignForm();

            form.Text = "Ryan's Report Designer";

            // Report Factory Service (which uses custom MyReportBase class)
            var service = new ReportFactory();
            form.DesignMdiController.AddService(typeof(ReportTypeService), service);

            return form;
        }

        private static void SetupDesignTimeDataSources()
        {
            // MEF Datasources
            var datasourceBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var relativePath = ConfigurationSettings.AppSettings["RelativeDataSourcePath"];
            var path = Path.Combine(datasourceBasePath, relativePath);
            DataSourceLocator.SetBasePath(path);
        }

        #region Select Datasource Dialog

        private static void AddToolbarButton_SelectDataSource(XRDesignForm form)
        {
            var item = new BarButtonItem(form.DesignBarManager, "Select Datasource...");

            item.ItemClick += (s, e) =>
            {
                if (form.DesignMdiController.ActiveDesignPanel == null)
                    return;

                var report = form.DesignMdiController.ActiveDesignPanel.Report;
                report.TryAs<MyReportBase>(myReport => PromptSelectDatasource(form, myReport));
            };

            form.DesignBarManager.Toolbar.AddItem(item);
        }

        private static void PromptSelectDatasource(XRDesignForm form, MyReportBase report)
        {
            Form selectForm2 = null;
            Action<DesignTimeDataSourceDefinition> callback = (definition) =>
            {
                selectForm2.Close();
                DesignTimeHelper.PopulateDesignTimeDataSource(report, definition);

                // Re-active Design Panel
                // This will refresh the Fields List
                form.DesignMdiController.ActiveDesignPanel.Activate();
            };
            var selectForm = new SelectDesignTimeDataSourceForm(report, callback);
            selectForm2 = selectForm;
            selectForm.BringToFront();
            selectForm.ShowDialog();
        }

        #endregion

        private static void SetupNLog()
        {
            // Create a Logger
            logger = LogManager.GetCurrentClassLogger();

            // Add the event handler for handling non-UI thread exceptions 
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                logger.FatalException("Report Designer encountered unhandled exception", exception);
            };
        }

    }
}
