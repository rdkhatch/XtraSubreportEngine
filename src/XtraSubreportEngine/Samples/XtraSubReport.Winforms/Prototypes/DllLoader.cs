using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace XtraSubReport.Winforms.Prototypes
{
    public class DllLoader : IDynamicDllLoader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private void LoadAssemblies(string path)
        {

            var assemblyFilePaths = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories).ToList();
            _logger.Trace("{0} dlls to load from path {1}", assemblyFilePaths.Count, path);
            for (var index = 0; index < assemblyFilePaths.Count; index++)
            {
                var file = assemblyFilePaths[index];
                _logger.Trace("Loading dll {0} or {1} from {2}", index + 1, assemblyFilePaths.Count);
                Assembly.LoadFrom(file);
            }
        }

        public void LoadDllsInDirectory(string path)
        {
            LoadAssemblies(path);
        }
    }
}