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
