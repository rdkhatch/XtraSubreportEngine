using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XtraSubreport.Engine.Designer;

namespace XtraSubReport.Winforms
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

        public object GetProjectBootstrapper()
        {
            return _projectName;
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

            if (Directory.Exists(_defaultRootPath))
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
