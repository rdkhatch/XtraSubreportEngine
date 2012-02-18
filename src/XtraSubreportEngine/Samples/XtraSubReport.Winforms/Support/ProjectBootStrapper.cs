using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using XtraSubReport.Winforms.Prototypes;

namespace XtraSubReport.Winforms.Support
{
    public class ProjectBootStrapper
    {
        private const string PluginsFolderName = "Plugins_Root";


        private Logger _logger;
        private readonly string _projectPath;
        private readonly string _reportsFolderName;
        private readonly string _datasourceFolderName;
        private readonly string _actionsFolderName;
        private readonly IFileAndDirectoryCloner _cloner;
        private readonly IDynamicDllLoader _loader;
        private string _reportsTargetFolderPath;
        private string _datasourceTargetFolderPath;
        private string _actionsTargetFolderPath;

        public ProjectBootStrapper(string projectPath, string reportsFolderName, string datasourceFolderName, string actionsFolderName, IFileAndDirectoryCloner cloner, IDynamicDllLoader loader)
        {
            if (String.IsNullOrWhiteSpace(reportsFolderName)) throw new ArgumentNullException("reportsFolderName");
            if (String.IsNullOrWhiteSpace(datasourceFolderName)) throw new ArgumentNullException("datasourceFolderName");
            if (String.IsNullOrWhiteSpace(actionsFolderName)) throw new ArgumentNullException("actionsFolderName");
            if (String.IsNullOrWhiteSpace(projectPath)) throw new ArgumentNullException("projectPath");

            _logger = LogManager.GetCurrentClassLogger();

            

            _projectPath = projectPath;
            _reportsFolderName = reportsFolderName;
            _datasourceFolderName = datasourceFolderName;
            _actionsFolderName = actionsFolderName;
            _cloner = cloner;
            _loader = loader;

            var pluginsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), PluginsFolderName);

            _logger.Trace("Project path: {0}", _projectPath);
            _logger.Trace("Plugins path: {0}", pluginsPath);

            _reportsTargetFolderPath = Path.Combine(pluginsPath, _reportsFolderName);
            _datasourceTargetFolderPath = Path.Combine(pluginsPath, _datasourceFolderName);
            _actionsTargetFolderPath = Path.Combine(pluginsPath, _actionsFolderName);

        }


        private void CreatePath(string path)
        {

            if (!Directory.Exists(path))
            {
                _logger.Trace("creating directory at {0}", path);
                Directory.CreateDirectory(path);
            }
        }

        public void ExecuteProjectBootStrapperFile(string bootstrapperBat)
        {
            if(string.IsNullOrWhiteSpace(bootstrapperBat)) throw new ArgumentNullException("bootstrapperBat");

            var fullPath = Path.Combine(_projectPath, bootstrapperBat);

            if (!File.Exists(fullPath))
            {
                _logger.Trace("bootstrapper at {0} was not found", fullPath);
                return;
            }
            if (Path.GetExtension(fullPath).ToUpper() != ".BAT")
            {
                _logger.Trace("bootstrapper file at {0} had incorrect extension", fullPath);
                return;
            }

            var proc = new System.Diagnostics.Process
                           {
                               StartInfo =
                                   {
                                       FileName = fullPath,
                                       RedirectStandardError = false,
                                       RedirectStandardOutput = true,
                                       UseShellExecute = true,
                                       WorkingDirectory = _projectPath
                                   }
                           };
            proc.Start();
            proc.WaitForExit();

            var output = proc.StandardOutput.ReadToEnd();

            _logger.Trace("output from bootstrapper: {0}", output);

        }

        public void CopyProjectFiles()
        {

            var reportsSourceFolderPath = Path.Combine(_projectPath, _reportsFolderName);
            var datasourceSourceFolderPath = Path.Combine(_projectPath, _datasourceFolderName);
            var actionsSourceFolderPath = Path.Combine(_projectPath, _actionsFolderName);

            CreatePath(_reportsTargetFolderPath);
            CreatePath(_datasourceTargetFolderPath);
            CreatePath(_actionsTargetFolderPath);

            CreatePath(reportsSourceFolderPath);
            CreatePath(datasourceSourceFolderPath);
            CreatePath(actionsSourceFolderPath);

            CloneFiles(reportsSourceFolderPath, _reportsTargetFolderPath);
            CloneFiles(datasourceSourceFolderPath, _datasourceTargetFolderPath);
            CloneFiles(actionsSourceFolderPath, _actionsTargetFolderPath);
        }

        private void CloneFiles (string sourceFolderName, string targetPath)
        {
            _logger.Trace("Cloning files from {0} to {1}", sourceFolderName, targetPath);
            CreatePath(targetPath);
            _cloner.Clone(sourceFolderName, targetPath);
        }


        public void LoadProjectAssemblies()
        {
            _loader.LoadDllsInDirectory(_datasourceTargetFolderPath);
            _loader.LoadDllsInDirectory(_actionsTargetFolderPath);
        }

    }
}