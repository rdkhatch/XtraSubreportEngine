using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XtraSubReport.Winforms.Support
{
    public class ProjectBootStrapper
    {
        private readonly string _projectPath;
        private readonly string _reportsFolderPath;
        private readonly string _datasourceFolderPath;
        private readonly string _actionsFolderPath;

        public ProjectBootStrapper(string projectPath, string reportsFolderName, string datasourceFolderName, string actionsFolderName)
        {
            if (String.IsNullOrWhiteSpace(reportsFolderName)) throw new ArgumentNullException("reportsFolderName");
            if (String.IsNullOrWhiteSpace(datasourceFolderName)) throw new ArgumentNullException("datasourceFolderName");
            if (String.IsNullOrWhiteSpace(actionsFolderName)) throw new ArgumentNullException("actionsFolderName");
            if (String.IsNullOrWhiteSpace(projectPath)) throw new ArgumentNullException("projectPath");


            _projectPath = projectPath;
            _reportsFolderPath = Path.Combine(_projectPath, reportsFolderName);
            _datasourceFolderPath = Path.Combine(_projectPath, datasourceFolderName);
            _actionsFolderPath = Path.Combine(_projectPath, actionsFolderName);
        }


        public Assembly[] ActionAssemblies
        {
            get { throw new NotImplementedException(); }
        }
        public Assembly[] DataSourceAssemblies
        {
            get { throw new NotImplementedException(); }
        }

        public string ActionsFolderPath
        {
            get { return _actionsFolderPath; }
        }

        public string DatasourceFolderPath
        {
            get { return _datasourceFolderPath; }
        }

        public string ReportsFolderPath
        {
            get { return _reportsFolderPath; }
        }

        public void CreateFoldersIfNeeded()
        {
            CreatePath(_reportsFolderPath);
            CreatePath(_datasourceFolderPath);
            CreatePath(_actionsFolderPath);
        }

        private static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void ExecuteBootStrapperBatchFileIfExists(string bootstrapperBat)
        {
         

            if(string.IsNullOrWhiteSpace(bootstrapperBat)) throw new ArgumentNullException("bootstrapperBat");

            var fullPath = Path.Combine(_projectPath, bootstrapperBat);

            if (!File.Exists(fullPath)) return;
            if (Path.GetExtension(fullPath).ToUpper() != ".BAT") return;

            var proc = new System.Diagnostics.Process
                           {
                               StartInfo =
                                   {
                                       FileName = fullPath,
                                       RedirectStandardError = false,
                                       RedirectStandardOutput = false,
                                       UseShellExecute = true,
                                       WorkingDirectory = _projectPath
                                   }
                           };
            proc.Start();
            proc.WaitForExit();
        }
    }
}