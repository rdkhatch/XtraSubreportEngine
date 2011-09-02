using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using XtraSubreport.Contracts;
using XtraSubreportEngine.Support;
using XtraSubreport.Engine;
using System.Reflection;
using System.Text;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Engine.Support;

namespace XtraSubreportEngine
{
    public static class DataSourceProvider
    {

        public static string _basePath { get; set; }

        static DataSourceProvider()
        {
            // Default Base Path
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SetBasePath(basePath);
        }


        public static void SetBasePath(string basePath)
        {
            // Always end a folder path with a slash
            // Important for telling files & folders apart
            if (!basePath.EndsWith(@"\"))
                basePath = basePath + @"\";

            _basePath = basePath;
        }

        public static string GetBasePath()
        {
            return GetDirectoryName(_basePath);
        }

        public static string GetDirectoryName(string filepath)
        {
            string path = Path.GetDirectoryName(filepath);

            // Always end a folder path with a slash
            // Important for telling files & folders apart
            if (!path.EndsWith(@"\"))
                path = path + @"\";

            return path;
        }

        public static IEnumerable<string> GetAllFoldersWithinBasePathContainingDLLs()
        {
            var basePath = GetBasePath();

            return (from filePath in Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories)
                    let folderName = GetDirectoryName(filePath)
                    select MakeRelativePath(folderName)
                   ).Distinct();
        }

        public static string FormatRelativePath(string relativePath)
        {
            var fullPath = MakeFullPath(relativePath);
            var result = MakeRelativePath(fullPath);
            return result;
        }

        public static string MakeFullPath(string relativePath)
        {
            var basePath = DataSourceProvider.GetBasePath();
            return Path.Combine(basePath, relativePath);
        }

        public static string MakeRelativePath(string fullPath)
        {
            var basePath = DataSourceProvider.GetBasePath();
            return MakeRelativePath(fullPath, basePath);
        }

        private static String MakeRelativePath(string fullPath, string relativetoPath)
        {
            if (String.IsNullOrEmpty(relativetoPath)) throw new ArgumentNullException("fullPath");
            if (String.IsNullOrEmpty(fullPath)) throw new ArgumentNullException("relativetoPath");

            if (!relativetoPath.EndsWith(@"\"))
                relativetoPath = relativetoPath + @"\";

            bool dontEscape = true;

            // Change Windows Slashes into URI slashes
            fullPath = fullPath.Replace(@"\", "/");
            relativetoPath = relativetoPath.Replace(@"\", "/");

            Uri fromUri = new Uri(relativetoPath, dontEscape);
            Uri toUri = new Uri(fullPath, dontEscape);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            // Change URI slahes back into Windows Slashes
            return relativeUri.ToString().Replace("/", @"\");
        }

        private static IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> GetDatasources(CompositionContainer container)
        {
            return container.GetExports<IReportDatasource, IReportDatasourceMetadata>();
        }

        public static IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> GetDatasources(string relativeFolder)
        {
            var fullPath = MakeFullPath(relativeFolder);

            IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> exports = new Lazy<IReportDatasource, IReportDatasourceMetadata>[] {};

            // Make sure directory exists
            if (!Directory.Exists(fullPath)) return exports;

            try
            {
                var catalog = new DirectoryCatalog(fullPath);

                var files = catalog.LoadedFiles;

                var container = new CompositionContainer(catalog);
                exports = GetDatasources(container);
            }
            catch (ReflectionTypeLoadException tLException)
            {
                MefHelper.ThrowReflectionTypeLoadException(tLException);
            } 

            return exports;
        }

        public static Lazy<IReportDatasource, IReportDatasourceMetadata> GetDatasource(DesignTimeDataSourceDefinition definition)
        {
            return GetDatasource(definition.DataSourceAssemblyLocationPath, definition.DataSourceName);
        }

        public static Lazy<IReportDatasource, IReportDatasourceMetadata> GetDatasource(string folderPath, string datasourceName)
        {
            var match = (from export in GetDatasources(folderPath)
                         where export.Metadata.Name == datasourceName
                         select export).SingleOrDefault();

            return match;
        }

        public static object GetObjectFromDataSourceDefinition(DesignTimeDataSourceDefinition datasource)
        {
            var rootDataSourceInterface = GetDatasource(datasource.DataSourceAssemblyLocationPath, datasource.DataSourceName);
            if (rootDataSourceInterface == null) return null;

            var rootDataSource = rootDataSourceInterface.Value.DataSource;
            var targetDataSource = ObjectGraphPathTraverser.TraversePath(rootDataSource, datasource.DataSourceRelationPath);
            return targetDataSource;
        }

    }
}