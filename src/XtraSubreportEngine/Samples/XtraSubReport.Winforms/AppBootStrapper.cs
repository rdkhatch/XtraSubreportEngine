﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XtraSubreport.Engine.Designer;

namespace XtraSubReport.Winforms
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

        public void ExecuteBatchFile(string bootstrapperBat)
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

    public class AppBootStrapper
    {
        private readonly string _defaultRootPath;
        private string _projectName;
        public string RootPath {get { return _defaultRootPath; }}

        public AppBootStrapper(string defaultRootPath)
        {
            _defaultRootPath = defaultRootPath;
        }

        public ProjectBootStrapper GetProjectBootstrapper(string reportsFolderName, string dataSourceFolderName, string actionsFolderName)
        {
            if(string.IsNullOrWhiteSpace(_projectName))
                throw new Exception("Project not set");

            return new ProjectBootStrapper(Path.Combine(_defaultRootPath,_projectName),reportsFolderName,dataSourceFolderName,actionsFolderName);
        }

        public void CreateRootPathIfNeeded()
        {
            CreatePath(_defaultRootPath); 
        }

        private static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public AppProjectsStructureMode DetectProjectMode()
        {
            var appProjectCount = GetProjects().Count();

            if (appProjectCount == 0) return AppProjectsStructureMode.None;
            if(appProjectCount == 1) return AppProjectsStructureMode.Single;

            if(string.IsNullOrEmpty(_projectName))
                return AppProjectsStructureMode.MultipleUnchosen;

            return AppProjectsStructureMode.MultipleChosen;
        }

        public IEnumerable<string> GetProjects()
        {
            CreateRootPathIfNeeded();
            return Directory.GetDirectories(_defaultRootPath);
        }

        public void SetProjectName(string item)
        {

            var path = Path.Combine(_defaultRootPath, item);

            if (Directory.Exists(path))
            {
                _projectName = item;    
            }
            else
            {
                throw new DirectoryNotFoundException("Project not found");
            }           
        }
    }
}