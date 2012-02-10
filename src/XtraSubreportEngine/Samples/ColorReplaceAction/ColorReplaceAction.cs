using System;
using System.Drawing;
using System.IO;
using System.Linq;
using ColorReplaceAction.Configuration;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace ColorReplaceAction
{
    [ReportRuntimeActionExportAttribute("Color Replacer", "Replaces ForeColors, BackColors, and BorderColors of any report label using a config file.")]
    public class ColorReplacerAction : ReportRuntimeActionBase<XRLabel>
    {
        private readonly ColorReplaceActionConfiguration _config;

        #region Constructors
        
        public ColorReplacerAction()
            : this(GetDefaultConfigFilePath())
        {
        }

        public ColorReplacerAction(string configFile)
            : this(ColorReplaceActionConfiguration.Deserialize(configFile))
        {
        }

        public ColorReplacerAction(ColorReplaceActionConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region Assets
        

        private static Color GetReplacementColor(Color color, ColorLocation location, ColorReplaceActionConfiguration config)
        {
            var q = from definition in config.ColorReplaceDefinitions
                    where location == definition.Location
                    where color.ToArgb() == definition.FromColor.ToArgb()
                    select definition;

            var colorChange = q.SingleOrDefault();

            return colorChange != null ? colorChange.ToColor : color;
        }

        private static string GetDefaultConfigFilePath()
        {
            // Default config file location
            var assemblyPath = Path.GetDirectoryName(typeof(ColorReplacerAction).Assembly.Location) + @"\";
            const string fileName = @"ColorReplaceAction.config";
            var filePath = System.IO.Path.Combine(assemblyPath, fileName);

            return filePath;
        }

        #endregion

        protected override void PerformAction(XRLabel control)
        {
            control.ForeColor = GetReplacementColor(control.ForeColor, ColorLocation.ForeColor, _config);
            control.BackColor = GetReplacementColor(control.BackColor, ColorLocation.BackColor, _config);
            control.BorderColor = GetReplacementColor(control.BorderColor, ColorLocation.BorderColor, _config);            
        }
    }
}