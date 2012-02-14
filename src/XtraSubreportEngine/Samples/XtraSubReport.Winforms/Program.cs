using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NLog;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Designer;

namespace XtraSubReport.Winforms
{

    public static class Program
    {
        private const string DefaultRootFolderName = "gcXtraReports\\ReportDesigner";
        private const string DataSourceDirectoryName = "Datasources";
        private const string ReportsDirectoryName = "Reports";
        private const string ActionsDirectoryName = "Actions";
        private const string BootStrapperBatchFileName = "bootstrapper.bat";

        // NLog - Helpful to diagnose if a DLL cannot be found, etc.
        private static Logger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupNLog();

            var bs = new AppBootStrapper(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DefaultRootFolderName));


            var mode = bs.DetectProjectMode();

            switch (mode)
            {
                    case AppProjectsStructureMode.None:
                    new Popups.NoProjectsExistWarning(bs).ShowDialog();
                    return;
                    case AppProjectsStructureMode.MultipleUnchosen:
                    new Popups.ChooseProject(bs).ShowDialog();
                    if (bs.DetectProjectMode() == AppProjectsStructureMode.MultipleUnchosen)
                        return;
                    break;
            }

            var projectBootstrapper = bs.GetProjectBootstrapper(ReportsDirectoryName,DataSourceDirectoryName,ActionsDirectoryName);

            projectBootstrapper.CreateFoldersIfNeeded();
            projectBootstrapper.ExecuteBatchFile(BootStrapperBatchFileName);

            MessageBox.Show("NOT IMPLEMENTED", "Now we can wire this up...");// + bs.GetProjectBootstrapper().ToString());
            return;

            // Runtime Actions
            var runtimeActions = new List<IReportRuntimeAction>()
            {
                new ReportRuntimeAction<XRLabel>(label => label.Name.Contains("gold"), label => label.BackColor = Color.Gold)
            };

            var rootProjectPath = string.Empty;
            List<IReportDatasourceProvider> datasourceProviders = null;

            var designerContext = new DesignerContext(runtimeActions, projectBootstrapper.ReportsFolderPath, rootProjectPath, datasourceProviders);

            Application.Run(designerContext.DesignForm);
        }

        private static string GetReportRelativeBasePath()
        {
            //var appPath = Path.GetDirectoryName(Application.ExecutablePath);
            var configRelativeReportPath = ConfigurationManager.AppSettings["RelativeReportBasePath"];
            //var baseReportPathUgly = Path.Combine(appPath, configRelativeReportPath);
            //var directory = new DirectoryInfo(baseReportPathUgly);
            //var basePath = directory.FullName;

            //return basePath;
            return configRelativeReportPath;
        }

        private static string GetDatasourceRelativeBasePath()
        {
            // MEF Datasource Base Path
            var datasourceBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var relativePath = ConfigurationManager.AppSettings["RelativeDataSourcePath"];
            var baseDatasourcePath = Path.Combine(datasourceBasePath, relativePath);
            return baseDatasourcePath;
        }

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
