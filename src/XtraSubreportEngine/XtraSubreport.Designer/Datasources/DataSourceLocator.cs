using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreportEngine
{
    public class DataSourceLocator
    {

        private string _fullBasePath;

        public DataSourceLocator(string fullBasePath)
        {
            // Default Base Path
            if (String.IsNullOrWhiteSpace(fullBasePath))
                fullBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _fullBasePath = MakeIntoFolderPath(fullBasePath);
        }

        #region Locate Datasource

        private IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> GetDatasources(CompositionContainer container)
        {
            return container.GetExports<IReportDatasource, IReportDatasourceMetadata>();
        }

        public IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> GetDatasources(string relativeFolder)
        {
            var fullPath = MakeFullPath(relativeFolder);

            IEnumerable<Lazy<IReportDatasource, IReportDatasourceMetadata>> exports = new Lazy<IReportDatasource, IReportDatasourceMetadata>[] { };

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

        public Lazy<IReportDatasource, IReportDatasourceMetadata> GetDatasource(DesignTimeDataSourceDefinition definition)
        {
            return GetDatasource(definition.DataSourceAssemblyLocationPath, definition.DataSourceName);
        }

        public Lazy<IReportDatasource, IReportDatasourceMetadata> GetDatasource(string folderPath, string datasourceName)
        {
            var match = (from export in GetDatasources(folderPath)
                         where export.Metadata.Name == datasourceName
                         select export).SingleOrDefault();

            return match;
        }

        public TraversedDatasourceResult GetTraversedObjectFromDataSourceDefinition(DesignTimeDataSourceDefinition definition)
        {
            if (definition == null)
                return new TraversedDatasourceResult(null, null, null);

            // Get Export
            var export = GetDatasource(definition.DataSourceAssemblyLocationPath, definition.DataSourceName);
            var exportInstance = export.Value;

            // Get Datasource
            var rootDataSource = exportInstance.GetDataSource();

            // Traverse Relation Path
            var targetDataSource = ObjectGraphPathTraverser.TraversePath(rootDataSource, definition.DataSourceRelationPath);

            // Assign Datasource Types to DesignTimeDatasource, not that we've obtained the datasource & traversed the relation path
            definition.RootDataSourceType = null;
            definition.DataSourceType = null;

            if (rootDataSource != null) definition.RootDataSourceType = rootDataSource.GetType();
            if (targetDataSource != null) definition.DataSourceType = targetDataSource.GetType();

            return new TraversedDatasourceResult(definition, rootDataSource, targetDataSource);
        }

        public class TraversedDatasourceResult
        {
            public DesignTimeDataSourceDefinition Definition { get; private set; }
            public object RootDataSource { get; private set; }
            public object TraversedDataSource { get; private set; }

            public bool Succeeded
            {
                get { return RootDataSource != null && TraversedDataSource != null; }
            }

            public TraversedDatasourceResult(DesignTimeDataSourceDefinition definition, object rootDataSource, object traversedDataSource)
            {
                Definition = definition;
                RootDataSource = rootDataSource;
                TraversedDataSource = traversedDataSource;
            }
        }

        #endregion



        #region Paths

        public string GetBasePath()
        {
            return GetDirectoryName(_fullBasePath);
        }

        private static string MakeIntoFolderPath(string path)
        {
            // Always end a folder path with a slash
            // Important for telling files & folders apart
            if (!path.EndsWith(@"\"))
                path = path + @"\";

            return path;
        }

        public static string GetDirectoryName(string filepath)
        {
            string path = Path.GetDirectoryName(filepath);
            path = MakeIntoFolderPath(path);
            return path;
        }

        public IEnumerable<string> GetAllFoldersWithinBasePathContainingDLLs()
        {
            var basePath = GetBasePath();

            return (from filePath in Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories)
                    let folderName = GetDirectoryName(filePath)
                    select MakeRelativePath(folderName)
                   ).Distinct();
        }

        public string FormatRelativePath(string relativePath)
        {
            var fullPath = MakeFullPath(relativePath);
            var result = MakeRelativePath(fullPath);
            return result;
        }

        public string MakeFullPath(string relativePath)
        {
            return Path.Combine(GetBasePath(), relativePath);
        }

        public string MakeRelativePath(string fullPath)
        {
            return MakeRelativePath(fullPath, GetBasePath());
        }

        private static String MakeRelativePath(string fullPath, string relativetoPath)
        {
            if (String.IsNullOrEmpty(relativetoPath)) throw new ArgumentNullException("fullPath");
            if (String.IsNullOrEmpty(fullPath)) throw new ArgumentNullException("relativetoPath");

            relativetoPath = MakeIntoFolderPath(relativetoPath);

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

        #endregion

    }
}