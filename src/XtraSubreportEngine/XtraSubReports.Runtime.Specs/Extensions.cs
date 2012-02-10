using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.Specs
{
    public static class Helpers
    {
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }


        public static string GetNewTempFile()
        {
            string path;

            do
            {
                path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            } while (Directory.Exists(path));

            return path;

        }
    }


}
