using System.IO;
using System.Linq;
using NLog;

namespace XtraSubReport.Winforms.Prototypes
{
    public class Cloner: IFileAndDirectoryCloner
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        // Copy directory structure recursively
        private void CopyDirectory(string src, string dst)
        {
            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dst))
            {
                _logger.Trace("Creating destination directory at {0}", dst);
                Directory.CreateDirectory(dst);
            }

            var files = Directory.GetFileSystemEntries(src);

            foreach (string fileName in files)
            {
                // Sub directories
                if (Directory.Exists(fileName))
                    CopyDirectory(fileName, dst + Path.GetFileName(fileName));
                // Files in directory
                else
                {
                    var dest = dst + Path.GetFileName(fileName);
                    _logger.Trace("Copying file from {0} to {1}", fileName,dest);
                    File.Copy(fileName, dest , true);
                }
            }
        }

        public void Clone(string sourcePath, string destinationPath)
        {
            CopyDirectory(sourcePath, destinationPath);
        }
    }
}