using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Extensions;
using XtraSubreportEngine.Support;
using GeniusCode.Framework.Extensions;
using System.Diagnostics;
using DevExpress.XtraBars;
using XtraSubreportEngine;
using System.Reflection;
using System.IO;
using System.Configuration;
using NLog;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions.Providers;
using ColorReplaceAction;

namespace XtraSubReport.Winforms
{
    static class Program
    {
        // Selected Subreport on Design Panel
        public static SubreportBase selectedSubreport;

        // NLog - Helpful to diagnose if a DLL cannot be found, etc.
        private static Logger logger;

        // Runtime Action Visitor, able to apply actions to Reports @ Runtime
        private static XRRuntimeVisitor visitor;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var form = CreateDesignForm();
            var controller = form.DesignMdiController;

            SetupNLog();
            SetupRuntimeActions();
            SetupDesignTime(controller);
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
                new ColorReplacerAction()
            };

            visitor = new XRRuntimeVisitor(runtimeActions);
            visitor.AttachToAggregator(report => true);
        }

        private static void SetupDesignTime(XRDesignMdiController controller)
        {
            // Design-Time Event Handler
            controller.OnDesignPanelActivated(designPanel =>
            {
                var report = designPanel.Report;

                // Populate Design-Time Datasource
                report.TryAs<MyReportBase>(myReport =>
                {
                    if (selectedSubreport == null)
                        // Stand-alone Report
                        DesignTimeHelper.PopulateDesignTimeDataSource(myReport);
                    else
                        // Subreport was Double-clicked, new DesignPanel has been activated for it
                        // Pass Design-Time DataSource from Parent to Subreport
                        DesignTimeHelper.PassDesignTimeDataSourceToSubreport(selectedSubreport, myReport);
                });

                // Capture selected Subreport
                // So we can enable double-clicking Subreport
                designPanel.SelectionChanged += (sender, e) =>
                {
                    var selected = designPanel.GetSelectedComponents();
                    selectedSubreport = selected.OfType<SubreportBase>().SingleOrDefault();

#if DEBUG
                    if (selectedSubreport != null)
                    {
                        var path = selectedSubreport.Band.GetFullDataMemberPath();
                        Debug.WriteLine("You selected subreport with Path: {0}".FormatString(path));
                    }
#endif
                };


            });
        }

        private static void SetupDesignTimeDataSources()
        {
            // MEF Datasources
            var datasourceBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var relativePath = ConfigurationSettings.AppSettings["RelativeDataSourcePath"];
            var path = Path.Combine(datasourceBasePath, relativePath);
            DataSourceProvider.SetBasePath(path);
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
