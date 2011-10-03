using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraReports.Extensions;

namespace XtraSubreport.Engine.Support
{

    public class RelativePathReportStorage : ReportStorageExtension
    {
        string _basePath;

        public RelativePathReportStorage(string reportBasePath)
        {
            _basePath = reportBasePath;
        }

        #region Open

        public override byte[] GetData(string url)
        {
            return base.GetData(ConvertToFullPath(url));
        }

        // Gets URL to open. Really an open file dialog.
        public override string GetNewUrl()
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = _basePath,
                Filter = "XtraReports|*.repx"
            };
            dialog.ShowDialog();
            var selectedFilePath = dialog.FileName;

            if (selectedFilePath.StartsWith(_basePath) == false)
            {
                var message = String.Format("You selected a file outside the base path! Please select a file within the base path. Base Path = '{0}'  File = '{1}'", _basePath, selectedFilePath);
                MessageBox.Show(message);
                return string.Empty;
            }

            var selectedUrl = selectedFilePath.Remove(0, _basePath.Length);
            return selectedUrl;
        }

        #endregion


        #region Save

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            return base.SetNewData(report, ConvertToFullPath(defaultUrl));
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            base.SetData(report, ConvertToFullPath(url));
        }

        #endregion

        public override bool IsValidUrl(string url)
        {
            return base.IsValidUrl(ConvertToFullPath(url));
        }



        private string ConvertToFullPath(string relativePath)
        {
            return Path.Combine(_basePath, relativePath);
        }
    }
}
