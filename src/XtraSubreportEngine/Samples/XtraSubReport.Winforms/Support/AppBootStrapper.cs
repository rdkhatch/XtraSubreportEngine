using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XtraSubReport.Winforms.Support
{
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
