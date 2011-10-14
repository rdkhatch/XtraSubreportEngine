using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NLog;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Designer;

namespace XtraSubReport.Winforms
{

    public static class Program
    {
        // NLog - Helpful to diagnose if a DLL cannot be found, etc.
        private static Logger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupNLog();

            // Reports
            var relativeReportBasePath = GetReportRelativeBasePath();

            // Datasources
            var relativeDatasourceBasePath = GetDatasourceRelativeBasePath();

            // Runtime Actions
            var runtimeActions = new List<IReportRuntimeAction>()
            {
                new ReportRuntimeActionBase<XRLabel>(label => label.Name.Contains("MakeMeGold"), label => label.BackColor = Color.Gold)
            };

            var designerContext = new DesignerContext(runtimeActions, relativeReportBasePath, relativeDatasourceBasePath);

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
