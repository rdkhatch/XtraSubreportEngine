using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtraSubReport.Winforms.Prototypes
{
    public interface IFileAndDirectoryCloner
    {
        void Clone(string sourcePath, string destinationPath);
    }

    public interface IDynamicDllLoader
    {
        void LoadDllsInDirectory(string path);
    }
}
