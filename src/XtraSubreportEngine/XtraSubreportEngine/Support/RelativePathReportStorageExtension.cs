using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraReports.Extensions;

namespace XtraSubreport.Engine.Support
{

    public class RelativePathReportStorage : ReportStorageExtension
    {
        string _relativeBasePath;
        string _executingAssemblyDirectory;
        string _fullBasePath;

        public RelativePathReportStorage(string relativeReportBasePath)
        {
            _relativeBasePath = relativeReportBasePath;
            _executingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var fullBasePathUgly = Path.Combine(_executingAssemblyDirectory, _relativeBasePath);
            _fullBasePath = new DirectoryInfo(fullBasePathUgly).FullName;
        }

        #region Open

        public override byte[] GetData(string url)
        {
            var convertedPath = ConvertToFullPath(url);
            return base.GetData(convertedPath);
        }

        // Gets URL to open. Really an open file dialog.
        public override string GetNewUrl()
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = _fullBasePath,
                Filter = "XtraReports|*.repx"
            };
            dialog.ShowDialog();

            // If cancelled / no file selected, exit
            if (string.IsNullOrWhiteSpace(dialog.FileName))
                return string.Empty;

            // Was file selected within Report base path?
            var selectedFullFilePath = dialog.FileName;
            if (isFullPathWithinBasePath(selectedFullFilePath) == false)
            {
                var message = String.Format("You selected a file outside the base path! Please select a file within the base path. Base Path = '{0}'  File = '{1}'", _fullBasePath, selectedFullFilePath);
                MessageBox.Show(message);
                return string.Empty;
            }

            // Convert full path to relative path
            var selectedRelativeFilePath = ConvertToRelativePath(selectedFullFilePath);

            return selectedRelativeFilePath;
        }

        #endregion


        #region Save

        // Save New
        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            var convertedURL = ConvertToFullPath(defaultUrl);
            return base.SetNewData(report, convertedURL);
        }

        // Save Existing
        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            var convertedURL = ConvertToFullPath(url);
            base.SetData(report, convertedURL);
        }

        #endregion

        public override bool IsValidUrl(string url)
        {
            var convertedURL = ConvertToFullPath(url);
            return base.IsValidUrl(convertedURL);
        }

        #region Helpers

        private string ConvertToFullPath(string relativePath)
        {
            // Strip ~\
            var toRemove = @"~\{0}\".FormatString(_relativeBasePath);
            relativePath = relativePath.Replace(toRemove, "");

            return Path.Combine(_relativeBasePath, relativePath);
        }

        private string ConvertToRelativePath(string fullPath)
        {
            var toRemove = @"{0}\".FormatString(_fullBasePath);
            return fullPath.Replace(toRemove, "");
        }

        private bool isFullPathWithinBasePath(string testFullPath)
        {
            return testFullPath.StartsWith(_fullBasePath);
        }

        #endregion
    }
}
