using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using XtraSubreport.Contracts.RuntimeActions;
using System.IO;
using System.Reflection;
using XtraSubreport.Engine.Support;

namespace XtraSubreport.Engine.RuntimeActions.Providers
{
    public class ReportMEFRuntimeActionProvider : IReportRuntimeActionProvider
    {
        CompositionContainer _container;

        public ReportMEFRuntimeActionProvider(CompositionContainer container)
        {
            _container = container;
        }

        public IEnumerable<IReportRuntimeAction> GetRuntimeActions()
        {
            IEnumerable<IReportRuntimeAction> result = null;

            try
            {
                var exports = _container.GetExports<IReportRuntimeAction>();

                result = (from export in exports
                          select export.Value).ToList();
            }
            catch (ReflectionTypeLoadException tLException)
            {
                MefHelper.ThrowReflectionTypeLoadException(tLException);
            }

            return result;
        }



        #region Create Catalog & Container Shortcut
        public ReportMEFRuntimeActionProvider(string directoryPath, bool recursive)
            : this(CreateDirectoryCatalog(directoryPath, recursive))
        {
        }

        private static CompositionContainer CreateDirectoryCatalog(string directoryPath, bool recursive)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("Runtime Action directory not found: " + directoryPath);

            var aggregate = new AggregateCatalog();
            var searchPattern = "*.dll";

            try
            {
                if (recursive)
                {
                    var q = from dir in Directory.EnumerateDirectories(directoryPath, "*", SearchOption.AllDirectories)
                            select new DirectoryCatalog(dir, searchPattern);

                    q.ToList().ForEach(catalog => aggregate.Catalogs.Add(catalog));
                }
                else
                    aggregate.Catalogs.Add(new DirectoryCatalog(directoryPath, searchPattern));
            }
            catch (ReflectionTypeLoadException tLException)
            {
                MefHelper.ThrowReflectionTypeLoadException(tLException);
            }

            return new CompositionContainer(aggregate);
        }
        #endregion
    }
}
