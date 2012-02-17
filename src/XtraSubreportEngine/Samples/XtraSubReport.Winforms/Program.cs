using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using DevExpress.XtraBars;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using GeniusCode.Framework.Extensions;
using NLog;
using XtraSubReport.Winforms.Popups;
using XtraSubReport.Winforms.Prototypes;
using XtraSubReport.Winforms.Repositories;
using XtraSubReport.Winforms.Support;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;
using DesignDataRepository = XtraSubReport.Winforms.Repositories.DesignDataRepository;
//using SelectDesignTimeDataSourceForm = XtraSubReport.Winforms.Popups.SelectDesignTimeDataSourceForm;

namespace XtraSubReport.Winforms
{
    public static class Program
    {
        public static ActionMessageHandler ActionMessageHandler;
        public static DebugMessageHandler DebugDebugMessageHandler;
        private const string DefaultRootFolderName = "gcXtraReports\\ReportDesigner";
        private const string DataSourceDirectoryName = "Datasources";
        private const string ReportsDirectoryName = "Reports";
        private const string ActionsDirectoryName = "Actions";
        private const string BootStrapperBatchFileName = "bootstrapper.bat";

        // NLog - Helpful to diagnose if a DLL cannot be found, etc.
        private static Logger _logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupNLog();
            
            ProjectBootStrapper projectBootstrapper;
            if (InitUsingBootstrappers(out projectBootstrapper) == false) return;

            
            CompositeRoot.Init(BuildContainer(projectBootstrapper));
            var form = CompositeRoot.Instance.GetDesignForm();
            DebugDebugMessageHandler = CompositeRoot.Instance.GetDebugMessageHandler();
            ActionMessageHandler = CompositeRoot.Instance.GetActionMessageHandler();
 /*           // Runtime Actions
            var runtimeActions = new List<IReportRuntimeAction>()
            {
                new ReportRuntimeAction<XRLabel>(label => label.Name.Contains("gold"), label => label.BackColor = Color.Gold)
            };*/

/*            var rootProjectPath = string.Empty;
            List<IReportDatasourceProvider> datasourceProviders = null;

            var designerContext = new DesignerContext(runtimeActions, projectBootstrapper.ReportsFolderPath, rootProjectPath, datasourceProviders);*/

            Application.Run(form);
        }

        private static bool InitUsingBootstrappers(out ProjectBootStrapper projectBootstrapper)
        {
            projectBootstrapper = null;

            var bs = new AppBootStrapper(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DefaultRootFolderName));


            var mode = bs.DetectProjectMode();

            switch (mode)
            {
                case AppProjectsStructureMode.None:
                    new Popups.NoProjectsExistWarning(bs).ShowDialog();
                    return false;
                case AppProjectsStructureMode.MultipleUnchosen:
                    new Popups.ChooseProject(bs).ShowDialog();
                    if (bs.DetectProjectMode() == AppProjectsStructureMode.MultipleUnchosen)
                        return false;
                    break;
            }

            projectBootstrapper = bs.GetProjectBootstrapper(ReportsDirectoryName, DataSourceDirectoryName,
                                                                ActionsDirectoryName);

            projectBootstrapper.CreateFoldersIfNeeded();
            projectBootstrapper.ExecuteBootStrapperBatchFileIfExists(BootStrapperBatchFileName);
            return true;
        }

        private static IContainer BuildContainer(ProjectBootStrapper projectBootstrapper)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(projectBootstrapper.ActionAssemblies).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(projectBootstrapper.DataSourceAssemblies).AsImplementedInterfaces();
            builder.RegisterInstance(EventAggregator.Singleton).AsImplementedInterfaces();
            builder.RegisterType<XRMessagingDesignForm>().OnActivated(a=> DrawToolbarButtons(a.Instance));
            builder.RegisterType<DesignReportMetadataAssociationRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DesignDataRepository>().AsImplementedInterfaces().SingleInstance();
            //TODO: Make this work again builder.RegisterType<SelectDesignTimeDataSourceForm>();
            builder.RegisterType<DesignDataContext2>().SingleInstance();
            builder.RegisterType<ActionMessageHandler>().SingleInstance();
            builder.RegisterType<DebugMessageHandler>().SingleInstance();
            builder.RegisterType<ReportControllerFactory>().AsImplementedInterfaces();
            builder.RegisterType<ObjectGraphPathTraverser>().AsImplementedInterfaces();
            return builder.Build();
        }

        private static void DrawToolbarButtons(XRMessagingDesignForm form)
        {
            var dataContext = CompositeRoot.Instance.GetDesignDataContext();

            var item = new BarButtonItem(form.DesignBarManager, "See Messages");

            // Click Handler
            item.ItemClick += (s, e) => new ShowMessages(DebugDebugMessageHandler).ShowDialog();

            // Add Datasource Button
            form.DesignBarManager.Toolbar.AddItem(item);


            item = new BarButtonItem(form.DesignBarManager, "Select Datasource...");

            // Click Handler
            item.ItemClick += (s, e) =>
            {
                if (form.DesignMdiController.ActiveDesignPanel == null)
                {
                    MessageBox.Show("Please create/open a report.");
                    return;
                }

                var report = form.DesignMdiController.ActiveDesignPanel.Report;
                report.TryAs<MyReportBase>(myReport => PromptSelectDatasource(form, myReport,dataContext));
            };

            // Add Datasource Button
            form.DesignBarManager.Toolbar.AddItem(item);


        }


        private static void PromptSelectDatasource(XRDesignForm form, MyReportBase report, IDesignDataContext dataContext)
        {
            //TODO: Make this work again
 /*           Form dialog = null;



            // Datasource Selected Callback
            Action<ReportDatasourceMetadataWithTraversal> callback = d =>
            {
                EventAggregator.Singleton.Publish(new DataSourceSelectedForReportMessage(d,report));
                dialog.Close();
            };

            // Create Select Datasource Dialog
            dialog = new SelectDesignTimeDataSourceForm(dataContext, report, callback);
            dialog.BringToFront();
            dialog.ShowDialog();*/
        }


        private static void SetupNLog()
        {
            // Create a Logger
            _logger = LogManager.GetCurrentClassLogger();

            // Add the event handler for handling non-UI thread exceptions 
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                _logger.FatalException("Report Designer encountered unhandled exception", exception);
            };
        }




    }
}
