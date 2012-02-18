using System.IO;
using System.Linq;

namespace XtraSubReport.Winforms.Prototypes
{
    public class Cloner: IFileAndDirectoryCloner
    {
        // Copy directory structure recursively
        private void CopyDirectory(string src, string dst)
        {
            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dst)) Directory.CreateDirectory(dst);

            var files = Directory.GetFileSystemEntries(src);

            foreach (string fileName in files)
            {
                // Sub directories
                if (Directory.Exists(fileName))
                    CopyDirectory(fileName, dst + Path.GetFileName(fileName));
                    // Files in directory
                else
                    File.Copy(fileName, dst + Path.GetFileName(fileName), true);
            }
        }

        public void Clone(string sourcePath, string destinationPath)
        {
            CopyDirectory(sourcePath, destinationPath);
        }
    }
}