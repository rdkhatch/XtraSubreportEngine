using System.IO;
using System.Linq;
using System.Reflection;

namespace XtraSubReport.Winforms.Prototypes
{
    public class DllLoader : IDynamicDllLoader
    {
        private void LoadAssemblies(string path)
        {
            var assemblyFilePaths = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories).ToList();
            foreach (var file in assemblyFilePaths)
                Assembly.LoadFrom(file);
        }

        public void LoadDllsInDirectory(string path)
        {
            LoadAssemblies(path);
        }
    }
}